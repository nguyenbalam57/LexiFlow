# Script ?? chuy?n t? LocalDB sang SQL Server Express sau này
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

# T?o th? m?c backup n?u ch?a có
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
    Write-Host "? Backup hoàn thành: $backupPath" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi backup: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. Ki?m tra SQL Server Express
Write-Host "?? Ki?m tra SQL Server Express..." -ForegroundColor Yellow
try {
    sqlcmd -S ".\SQLEXPRESS" -Q "SELECT @@VERSION" -E
    Write-Host "? SQL Server Express có s?n" -ForegroundColor Green
} catch {
    Write-Host "? SQL Server Express không có s?n" -ForegroundColor Red
    Write-Host "?? Vui lòng cài ??t SQL Server Express tr??c khi ch?y script này" -ForegroundColor Yellow
    exit 1
}

# 3. Restore database lên SQL Server Express
Write-Host "?? Restore database lên SQL Server Express..." -ForegroundColor Yellow
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
    Write-Host "? Restore hoàn thành" -ForegroundColor Green
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
    Write-Host "? ?ã c?p nh?t connection string" -ForegroundColor Green
} else {
    Write-Host "?? Không tìm th?y appsettings.Development.json" -ForegroundColor Yellow
}

# 5. Test k?t n?i m?i
Write-Host "?? Test k?t n?i SQL Server Express..." -ForegroundColor Yellow
try {
    dotnet ef dbcontext info --project LexiFlow.Infrastructure --startup-project LexiFlow.API
    Write-Host "? K?t n?i SQL Server Express thành công!" -ForegroundColor Green
} catch {
    Write-Host "? L?i k?t n?i: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "?? Migration hoàn thành!" -ForegroundColor Green
Write-Host "?? Database ?ã ???c chuy?n t? LocalDB sang SQL Server Express" -ForegroundColor Cyan