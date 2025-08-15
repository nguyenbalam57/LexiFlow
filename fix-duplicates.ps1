```powershell
# Fix duplicate methods in LexiFlowContext
Write-Host "Fixing duplicate methods..." -ForegroundColor Yellow

$file = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs"
$content = Get-Content $file -Raw

# Remove old duplicate methods (lines ~551-1050)
$pattern1 = '(?s)private void ConfigureProgressEntities\(ModelBuilder modelBuilder\).*?}\s*(?=\s*/// <summary>\s*/// Configure exam and testing entities)'
$content = $content -replace $pattern1, ''

$pattern2 = '(?s)private void ConfigureExamEntities\(ModelBuilder modelBuilder\).*?}\s*(?=\s*/// <summary>\s*/// Configure planning and study entities)'  
$content = $content -replace $pattern2, ''

$pattern3 = '(?s)private void ConfigurePlanningEntities\(ModelBuilder modelBuilder\).*?}\s*(?=\s*/// <summary>\s*/// Override SaveChanges)'
$content = $content -replace $pattern3, ''

$content | Set-Content $file
Write-Host "Fixed!" -ForegroundColor Green
'@