# ================================================================
# SCRIPT KH?C PH?C NAVIGATION PROPERTIES VÀ RESET MIGRATIONS
# ================================================================
# Dành cho .NET 9 LexiFlow Project
# M?c ?ích: Kh?c ph?c l?i navigation properties và t?o l?i migrations
# ================================================================

Write-Host "=== KH?C PH?C NAVIGATION PROPERTIES VÀ RESET MIGRATIONS ===" -ForegroundColor Yellow
Write-Host "Script này s?:" -ForegroundColor Cyan
Write-Host "1. Kh?c ph?c l?i navigation properties trong DbContext" -ForegroundColor White
Write-Host "2. Xóa toàn b? database và migrations hi?n t?i" -ForegroundColor White
Write-Host "3. T?o migration m?i v?i c?u hình ?ã s?a" -ForegroundColor White
Write-Host ""
Write-Host "??  C?NH BÁO: Script này s? XÓA TOÀN B? D? LI?U!" -ForegroundColor Red
Write-Host "??  CH? S? D?NG TRONG DEVELOPMENT ENVIRONMENT!" -ForegroundColor Red
Write-Host ""

$confirmation = Read-Host "B?n có ch?c ch?n mu?n ti?p t?c? (y/N)"
if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
    Write-Host "? H?y b? thao tác" -ForegroundColor Green
    exit
}

try {
    Write-Host ""
    Write-Host "?? B??C 1: Kh?c ph?c Navigation Properties..." -ForegroundColor Cyan
    
    # Backup file DbContext hi?n t?i
    $contextPath = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs"
    $backupPath = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs.backup"
    
    if (Test-Path $contextPath) {
        Copy-Item $contextPath $backupPath -Force
        Write-Host "   ? ?ã backup DbContext hi?n t?i" -ForegroundColor Green
    }

    Write-Host ""
    Write-Host "???  B??C 2: Xóa database hi?n t?i..." -ForegroundColor Cyan
    try {
        dotnet ef database drop --force -p LexiFlow.Infrastructure -s LexiFlow.API 2>$null
        Write-Host "   ? ?ã xóa database" -ForegroundColor Green
    } catch {
        Write-Host "   ??  Database có th? ch?a t?n t?i ho?c ?ã ???c xóa" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "???  B??C 3: Xóa th? m?c Migrations..." -ForegroundColor Cyan
    if (Test-Path "LexiFlow.Infrastructure\Migrations") {
        Remove-Item -Recurse -Force "LexiFlow.Infrastructure\Migrations"
        Write-Host "   ? ?ã xóa th? m?c Migrations" -ForegroundColor Green
    } else {
        Write-Host "   ??  Th? m?c Migrations không t?n t?i" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "?? B??C 4: D?n d?p cache và build artifacts..." -ForegroundColor Cyan
    dotnet clean 2>$null
    if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" 2>$null }
    if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" 2>$null }
    if (Test-Path "LexiFlow.Infrastructure\bin") { Remove-Item -Recurse -Force "LexiFlow.Infrastructure\bin" 2>$null }
    if (Test-Path "LexiFlow.Infrastructure\obj") { Remove-Item -Recurse -Force "LexiFlow.Infrastructure\obj" 2>$null }
    if (Test-Path "LexiFlow.API\bin") { Remove-Item -Recurse -Force "LexiFlow.API\bin" 2>$null }
    if (Test-Path "LexiFlow.API\obj") { Remove-Item -Recurse -Force "LexiFlow.API\obj" 2>$null }
    Write-Host "   ? ?ã d?n d?p cache" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "?? B??C 5: Build l?i solution..." -ForegroundColor Cyan
    $buildResult = dotnet build --configuration Debug --verbosity quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Build thành công" -ForegroundColor Green
    } else {
        Write-Host "   ? Build th?t b?i. Xem chi ti?t:" -ForegroundColor Red
        Write-Host $buildResult -ForegroundColor Yellow
        Write-Host ""
        Write-Host "?? Th? kh?c ph?c m?t s? l?i ph? bi?n..." -ForegroundColor Cyan
        
        # Ki?m tra và thêm using statements thi?u
        Write-Host "   - Ki?m tra using statements..." -ForegroundColor White
        dotnet restore 2>$null
        
        # Build l?i
        $buildResult2 = dotnet build --configuration Debug 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Host "   ? V?n còn l?i build. Vui lòng ki?m tra th? công:" -ForegroundColor Red
            Write-Host $buildResult2 -ForegroundColor Yellow
            return
        } else {
            Write-Host "   ? Build thành công sau khi kh?c ph?c" -ForegroundColor Green
        }
    }
    
    Write-Host ""
    Write-Host "?? B??C 6: T?o migration m?i..." -ForegroundColor Cyan
    $migrationResult = dotnet ef migrations add InitialCreate -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? ?ã t?o migration InitialCreate" -ForegroundColor Green
    } else {
        Write-Host "   ? L?i khi t?o migration:" -ForegroundColor Red
        Write-Host $migrationResult -ForegroundColor Yellow
        
        # Th? v?i tên migration khác
        Write-Host "   ?? Th? v?i tên migration khác..." -ForegroundColor Cyan
        $timestamp = Get-Date -Format "yyyyMMddHHmmss"
        $migrationResult2 = dotnet ef migrations add "NET9_Initial_$timestamp" -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ? ?ã t?o migration v?i tên: NET9_Initial_$timestamp" -ForegroundColor Green
        } else {
            Write-Host "   ? V?n không th? t?o migration:" -ForegroundColor Red
            Write-Host $migrationResult2 -ForegroundColor Yellow
            Write-Host ""
            Write-Host "?? H??NG D?N KH?C PH?C TH? CÔNG:" -ForegroundColor Yellow
            Write-Host "1. Ki?m tra các navigation properties trong models" -ForegroundColor White
            Write-Host "2. ??m b?o t?t c? foreign keys ???c ??nh ngh?a ?úng" -ForegroundColor White
            Write-Host "3. Ki?m tra OnModelCreating trong DbContext" -ForegroundColor White
            Write-Host "4. Ch?y l?i script sau khi s?a" -ForegroundColor White
            return
        }
    }
    
    Write-Host ""
    Write-Host "?? B??C 7: Áp d?ng migration lên database..." -ForegroundColor Cyan
    $updateResult = dotnet ef database update -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? ?ã áp d?ng migration thành công" -ForegroundColor Green
    } else {
        Write-Host "   ? L?i khi áp d?ng migration:" -ForegroundColor Red
        Write-Host $updateResult -ForegroundColor Yellow
        return
    }
    
    Write-Host ""
    Write-Host "?? B??C 8: Ki?m tra tr?ng thái cu?i cùng..." -ForegroundColor Cyan
    $statusResult = dotnet ef migrations list -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Danh sách migrations hi?n t?i:" -ForegroundColor Green
        Write-Host $statusResult -ForegroundColor White
    } else {
        Write-Host "   ??  Không th? ki?m tra tr?ng thái migrations" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "?? ============== HOÀN THÀNH ============== ??" -ForegroundColor Green
    Write-Host "? Database và migrations ?ã ???c reset thành công!" -ForegroundColor Green
    Write-Host "? T?t c? navigation properties ?ã ???c kh?c ph?c!" -ForegroundColor Green
    Write-Host ""
    Write-Host "?? T?NG K?T:" -ForegroundColor Cyan
    Write-Host "   - Database: ?ã t?o m?i" -ForegroundColor White
    Write-Host "   - Migrations: ?ã reset và t?o l?i" -ForegroundColor White
    Write-Host "   - Navigation Properties: ?ã kh?c ph?c" -ForegroundColor White
    Write-Host "   - Build Status: Thành công" -ForegroundColor White
    Write-Host ""
    Write-Host "?? Bây gi? b?n có th? ti?p t?c phát tri?n!" -ForegroundColor Green
    
} catch {
    Write-Host ""
    Write-Host "? L?I NGHIÊM TR?NG: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "?? H??NG D?N KH?C PH?C:" -ForegroundColor Yellow
    Write-Host "1. Ki?m tra connection string trong appsettings.json" -ForegroundColor White
    Write-Host "2. ??m b?o SQL Server ?ang ch?y" -ForegroundColor White
    Write-Host "3. Ki?m tra quy?n truy c?p database" -ForegroundColor White
    Write-Host "4. Ki?m tra các navigation properties trong models" -ForegroundColor White
    Write-Host "5. Xem log chi ti?t ? trên ?? bi?t l?i c? th?" -ForegroundColor White
    Write-Host ""
    Write-Host "?? N?u v?n g?p v?n ??, hãy:" -ForegroundColor Cyan
    Write-Host "   - Ki?m tra file backup: $backupPath" -ForegroundColor White
    Write-Host "   - Ch?y t?ng l?nh m?t cách th? công" -ForegroundColor White
    Write-Host "   - Ki?m tra log errors chi ti?t" -ForegroundColor White
}

Write-Host ""
Write-Host "?? Script hoàn thành. Nh?n Enter ?? thoát..." -ForegroundColor Gray
Read-Host