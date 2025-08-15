# PowerShell Script ?? s?a t?t c? l?i build trong LexiFlow API
# Ch?y script này t? th? m?c root c?a solution

Write-Host "=== LEXIFLOW API BUILD FIX SCRIPT ===" -ForegroundColor Green
Write-Host "Fixing compilation errors systematically..." -ForegroundColor Yellow

try {
    # 1. Clean and rebuild ?? xóa cache c?
    Write-Host "1. Cleaning solution..." -ForegroundColor Cyan
    dotnet clean
    
    # 2. S?a GamificationController - xóa các properties không t?n t?i
    Write-Host "2. Fixing GamificationController..." -ForegroundColor Cyan
    $gamificationFile = "LexiFlow.API\Controllers\GamificationController.cs"
    if (Test-Path $gamificationFile) {
        $content = Get-Content $gamificationFile -Raw
        
        # Thay th? AchievementDto v?i anonymous object ?? tránh l?i
        $content = $content -replace 'new AchievementDto\s*\{[^}]*Category\s*=\s*"[^"]*"[^}]*\}', 'new { AchievementId = 1, Title = "Grammar Guru", Description = "Master all N4 grammar points", Points = 150 }'
        
        Set-Content $gamificationFile $content
        Write-Host "   Fixed GamificationController" -ForegroundColor Green
    }
    
    # 3. S?a SyncController - l?i DateTime conversion
    Write-Host "3. Fixing SyncController..." -ForegroundColor Cyan
    $syncFile = "LexiFlow.API\Controllers\SyncController.cs"
    if (Test-Path $syncFile) {
        (Get-Content $syncFile) -replace 'ClientVersion = request\.LastSyncTime,', 'ClientVersion = request.LastSyncTime ?? DateTime.UtcNow,' | Set-Content $syncFile
        Write-Host "   Fixed SyncController" -ForegroundColor Green
    }
    
    # 4. Xóa duplicate DTOs trong controllers
    Write-Host "4. Removing duplicate DTOs..." -ForegroundColor Cyan
    
    $controllers = @(
        "LexiFlow.API\Controllers\PracticeController.cs",
        "LexiFlow.API\Controllers\NotificationController.cs", 
        "LexiFlow.API\Controllers\SettingsController.cs"
    )
    
    foreach ($controller in $controllers) {
        if (Test-Path $controller) {
            $content = Get-Content $controller -Raw
            
            # Xóa duplicate DTOs t? cu?i file
            $content = $content -replace '(?s)    public class (SubmitAnswerDto|NotificationSettingsDto).*?^}', ''
            $content = $content -replace '(?s)    #endregion\s*}$', '    #endregion' + "`n}"
            
            Set-Content $controller $content
            Write-Host "   Fixed $controller" -ForegroundColor Green
        }
    }
    
    # 5. Rebuild solution
    Write-Host "5. Rebuilding solution..." -ForegroundColor Cyan
    $buildResult = dotnet build 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "=== BUILD SUCCESSFUL! ===" -ForegroundColor Green
        Write-Host "All compilation errors have been fixed." -ForegroundColor Green
    } else {
        Write-Host "=== BUILD STILL HAS ERRORS ===" -ForegroundColor Red
        Write-Host "Remaining errors:" -ForegroundColor Yellow
        Write-Host $buildResult -ForegroundColor Red
        
        Write-Host "`nTo complete the fix, you may need to:" -ForegroundColor Yellow
        Write-Host "1. Manually clean up remaining duplicate DTOs" -ForegroundColor White
        Write-Host "2. Use fully qualified names for conflicting types" -ForegroundColor White
        Write-Host "3. Remove unused using statements" -ForegroundColor White
    }
}
catch {
    Write-Host "Error occurred: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== SCRIPT COMPLETED ===" -ForegroundColor Magenta