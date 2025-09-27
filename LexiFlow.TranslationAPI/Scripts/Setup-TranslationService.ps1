# LexiFlow Translation Service Setup Script
# Run as Administrator

param(
    [switch]$SkipPythonInstall,
    [switch]$ServiceOnly
)

Write-Host "=== LexiFlow Translation Service Setup ===" -ForegroundColor Cyan

# Check if running as admin
if (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Host "Please run this script as Administrator!" -ForegroundColor Red
    exit 1
}

$scriptPath = $PSScriptRoot
$pythonPath = Join-Path $scriptPath "..\Python"
$servicePath = "C:\LexiFlow\TranslationService"

try {
    # Create service directory
    Write-Host "Creating service directory..." -ForegroundColor Yellow
    if (!(Test-Path $servicePath)) {
        New-Item -ItemType Directory -Path $servicePath -Force | Out-Null
    }

    # Install Python dependencies
    if (-not $SkipPythonInstall) {
        Write-Host "Installing Python dependencies..." -ForegroundColor Yellow
        
        $requirementsPath = Join-Path $pythonPath "requirements.txt"
        if (Test-Path $requirementsPath) {
            pip install -r $requirementsPath --no-warn-script-location
        } else {
            pip install flask transformers torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu
            pip install pywin32
        }
    }

    # Copy Python files
    Write-Host "Copying service files..." -ForegroundColor Yellow
    Copy-Item -Path "$pythonPath\*" -Destination $servicePath -Recurse -Force

    # Create and install Windows service
    if (-not $ServiceOnly) {
        Write-Host "Installing Windows service..." -ForegroundColor Yellow
        
        $serviceScript = @"
import win32serviceutil
import win32service
import win32event
import servicemanager
import sys
import os
import subprocess
import time
import logging

class LexiFlowTranslationService(win32serviceutil.ServiceFramework):
    _svc_name_ = "LexiFlowTranslationService"
    _svc_display_name_ = "LexiFlow Translation Service"
    _svc_description_ = "Machine Translation Service using Facebook M2M-100 model"

    def __init__(self, args):
        win32serviceutil.ServiceFramework.__init__(self, args)
        self.hWaitStop = win32event.CreateEvent(None, 0, 0, None)
        self.is_alive = True
        self.process = None

    def SvcStop(self):
        self.ReportServiceStatus(win32service.SERVICE_STOP_PENDING)
        win32event.SetEvent(self.hWaitStop)
        self.is_alive = False
        if self.process:
            self.process.terminate()

    def SvcDoRun(self):
        servicemanager.LogMsg(
            servicemanager.EVENTLOG_INFORMATION_TYPE,
            servicemanager.PYS_SERVICE_STARTED,
            (self._svc_name_, '')
        )
        self.main()

    def main(self):
        script_path = os.path.join(r"$servicePath", 'm2m100_service.py')
        python_path = sys.executable
        
        while self.is_alive:
            try:
                self.process = subprocess.Popen([
                    python_path, script_path
                ], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
                
                servicemanager.LogInfoMsg("Translation service started")
                
                while self.is_alive:
                    if self.process.poll() is not None:
                        servicemanager.LogErrorMsg("Translation service crashed, restarting...")
                        time.sleep(5)
                        break
                    time.sleep(1)
                    
            except Exception as e:
                servicemanager.LogErrorMsg(f"Error in translation service: {str(e)}")
                time.sleep(10)

if __name__ == '__main__':
    win32serviceutil.HandleCommandLine(LexiFlowTranslationService)
"@

        $serviceScript | Out-File -FilePath "$servicePath\windows_service.py" -Encoding UTF8
        
        # Install the service
        Set-Location $servicePath
        python windows_service.py install
        
        # Start the service
        Start-Service -Name "LexiFlowTranslationService" -ErrorAction SilentlyContinue
    }

    # Create startup task
    Write-Host "Creating startup task..." -ForegroundColor Yellow
    $taskXml = @"
<?xml version="1.0" encoding="UTF-16"?>
<Task version="1.2" xmlns="http://schemas.microsoft.com/windows/2004/02/mit/task">
  <RegistrationInfo>
    <Description>Auto-start LexiFlow Translation Service</Description>
  </RegistrationInfo>
  <Triggers>
    <BootTrigger>
      <Enabled>true</Enabled>
    </BootTrigger>
  </Triggers>
  <Principals>
    <Principal id="Author">
      <UserId>S-1-5-18</UserId>
      <RunLevel>HighestAvailable</RunLevel>
    </Principal>
  </Principals>
  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
    <AllowHardTerminate>true</AllowHardTerminate>
    <StartWhenAvailable>true</StartWhenAvailable>
    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
    <AllowStartOnDemand>true</AllowStartOnDemand>
    <Enabled>true</Enabled>
    <Hidden>false</Hidden>
    <RunOnlyIfIdle>false</RunOnlyIfIdle>
    <WakeToRun>false</WakeToRun>
    <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
    <Priority>7</Priority>
  </Settings>
  <Actions>
    <Exec>
      <Command>net</Command>
      <Arguments>start LexiFlowTranslationService</Arguments>
    </Exec>
  </Actions>
</Task>
"@

    $taskXml | Out-File -FilePath "$servicePath\startup_task.xml" -Encoding UTF8
    schtasks /create /xml "$servicePath\startup_task.xml" /tn "LexiFlowTranslationAutoStart" /f

    Write-Host "=== Setup Completed Successfully! ===" -ForegroundColor Green
    Write-Host "Service URL: http://127.0.0.1:5001" -ForegroundColor Cyan
    Write-Host "Health Check: curl http://127.0.0.1:5001/health" -ForegroundColor White
    
} catch {
    Write-Host "Setup failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}