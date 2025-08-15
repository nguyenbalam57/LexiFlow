# Script ?? chuy?n t? LocalDB sang SQL Server Express sau n�y
# PowerShell script ?? migration data

param(
    [string]$SourceConnectionString = "Server=(localdb)\mssqllocaldb;Database=LexiFlow;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
    [string]$TargetConnectionString = "Server=.\SQLEXPRESS;Database=LexiFlow;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
)

Write-Host "?? Migration t? LocalDB sang SQL Server Express..." -ForegroundColor Green

# 1. Backup database t? LocalDB
Write-Host "?? T?o backup t? LocalDB..." -ForegroundColor Yellow
$backupPath = "C:\temp\LexiFlow_LocalDB_Backup.bak"
$backupFolder = "C:\temp"

# T?o th? m?c backup n?u ch?a c�
if (!(Test-Path $backupFolder)) {
    New-Item -ItemType Directory -Path $backupFolder -Force
}

try {
    $backupSql = @"
BACKUP DATABASE [LexiFlow] 
TO DISK = '$backupPath'
WITH FORMAT, INIT, COMPRESSION;
"@
    
    sqlcmd -S "(localdb)\mssqllocaldb" -Q $backupSql -E
    Write-Host "? Backup ho�n th�nh: $backupPath" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi backup: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. Ki?m tra SQL Server Express
Write-Host "?? Ki?m tra SQL Server Express..." -ForegroundColor Yellow
try {
    sqlcmd -S ".\SQLEXPRESS" -Q "SELECT @@VERSION" -E
    Write-Host "? SQL Server Express c� s?n" -ForegroundColor Green
} catch {
    Write-Host "? SQL Server Express kh�ng c� s?n" -ForegroundColor Red
    Write-Host "?? Vui l�ng c�i ??t SQL Server Express tr??c khi ch?y script n�y" -ForegroundColor Yellow
    exit 1
}

# 3. Restore database l�n SQL Server Express
Write-Host "?? Restore database l�n SQL Server Express..." -ForegroundColor Yellow
try {
    $restoreSql = @"
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'LexiFlow')
BEGIN
    ALTER DATABASE [LexiFlow] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [LexiFlow];
END

RESTORE DATABASE [LexiFlow] 
FROM DISK = '$backupPath'
WITH REPLACE;
"@
    
    sqlcmd -S ".\SQLEXPRESS" -Q $restoreSql -E
    Write-Host "? Restore ho�n th�nh" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi restore: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 4. C?p nh?t connection string
Write-Host "?? C?p nh?t connection string..." -ForegroundColor Yellow
$appsettingsPath = "LexiFlow.API\appsettings.Development.json"
if (Test-Path $appsettingsPath) {
    $appsettings = Get-Content $appsettingsPath | ConvertFrom-Json
    $appsettings.ConnectionStrings.DefaultConnection = $TargetConnectionString
    $appsettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
    Write-Host "? ?� c?p nh?t connection string" -ForegroundColor Green
} else {
    Write-Host "?? Kh�ng t�m th?y appsettings.Development.json" -ForegroundColor Yellow
}

# 5. Test k?t n?i m?i
Write-Host "?? Test k?t n?i SQL Server Express..." -ForegroundColor Yellow
try {
    dotnet ef dbcontext info --project LexiFlow.Infrastructure --startup-project LexiFlow.API
    Write-Host "? K?t n?i SQL Server Express th�nh c�ng!" -ForegroundColor Green
} catch {
    Write-Host "? L?i k?t n?i: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "?? Migration ho�n th�nh!" -ForegroundColor Green
Write-Host "?? Database ?� ???c chuy?n t? LocalDB sang SQL Server Express" -ForegroundColor Cyan