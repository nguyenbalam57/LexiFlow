# Script ?? reset migrations hoàn toàn
# S? d?ng khi models ?ã thay ??i và không th? rollback bình th??ng

Write-Host "=== RESET MIGRATIONS SCRIPT ===" -ForegroundColor Yellow
Write-Host "Script này s? xóa toàn b? database và migrations hi?n t?i" -ForegroundColor Red
Write-Host "CH? S? D?NG TRONG DEVELOPMENT ENVIRONMENT!" -ForegroundColor Red

$confirmation = Read-Host "B?n có ch?c ch?n mu?n ti?p t?c? (y/N)"
if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
    Write-Host "H?y b? thao tác" -ForegroundColor Green
    exit
}

try {
    Write-Host "1. Xóa database hi?n t?i..." -ForegroundColor Cyan
    dotnet ef database drop --force -p LexiFlow.Infrastructure -s LexiFlow.API
    
    Write-Host "2. Xóa th? m?c Migrations..." -ForegroundColor Cyan
    if (Test-Path "LexiFlow.Infrastructure\Migrations") {
        Remove-Item -Recurse -Force "LexiFlow.Infrastructure\Migrations"
        Write-Host "   - ?ã xóa th? m?c Migrations" -ForegroundColor Green
    }
    
    Write-Host "3. T?o migration m?i..." -ForegroundColor Cyan
    dotnet ef migrations add InitialCreate -p LexiFlow.Infrastructure -s LexiFlow.API
    
    Write-Host "4. Áp d?ng migration..." -ForegroundColor Cyan
    dotnet ef database update -p LexiFlow.Infrastructure -s LexiFlow.API
    
    Write-Host "=== HOÀN THÀNH ===" -ForegroundColor Green
    Write-Host "Database và migrations ?ã ???c reset thành công!" -ForegroundColor Green
    
} catch {
    Write-Host "L?I: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Vui lòng ki?m tra và th? l?i" -ForegroundColor Yellow
}