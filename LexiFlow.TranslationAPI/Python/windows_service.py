import win32serviceutil
import win32service
import win32event
import servicemanager
import sys
import os
import subprocess
import time
import logging
import signal
from pathlib import Path

class LexiFlowTranslationService(win32serviceutil.ServiceFramework):
    _svc_name_ = "LexiFlowTranslationService"
    _svc_display_name_ = "LexiFlow Translation Service"
    _svc_description_ = "Machine Translation Service using Facebook M2M-100 model for LexiFlow application"

    def __init__(self, args):
        win32serviceutil.ServiceFramework.__init__(self, args)
        self.hWaitStop = win32event.CreateEvent(None, 0, 0, None)
        self.is_alive = True
        self.process = None
        self.restart_count = 0
        self.max_restarts = 5
        self.setup_logging()

    def setup_logging(self):
        """Setup logging for the Windows service"""
        log_path = Path(__file__).parent / 'service.log'
        logging.basicConfig(
            level=logging.INFO,
            format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
            handlers=[
                logging.FileHandler(str(log_path)),
                logging.StreamHandler()
            ]
        )
        self.logger = logging.getLogger('LexiFlowTranslationService')

    def SvcStop(self):
        """Handle service stop request"""
        self.logger.info("Service stop requested")
        self.ReportServiceStatus(win32service.SERVICE_STOP_PENDING)
        
        # Set the stop event
        win32event.SetEvent(self.hWaitStop)
        self.is_alive = False
        
        # Terminate the Python process gracefully
        if self.process:
            try:
                self.logger.info("Terminating Python translation service...")
                self.process.terminate()
                
                # Wait up to 10 seconds for graceful shutdown
                try:
                    self.process.wait(timeout=10)
                except subprocess.TimeoutExpired:
                    self.logger.warning("Process didn't terminate gracefully, killing it...")
                    self.process.kill()
                    
                self.logger.info("Python translation service stopped")
            except Exception as e:
                self.logger.error(f"Error stopping process: {e}")

    def SvcDoRun(self):
        """Main service entry point"""
        try:
            servicemanager.LogMsg(
                servicemanager.EVENTLOG_INFORMATION_TYPE,
                servicemanager.PYS_SERVICE_STARTED,
                (self._svc_name_, '')
            )
            self.logger.info("LexiFlow Translation Service starting...")
            self.main()
        except Exception as e:
            self.logger.error(f"Service startup error: {e}")
            servicemanager.LogErrorMsg(f"Service startup error: {e}")

    def main(self):
        """Main service loop"""
        # Get paths
        service_dir = Path(__file__).parent
        script_path = service_dir / 'm2m100_service.py'
        python_path = sys.executable
        
        if not script_path.exists():
            self.logger.error(f"Translation script not found: {script_path}")
            return

        self.logger.info(f"Using Python: {python_path}")
        self.logger.info(f"Script path: {script_path}")
        self.logger.info(f"Working directory: {service_dir}")

        # Change to service directory
        os.chdir(str(service_dir))

        while self.is_alive and self.restart_count < self.max_restarts:
            try:
                self.logger.info(f"Starting translation service (attempt {self.restart_count + 1})")
                
                # Start the Flask service
                self.process = subprocess.Popen([
                    str(python_path), str(script_path)
                ], 
                stdout=subprocess.PIPE, 
                stderr=subprocess.PIPE,
                cwd=str(service_dir),
                creationflags=subprocess.CREATE_NO_WINDOW  # Hide console window
                )
                
                servicemanager.LogInfoMsg("Translation service process started")
                self.logger.info(f"Translation service started with PID: {self.process.pid}")
                
                # Monitor the process
                while self.is_alive:
                    # Check if service stop was requested
                    if win32event.WaitForSingleObject(self.hWaitStop, 1000) == win32event.WAIT_OBJECT_0:
                        self.logger.info("Stop event received")
                        break
                    
                    # Check if process is still running
                    if self.process.poll() is not None:
                        # Process has terminated
                        stdout, stderr = self.process.communicate()
                        
                        self.logger.error("Translation service process terminated unexpectedly")
                        if stdout:
                            self.logger.error(f"STDOUT: {stdout.decode('utf-8', errors='ignore')}")
                        if stderr:
                            self.logger.error(f"STDERR: {stderr.decode('utf-8', errors='ignore')}")
                        
                        servicemanager.LogErrorMsg("Translation service crashed, attempting restart...")
                        self.restart_count += 1
                        
                        if self.restart_count < self.max_restarts:
                            self.logger.info(f"Restarting in 5 seconds... (attempt {self.restart_count + 1}/{self.max_restarts})")
                            time.sleep(5)
                        break
                
                # Reset restart counter on successful run (longer than 5 minutes)
                if self.process and self.process.poll() is None:
                    # Process is still running after the loop, reset counter
                    self.restart_count = 0
                    
            except Exception as e:
                self.logger.error(f"Error in service loop: {e}")
                servicemanager.LogErrorMsg(f"Error in translation service: {e}")
                self.restart_count += 1
                
                if self.restart_count < self.max_restarts:
                    time.sleep(10)  # Wait longer on exception
        
        if self.restart_count >= self.max_restarts:
            self.logger.error("Maximum restart attempts reached, service stopping")
            servicemanager.LogErrorMsg("Translation service failed to start after maximum retries")
        
        self.logger.info("LexiFlow Translation Service stopped")

def install_service():
    """Install the Windows service"""
    try:
        win32serviceutil.InstallService(
            LexiFlowTranslationService._svc_reg_class_,
            LexiFlowTranslationService._svc_name_,
            LexiFlowTranslationService._svc_display_name_,
            description=LexiFlowTranslationService._svc_description_
        )
        print(f"Service '{LexiFlowTranslationService._svc_display_name_}' installed successfully")
    except Exception as e:
        print(f"Failed to install service: {e}")

def remove_service():
    """Remove the Windows service"""
    try:
        win32serviceutil.RemoveService(LexiFlowTranslationService._svc_name_)
        print(f"Service '{LexiFlowTranslationService._svc_display_name_}' removed successfully")
    except Exception as e:
        print(f"Failed to remove service: {e}")

def start_service():
    """Start the Windows service"""
    try:
        win32serviceutil.StartService(LexiFlowTranslationService._svc_name_)
        print(f"Service '{LexiFlowTranslationService._svc_display_name_}' started successfully")
    except Exception as e:
        print(f"Failed to start service: {e}")

def stop_service():
    """Stop the Windows service"""
    try:
        win32serviceutil.StopService(LexiFlowTranslationService._svc_name_)
        print(f"Service '{LexiFlowTranslationService._svc_display_name_}' stopped successfully")
    except Exception as e:
        print(f"Failed to stop service: {e}")

if __name__ == '__main__':
    if len(sys.argv) == 1:
        # Run interactive mode
        servicemanager.Initialize()
        servicemanager.PrepareToHostSingle(LexiFlowTranslationService)
        servicemanager.StartServiceCtrlDispatcher()
    else:
        # Handle command line arguments
        cmd = sys.argv[1].lower()
        if cmd == 'install':
            install_service()
        elif cmd == 'remove':
            remove_service()
        elif cmd == 'start':
            start_service()
        elif cmd == 'stop':
            stop_service()
        else:
            win32serviceutil.HandleCommandLine(LexiFlowTranslationService)