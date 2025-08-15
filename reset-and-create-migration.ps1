# Reset và t?o migration m?i cho LocalDB
# PowerShell script ?? setup LocalDB và migration

Write-Host "?? B?t ??u reset và t?o migration m?i cho LocalDB..." -ForegroundColor Green

# 1. Ki?m tra LocalDB có s?n không
Write-Host "?? Ki?m tra LocalDB..." -ForegroundColor Yellow
try {
    $localDbInstances = sqllocaldb.exe info
    if ($localDbInstances -contains "mssqllocaldb") {
        Write-Host "? LocalDB ?ã s?n sàng" -ForegroundColor Green
    } else {
        Write-Host "?? T?o LocalDB instance..." -ForegroundColor Yellow
        sqllocaldb.exe create mssqllocaldb
        sqllocaldb.exe start mssqllocaldb
    }
} catch {
    Write-Host "? L?i khi ki?m tra LocalDB: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. Xóa migrations c?
Write-Host "??? Xóa migrations c?..." -ForegroundColor Yellow
$migrationsPath = "LexiFlow.Infrastructure\Migrations"
if (Test-Path $migrationsPath) {
    Remove-Item -Path $migrationsPath -Recurse -Force
    Write-Host "? ?ã xóa migrations c?" -ForegroundColor Green
} else {
    Write-Host "?? Không có migrations c? ?? xóa" -ForegroundColor Cyan
}

# 3. T?o migration m?i
Write-Host "?? T?o migration m?i..." -ForegroundColor Yellow
try {
    dotnet ef migrations add InitialLocalDB --project LexiFlow.Infrastructure --startup-project LexiFlow.API --verbose
    Write-Host "? ?ã t?o migration InitialLocalDB" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi t?o migration: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 4. C?p nh?t database
Write-Host "?? C?p nh?t database..." -ForegroundColor Yellow
try {
    dotnet ef database update --project LexiFlow.Infrastructure --startup-project LexiFlow.API --verbose
    Write-Host "? ?ã c?p nh?t database thành công" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi c?p nh?t database: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "?? Hãy ki?m tra connection string và Entity Framework configuration" -ForegroundColor Yellow
    exit 1
}

# 5. Ki?m tra k?t n?i database
Write-Host "?? Ki?m tra k?t n?i database..." -ForegroundColor Yellow
try {
    $connectionString = "Server=(localdb)\mssqllocaldb;Database=LexiFlow;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"
    $tableCount = $command.ExecuteScalar()
    $connection.Close()
    
    Write-Host "? K?t n?i database thành công! Có $tableCount b?ng ???c t?o" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi ki?m tra database: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "?? Hoàn thành setup LocalDB và migration!" -ForegroundColor Green
Write-Host "?? Bây gi? b?n có th? ch?y ?ng d?ng v?i: dotnet run --project LexiFlow.API" -ForegroundColor Cyan