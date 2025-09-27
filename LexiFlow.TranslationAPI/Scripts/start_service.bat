@echo off
setlocal

echo ========================================
echo LexiFlow Translation Service Manager
echo ========================================

:: Check for Administrator privileges
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERROR: This script must be run as Administrator!
    echo Right-click and select "Run as administrator"
    pause
    exit /b 1
)

:MENU
echo.
echo Select an option:
echo 1. Start Service
echo 2. Stop Service  
echo 3. Restart Service
echo 4. Check Status
echo 5. View Logs
echo 6. Test Health
echo 7. Exit
echo.
set /p choice=Enter your choice (1-7): 

if "%choice%"=="1" goto START
if "%choice%"=="2" goto STOP
if "%choice%"=="3" goto RESTART
if "%choice%"=="4" goto STATUS
if "%choice%"=="5" goto LOGS
if "%choice%"=="6" goto HEALTH
if "%choice%"=="7" goto EXIT

echo Invalid choice. Please try again.
goto MENU

:START
echo.
echo Starting LexiFlow Translation Service...
net start LexiFlowTranslationService
if %errorLevel% equ 0 (
    echo Service started successfully!
) else (
    echo Failed to start service. Error code: %errorLevel%
)
goto MENU

:STOP
echo.
echo Stopping LexiFlow Translation Service...
net stop LexiFlowTranslationService
if %errorLevel% equ 0 (
    echo Service stopped successfully!
) else (
    echo Failed to stop service. Error code: %errorLevel%
)
goto MENU

:RESTART
echo.
echo Restarting LexiFlow Translation Service...
net stop LexiFlowTranslationService
timeout /t 3 /nobreak >nul
net start LexiFlowTranslationService
if %errorLevel% equ 0 (
    echo Service restarted successfully!
) else (
    echo Failed to restart service. Error code: %errorLevel%
)
goto MENU

:STATUS
echo.
echo Checking service status...
sc query LexiFlowTranslationService
echo.
echo Service configuration:
sc qc LexiFlowTranslationService
goto MENU

:LOGS
echo.
echo Opening log directory...
set LOG_DIR=C:\LexiFlow\TranslationService\logs
if exist "%LOG_DIR%" (
    explorer "%LOG_DIR%"
) else (
    set LOG_DIR=C:\LexiFlow\TranslationService
    if exist "%LOG_DIR%\service.log" (
        explorer "%LOG_DIR%"
    ) else (
        echo Log directory not found. Service may not be installed correctly.
    )
)
goto MENU

:HEALTH
echo.
echo Testing service health...
echo Waiting for service to respond...

:: Wait a moment for service to be ready
timeout /t 2 /nobreak >nul

:: Test health endpoint using curl
curl -s http://127.0.0.1:5001/health
if %errorLevel% equ 0 (
    echo.
    echo Service is responding!
) else (
    echo Service is not responding. Checking if it's running...
    sc query LexiFlowTranslationService | find "RUNNING" >nul
    if %errorLevel% equ 0 (
        echo Service is running but not responding on port 5001
        echo It may still be initializing the ML model...
    ) else (
        echo Service is not running
    )
)

:: Test with PowerShell as backup
if %errorLevel% neq 0 (
    echo.
    echo Trying alternative health check...
    powershell -Command "try { $response = Invoke-WebRequest -Uri 'http://127.0.0.1:5001/health' -TimeoutSec 5; Write-Host 'Health check successful:'; Write-Host $response.Content } catch { Write-Host 'Health check failed:' $_.Exception.Message }"
)

goto MENU

:EXIT
echo.
echo Goodbye!
exit /b 0