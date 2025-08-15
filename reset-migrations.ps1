# Script ?? reset migrations ho�n to�n
# S? d?ng khi models ?� thay ??i v� kh�ng th? rollback b�nh th??ng

Write-Host "=== RESET MIGRATIONS SCRIPT ===" -ForegroundColor Yellow
Write-Host "Script n�y s? x�a to�n b? database v� migrations hi?n t?i" -ForegroundColor Red
Write-Host "CH? S? D?NG TRONG DEVELOPMENT ENVIRONMENT!" -ForegroundColor Red

$confirmation = Read-Host "B?n c� ch?c ch?n mu?n ti?p t?c? (y/N)"
if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
    Write-Host "H?y b? thao t�c" -ForegroundColor Green
    exit
}

try {
    Write-Host "1. X�a database hi?n t?i..." -ForegroundColor Cyan
    dotnet ef database drop --force -p LexiFlow.Infrastructure -s LexiFlow.API
    
    Write-Host "2. X�a th? m?c Migrations..." -ForegroundColor Cyan
    if (Test-Path "LexiFlow.Infrastructure\Migrations") {
        Remove-Item -Recurse -Force "LexiFlow.Infrastructure\Migrations"
        Write-Host "   - ?� x�a th? m?c Migrations" -ForegroundColor Green
    }
    
    Write-Host "3. T?o migration m?i..." -ForegroundColor Cyan
    dotnet ef migrations add InitialCreate -p LexiFlow.Infrastructure -s LexiFlow.API
    
    Write-Host "4. �p d?ng migration..." -ForegroundColor Cyan
    dotnet ef database update -p LexiFlow.Infrastructure -s LexiFlow.API
    
    Write-Host "=== HO�N TH�NH ===" -ForegroundColor Green
    Write-Host "Database v� migrations ?� ???c reset th�nh c�ng!" -ForegroundColor Green
    
} catch {
    Write-Host "L?I: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Vui l�ng ki?m tra v� th? l?i" -ForegroundColor Yellow
}