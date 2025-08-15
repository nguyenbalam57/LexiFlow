# Reset v� t?o migration m?i cho LocalDB
# PowerShell script ?? setup LocalDB v� migration

Write-Host "?? B?t ??u reset v� t?o migration m?i cho LocalDB..." -ForegroundColor Green

# 1. Ki?m tra LocalDB c� s?n kh�ng
Write-Host "?? Ki?m tra LocalDB..." -ForegroundColor Yellow
try {
    $localDbInstances = sqllocaldb.exe info
    if ($localDbInstances -contains "mssqllocaldb") {
        Write-Host "? LocalDB ?� s?n s�ng" -ForegroundColor Green
    } else {
        Write-Host "?? T?o LocalDB instance..." -ForegroundColor Yellow
        sqllocaldb.exe create mssqllocaldb
        sqllocaldb.exe start mssqllocaldb
    }
} catch {
    Write-Host "? L?i khi ki?m tra LocalDB: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. X�a migrations c?
Write-Host "??? X�a migrations c?..." -ForegroundColor Yellow
$migrationsPath = "LexiFlow.Infrastructure\Migrations"
if (Test-Path $migrationsPath) {
    Remove-Item -Path $migrationsPath -Recurse -Force
    Write-Host "? ?� x�a migrations c?" -ForegroundColor Green
} else {
    Write-Host "?? Kh�ng c� migrations c? ?? x�a" -ForegroundColor Cyan
}

# 3. T?o migration m?i
Write-Host "?? T?o migration m?i..." -ForegroundColor Yellow
try {
    dotnet ef migrations add InitialLocalDB --project LexiFlow.Infrastructure --startup-project LexiFlow.API --verbose
    Write-Host "? ?� t?o migration InitialLocalDB" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi t?o migration: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 4. C?p nh?t database
Write-Host "?? C?p nh?t database..." -ForegroundColor Yellow
try {
    dotnet ef database update --project LexiFlow.Infrastructure --startup-project LexiFlow.API --verbose
    Write-Host "? ?� c?p nh?t database th�nh c�ng" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi c?p nh?t database: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "?? H�y ki?m tra connection string v� Entity Framework configuration" -ForegroundColor Yellow
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
    
    Write-Host "? K?t n?i database th�nh c�ng! C� $tableCount b?ng ???c t?o" -ForegroundColor Green
} catch {
    Write-Host "? L?i khi ki?m tra database: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "?? Ho�n th�nh setup LocalDB v� migration!" -ForegroundColor Green
Write-Host "?? B�y gi? b?n c� th? ch?y ?ng d?ng v?i: dotnet run --project LexiFlow.API" -ForegroundColor Cyan