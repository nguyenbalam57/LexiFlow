@echo off
echo ========================================
echo LexiFlow Migration Creator - .NET 9
echo ========================================

echo.
echo Checking EF Core Tools...
dotnet ef --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing EF Core Tools...
    dotnet tool install --global dotnet-ef
)

echo.
echo Choose migration type:
echo 1. Initial Create
echo 2. Performance Indexes  
echo 3. Custom Migration
echo 4. Update Database
echo 5. Show Migration History

set /p choice="Enter choice (1-5): "

cd LexiFlow.API

if "%choice%"=="1" (
    echo Creating Initial Migration...
    dotnet ef migrations add InitialCreate --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
    echo.
    set /p update="Update database now? (y/n): "
    if /i "%update%"=="y" (
        dotnet ef database update --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
    )
) else if "%choice%"=="2" (
    echo Creating Performance Indexes Migration...
    dotnet ef migrations add AddPerformanceIndexes --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
    echo.
    set /p update="Update database now? (y/n): "
    if /i "%update%"=="y" (
        dotnet ef database update --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
    )
) else if "%choice%"=="3" (
    set /p name="Enter migration name: "
    echo Creating Custom Migration: %name%
    dotnet ef migrations add %name% --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
    echo.
    set /p update="Update database now? (y/n): "
    if /i "%update%"=="y" (
        dotnet ef database update --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
    )
) else if "%choice%"=="4" (
    echo Updating Database...
    dotnet ef database update --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
) else if "%choice%"=="5" (
    echo Migration History:
    dotnet ef migrations list --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
)

cd ..
echo.
echo Migration script completed!
pause