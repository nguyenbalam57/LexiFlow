@echo off
setlocal enabledelayedexpansion

echo ========================================
echo LexiFlow Translation Service Installer
echo ========================================

:: Check for Administrator privileges
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERROR: This script must be run as Administrator!
    echo Right-click and select "Run as administrator"
    pause
    exit /b 1
)

:: Set paths
set SCRIPT_DIR=%~dp0
set PYTHON_DIR=%SCRIPT_DIR%..\Python
set SERVICE_DIR=C:\LexiFlow\TranslationService
set LOG_DIR=%SERVICE_DIR%\logs

echo Script directory: %SCRIPT_DIR%
echo Python directory: %PYTHON_DIR%
echo Service directory: %SERVICE_DIR%

:: Create service directory
echo.
echo Creating service directory...
if not exist "%SERVICE_DIR%" (
    mkdir "%SERVICE_DIR%"
    echo Created: %SERVICE_DIR%
) else (
    echo Directory already exists: %SERVICE_DIR%
)

:: Create logs directory
if not exist "%LOG_DIR%" (
    mkdir "%LOG_DIR%"
    echo Created: %LOG_DIR%
)

:: Copy Python files
echo.
echo Copying Python service files...
if exist "%PYTHON_DIR%" (
    xcopy "%PYTHON_DIR%\*" "%SERVICE_DIR%\" /E /Y /Q
    echo Python files copied successfully
) else (
    echo ERROR: Python directory not found: %PYTHON_DIR%
    pause
    exit /b 1
)

:: Check if Python is installed
python --version >nul 2>&1
if %errorLevel% neq 0 (
    echo ERROR: Python is not installed or not in PATH!
    echo Please install Python 3.8 or later from https://python.org
    pause
    exit /b 1
)

:: Install Python dependencies
echo.
echo Installing Python dependencies...
cd /d "%SERVICE_DIR%"

if exist "requirements.txt" (
    pip install -r requirements.txt --no-warn-script-location
    if !errorLevel! neq 0 (
        echo ERROR: Failed to install Python dependencies
        pause
        exit /b 1
    )
    echo Dependencies installed successfully
) else (
    echo Installing basic dependencies...
    pip install flask transformers torch torchvision torchaudio pywin32 --no-warn-script-location
    if !errorLevel! neq 0 (
        echo ERROR: Failed to install Python dependencies
        pause
        exit /b 1
    )
)

:: Install Windows Service
echo.
echo Installing Windows Service...
if exist "windows_service.py" (
    python windows_service.py install
    if !errorLevel! neq 0 (
        echo ERROR: Failed to install Windows service
        pause
        exit /b 1
    )
    echo Windows service installed successfully
) else (
    echo ERROR: windows_service.py not found
    pause
    exit /b 1
)

:: Configure service to start automatically
echo.
echo Configuring service for automatic startup...
sc config LexiFlowTranslationService start= auto
sc description LexiFlowTranslationService "Machine Translation Service using Facebook M2M-100 model for LexiFlow application"

:: Start the service
echo.
echo Starting LexiFlow Translation Service...
net start LexiFlowTranslationService
if %errorLevel% equ 0 (
    echo Service started successfully!
) else (
    echo Warning: Service installation completed but failed to start
    echo You can start it manually using: net start LexiFlowTranslationService
)

:: Create uninstall script
echo.
echo Creating uninstall script...
(
echo @echo off
echo echo Stopping LexiFlow Translation Service...
echo net stop LexiFlowTranslationService
echo echo Removing Windows Service...
echo cd /d "%SERVICE_DIR%"
echo python windows_service.py remove
echo echo Service removed successfully
echo pause
) > "%SERVICE_DIR%\uninstall_service.bat"

:: Test service health
echo.
echo Testing service health...
timeout /t 10 /nobreak >nul
curl -s http://127.0.0.1:5001/health >nul 2>&1
if %errorLevel% equ 0 (
    echo Service is running and healthy!
    echo API URL: http://127.0.0.1:5001
) else (
    echo Service may still be starting up...
    echo You can check the status with: sc query LexiFlowTranslationService
)

echo.
echo ========================================
echo Installation completed successfully!
echo ========================================
echo.
echo Service Details:
echo   Name: LexiFlowTranslationService
echo   URL: http://127.0.0.1:5001
echo   Directory: %SERVICE_DIR%
echo   Status: sc query LexiFlowTranslationService
echo   Logs: %LOG_DIR%
echo.
echo Management Commands:
echo   Start:     net start LexiFlowTranslationService
echo   Stop:      net stop LexiFlowTranslationService  
echo   Uninstall: %SERVICE_DIR%\uninstall_service.bat
echo.
echo The service will automatically start when Windows starts.
echo.
pause