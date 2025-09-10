@echo off
REM ============================================================================
REM BATCH FILE T?O MIGRATION LEXIFLOW - WINDOWS
REM Phiên b?n: .NET 9 - Entity Framework Core 9
REM S? d?ng: create-lexiflow-migration.bat [MigrationName]
REM ============================================================================

setlocal enabledelayedexpansion

REM Ki?m tra tham s?
set MIGRATION_NAME=%1
if "%MIGRATION_NAME%"=="" set MIGRATION_NAME=LexiFlowInitialMigration

REM ???ng d?n projects
set INFRASTRUCTURE_PROJECT=LexiFlow.Infrastructure
set STARTUP_PROJECT=LexiFlow.API

echo ========================================
echo    LEXIFLOW MIGRATION CREATOR
echo ========================================
echo Migration Name: %MIGRATION_NAME%
echo Infrastructure: %INFRASTRUCTURE_PROJECT%
echo Startup: %STARTUP_PROJECT%
echo ========================================
echo.

REM Ki?m tra .NET version
echo [INFO] Checking .NET version...
dotnet --version
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] .NET CLI not found! Please install .NET 9 SDK
    pause
    exit /b 1
)

REM Ki?m tra EF Tools
echo [INFO] Checking Entity Framework Tools...
dotnet ef --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Entity Framework Tools not found!
    echo [INFO] Installing EF Tools...
    dotnet tool install --global dotnet-ef
    if !ERRORLEVEL! NEQ 0 (
        echo [ERROR] Failed to install EF Tools
        pause
        exit /b 1
    )
)

REM Ki?m tra project files
echo [INFO] Checking project files...
if not exist "%INFRASTRUCTURE_PROJECT%\LexiFlow.Infrastructure.csproj" (
    echo [ERROR] Infrastructure project not found: %INFRASTRUCTURE_PROJECT%
    pause
    exit /b 1
)

if not exist "%STARTUP_PROJECT%\LexiFlow.API.csproj" (
    echo [ERROR] Startup project not found: %STARTUP_PROJECT%
    pause
    exit /b 1
)

echo [SUCCESS] All project files found!

REM Build project ?? ki?m tra l?i
echo [INFO] Building infrastructure project...
dotnet build %INFRASTRUCTURE_PROJECT% --verbosity quiet
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Build failed! Please fix compilation errors first.
    echo [INFO] You can skip this check by adding 'skipbuild' parameter
    if not "%2"=="skipbuild" (
        pause
        exit /b 1
    )
)

echo [SUCCESS] Build completed!

REM Backup migrations c? n?u có
if exist "%INFRASTRUCTURE_PROJECT%\Migrations" (
    echo [WARNING] Existing migrations found!
    set /p BACKUP="Create backup before proceeding? (y/n): "
    if /i "!BACKUP!"=="y" (
        set BACKUP_DIR=%INFRASTRUCTURE_PROJECT%\Migrations.backup.%date:~6,4%%date:~3,2%%date:~0,2%_%time:~0,2%%time:~3,2%%time:~6,2%
        set BACKUP_DIR=!BACKUP_DIR: =0!
        echo [INFO] Creating backup at: !BACKUP_DIR!
        xcopy "%INFRASTRUCTURE_PROJECT%\Migrations" "!BACKUP_DIR!\" /E /I /Y >nul
        if !ERRORLEVEL! EQU 0 (
            echo [SUCCESS] Backup created!
            rmdir /s /q "%INFRASTRUCTURE_PROJECT%\Migrations"
            echo [INFO] Old migrations removed
        ) else (
            echo [ERROR] Backup failed!
            pause
            exit /b 1
        )
    )
)

REM T?o migration
echo ========================================
echo Creating Migration: %MIGRATION_NAME%
echo ========================================
echo.

dotnet ef migrations add "%MIGRATION_NAME%" --project %INFRASTRUCTURE_PROJECT% --startup-project %STARTUP_PROJECT% --verbose

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCESS] Migration created successfully!
    echo.
    
    REM H?i có mu?n update database không
    set /p UPDATE_DB="Update database now? (y/n): "
    if /i "!UPDATE_DB!"=="y" (
        echo [INFO] Updating database...
        dotnet ef database update --project %INFRASTRUCTURE_PROJECT% --startup-project %STARTUP_PROJECT% --verbose
        
        if !ERRORLEVEL! EQU 0 (
            echo [SUCCESS] Database updated successfully!
        ) else (
            echo [ERROR] Database update failed!
            echo [INFO] You can update manually later using:
            echo dotnet ef database update --project %INFRASTRUCTURE_PROJECT% --startup-project %STARTUP_PROJECT%
        )
    )
    
    echo.
    echo ========================================
    echo         MIGRATION COMPLETED!
    echo ========================================
    echo Migration files are located in:
    echo %INFRASTRUCTURE_PROJECT%\Migrations\
    echo.
    echo Next steps:
    echo 1. Review migration files
    echo 2. Test the application: dotnet run --project %STARTUP_PROJECT%
    echo 3. Check Swagger UI: https://localhost:7041/swagger
    echo.
    
) else (
    echo.
    echo [ERROR] Migration failed!
    echo.
    echo Common issues and solutions:
    echo.
    echo 1. Pending model changes:
    echo    - Try a different migration name
    echo    - Remove old migrations first
    echo.
    echo 2. Database connection error:
    echo    - Check connection string in appsettings.json
    echo    - Ensure SQL Server is running
    echo.
    echo 3. Foreign key constraint error:
    echo    - Run: fix-lexiflow-migration-errors.ps1 -FixForeignKeys -AutoFix
    echo.
    echo 4. Duplicate entity error:
    echo    - Check LexiFlowContext for duplicate DbSet definitions
    echo    - Run: fix-lexiflow-migration-errors.ps1 -FixDuplicates
    echo.
    
    pause
    exit /b 1
)

echo Press any key to exit...
pause >nul