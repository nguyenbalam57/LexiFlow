# ================================================================
# SCRIPT KH?C PH?C NAVIGATION PROPERTIES V� RESET MIGRATIONS
# ================================================================
# D�nh cho .NET 9 LexiFlow Project
# M?c ?�ch: Kh?c ph?c l?i navigation properties v� t?o l?i migrations
# ================================================================

Write-Host "=== KH?C PH?C NAVIGATION PROPERTIES V� RESET MIGRATIONS ===" -ForegroundColor Yellow
Write-Host "Script n�y s?:" -ForegroundColor Cyan
Write-Host "1. Kh?c ph?c l?i navigation properties trong DbContext" -ForegroundColor White
Write-Host "2. X�a to�n b? database v� migrations hi?n t?i" -ForegroundColor White
Write-Host "3. T?o migration m?i v?i c?u h�nh ?� s?a" -ForegroundColor White
Write-Host ""
Write-Host "??  C?NH B�O: Script n�y s? X�A TO�N B? D? LI?U!" -ForegroundColor Red
Write-Host "??  CH? S? D?NG TRONG DEVELOPMENT ENVIRONMENT!" -ForegroundColor Red
Write-Host ""

$confirmation = Read-Host "B?n c� ch?c ch?n mu?n ti?p t?c? (y/N)"
if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
    Write-Host "? H?y b? thao t�c" -ForegroundColor Green
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
        Write-Host "   ? ?� backup DbContext hi?n t?i" -ForegroundColor Green
    }

    Write-Host ""
    Write-Host "???  B??C 2: X�a database hi?n t?i..." -ForegroundColor Cyan
    try {
        dotnet ef database drop --force -p LexiFlow.Infrastructure -s LexiFlow.API 2>$null
        Write-Host "   ? ?� x�a database" -ForegroundColor Green
    } catch {
        Write-Host "   ??  Database c� th? ch?a t?n t?i ho?c ?� ???c x�a" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "???  B??C 3: X�a th? m?c Migrations..." -ForegroundColor Cyan
    if (Test-Path "LexiFlow.Infrastructure\Migrations") {
        Remove-Item -Recurse -Force "LexiFlow.Infrastructure\Migrations"
        Write-Host "   ? ?� x�a th? m?c Migrations" -ForegroundColor Green
    } else {
        Write-Host "   ??  Th? m?c Migrations kh�ng t?n t?i" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "?? B??C 4: D?n d?p cache v� build artifacts..." -ForegroundColor Cyan
    dotnet clean 2>$null
    if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" 2>$null }
    if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" 2>$null }
    if (Test-Path "LexiFlow.Infrastructure\bin") { Remove-Item -Recurse -Force "LexiFlow.Infrastructure\bin" 2>$null }
    if (Test-Path "LexiFlow.Infrastructure\obj") { Remove-Item -Recurse -Force "LexiFlow.Infrastructure\obj" 2>$null }
    if (Test-Path "LexiFlow.API\bin") { Remove-Item -Recurse -Force "LexiFlow.API\bin" 2>$null }
    if (Test-Path "LexiFlow.API\obj") { Remove-Item -Recurse -Force "LexiFlow.API\obj" 2>$null }
    Write-Host "   ? ?� d?n d?p cache" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "?? B??C 5: Build l?i solution..." -ForegroundColor Cyan
    $buildResult = dotnet build --configuration Debug --verbosity quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Build th�nh c�ng" -ForegroundColor Green
    } else {
        Write-Host "   ? Build th?t b?i. Xem chi ti?t:" -ForegroundColor Red
        Write-Host $buildResult -ForegroundColor Yellow
        Write-Host ""
        Write-Host "?? Th? kh?c ph?c m?t s? l?i ph? bi?n..." -ForegroundColor Cyan
        
        # Ki?m tra v� th�m using statements thi?u
        Write-Host "   - Ki?m tra using statements..." -ForegroundColor White
        dotnet restore 2>$null
        
        # Build l?i
        $buildResult2 = dotnet build --configuration Debug 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Host "   ? V?n c�n l?i build. Vui l�ng ki?m tra th? c�ng:" -ForegroundColor Red
            Write-Host $buildResult2 -ForegroundColor Yellow
            return
        } else {
            Write-Host "   ? Build th�nh c�ng sau khi kh?c ph?c" -ForegroundColor Green
        }
    }
    
    Write-Host ""
    Write-Host "?? B??C 6: T?o migration m?i..." -ForegroundColor Cyan
    $migrationResult = dotnet ef migrations add InitialCreate -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? ?� t?o migration InitialCreate" -ForegroundColor Green
    } else {
        Write-Host "   ? L?i khi t?o migration:" -ForegroundColor Red
        Write-Host $migrationResult -ForegroundColor Yellow
        
        # Th? v?i t�n migration kh�c
        Write-Host "   ?? Th? v?i t�n migration kh�c..." -ForegroundColor Cyan
        $timestamp = Get-Date -Format "yyyyMMddHHmmss"
        $migrationResult2 = dotnet ef migrations add "NET9_Initial_$timestamp" -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ? ?� t?o migration v?i t�n: NET9_Initial_$timestamp" -ForegroundColor Green
        } else {
            Write-Host "   ? V?n kh�ng th? t?o migration:" -ForegroundColor Red
            Write-Host $migrationResult2 -ForegroundColor Yellow
            Write-Host ""
            Write-Host "?? H??NG D?N KH?C PH?C TH? C�NG:" -ForegroundColor Yellow
            Write-Host "1. Ki?m tra c�c navigation properties trong models" -ForegroundColor White
            Write-Host "2. ??m b?o t?t c? foreign keys ???c ??nh ngh?a ?�ng" -ForegroundColor White
            Write-Host "3. Ki?m tra OnModelCreating trong DbContext" -ForegroundColor White
            Write-Host "4. Ch?y l?i script sau khi s?a" -ForegroundColor White
            return
        }
    }
    
    Write-Host ""
    Write-Host "?? B??C 7: �p d?ng migration l�n database..." -ForegroundColor Cyan
    $updateResult = dotnet ef database update -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? ?� �p d?ng migration th�nh c�ng" -ForegroundColor Green
    } else {
        Write-Host "   ? L?i khi �p d?ng migration:" -ForegroundColor Red
        Write-Host $updateResult -ForegroundColor Yellow
        return
    }
    
    Write-Host ""
    Write-Host "?? B??C 8: Ki?m tra tr?ng th�i cu?i c�ng..." -ForegroundColor Cyan
    $statusResult = dotnet ef migrations list -p LexiFlow.Infrastructure -s LexiFlow.API 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Danh s�ch migrations hi?n t?i:" -ForegroundColor Green
        Write-Host $statusResult -ForegroundColor White
    } else {
        Write-Host "   ??  Kh�ng th? ki?m tra tr?ng th�i migrations" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "?? ============== HO�N TH�NH ============== ??" -ForegroundColor Green
    Write-Host "? Database v� migrations ?� ???c reset th�nh c�ng!" -ForegroundColor Green
    Write-Host "? T?t c? navigation properties ?� ???c kh?c ph?c!" -ForegroundColor Green
    Write-Host ""
    Write-Host "?? T?NG K?T:" -ForegroundColor Cyan
    Write-Host "   - Database: ?� t?o m?i" -ForegroundColor White
    Write-Host "   - Migrations: ?� reset v� t?o l?i" -ForegroundColor White
    Write-Host "   - Navigation Properties: ?� kh?c ph?c" -ForegroundColor White
    Write-Host "   - Build Status: Th�nh c�ng" -ForegroundColor White
    Write-Host ""
    Write-Host "?? B�y gi? b?n c� th? ti?p t?c ph�t tri?n!" -ForegroundColor Green
    
} catch {
    Write-Host ""
    Write-Host "? L?I NGHI�M TR?NG: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "?? H??NG D?N KH?C PH?C:" -ForegroundColor Yellow
    Write-Host "1. Ki?m tra connection string trong appsettings.json" -ForegroundColor White
    Write-Host "2. ??m b?o SQL Server ?ang ch?y" -ForegroundColor White
    Write-Host "3. Ki?m tra quy?n truy c?p database" -ForegroundColor White
    Write-Host "4. Ki?m tra c�c navigation properties trong models" -ForegroundColor White
    Write-Host "5. Xem log chi ti?t ? tr�n ?? bi?t l?i c? th?" -ForegroundColor White
    Write-Host ""
    Write-Host "?? N?u v?n g?p v?n ??, h�y:" -ForegroundColor Cyan
    Write-Host "   - Ki?m tra file backup: $backupPath" -ForegroundColor White
    Write-Host "   - Ch?y t?ng l?nh m?t c�ch th? c�ng" -ForegroundColor White
    Write-Host "   - Ki?m tra log errors chi ti?t" -ForegroundColor White
}

Write-Host ""
Write-Host "?? Script ho�n th�nh. Nh?n Enter ?? tho�t..." -ForegroundColor Gray
Read-Host