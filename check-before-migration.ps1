# Ki?m tra v� fix l?i build tr??c khi migration
# PowerShell script ?? ki?m tra build errors

Write-Host "?? Ki?m tra l?i build v� Entity Framework..." -ForegroundColor Green

# 1. Build project ?? ki?m tra l?i
Write-Host "??? Build solution..." -ForegroundColor Yellow
try {
    dotnet build --no-restore --verbosity normal
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Build th�nh c�ng" -ForegroundColor Green
    } else {
        Write-Host "? Build th?t b?i, ki?m tra l?i tr�n" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "? L?i khi build: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. Ki?m tra Entity Framework tools
Write-Host "??? Ki?m tra EF Core tools..." -ForegroundColor Yellow
try {
    dotnet ef --version
    Write-Host "? EF Core tools c� s?n" -ForegroundColor Green
} catch {
    Write-Host "?? C�i ??t EF Core tools..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
}

# 3. Ki?m tra DbContext
Write-Host "?? Ki?m tra DbContext configuration..." -ForegroundColor Yellow
try {
    dotnet ef dbcontext info --project LexiFlow.Infrastructure --startup-project LexiFlow.API
    Write-Host "? DbContext configuration OK" -ForegroundColor Green
} catch {
    Write-Host "? L?i DbContext configuration: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "?? Ki?m tra LexiFlowContext v� connection string" -ForegroundColor Yellow
    exit 1
}

Write-Host "?? T?t c? ki?m tra ?� pass! C� th? ti?p t?c v?i migration" -ForegroundColor Green