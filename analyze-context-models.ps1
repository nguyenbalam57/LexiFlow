# Phân tích t??ng ??ng LexiFlowContext v?i Models
Write-Host "?? ?ang phân tích t??ng ??ng gi?a LexiFlowContext và Models..." -ForegroundColor Cyan

# Ki?m tra DbSets trong LexiFlowContext
Write-Host "`n?? DbSets trong LexiFlowContext:" -ForegroundColor Yellow

$contextFile = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs"
if (Test-Path $contextFile) {
    $contextContent = Get-Content $contextFile -Raw
    
    # Trích xu?t DbSets
    $dbSetPattern = 'public DbSet<(\w+)> (\w+) \{ get; set; \}'
    $matches = [regex]::Matches($contextContent, $dbSetPattern)
    
    Write-Host "? Tìm th?y $($matches.Count) DbSets:" -ForegroundColor Green
    
    $dbSets = @{}
    foreach ($match in $matches) {
        $modelType = $match.Groups[1].Value
        $dbSetName = $match.Groups[2].Value
        $dbSets[$modelType] = $dbSetName
        Write-Host "   - $dbSetName : $modelType" -ForegroundColor White
    }
}

# Ki?m tra Models th?c t? trong workspace
Write-Host "`n?? Models th?c t? trong workspace:" -ForegroundColor Yellow

$modelFiles = Get-ChildItem -Path "LexiFlow.Models" -Recurse -Filter "*.cs" | Where-Object { 
    $_.Name -notmatch "(Interface|Dto|Request|Response|Specification)" 
}

$actualModels = @()
foreach ($file in $modelFiles) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match 'public class (\w+)') {
        $className = $matches[1]
        $actualModels += $className
        Write-Host "   ? $className (in $($file.Name))" -ForegroundColor Green
    }
}

Write-Host "`n?? Th?ng kê:" -ForegroundColor Cyan
Write-Host "   - DbSets trong Context: $($dbSets.Count)" -ForegroundColor White
Write-Host "   - Models th?c t?: $($actualModels.Count)" -ForegroundColor White

# Ki?m tra models có trong DbSet nh?ng không t?n t?i
Write-Host "`n? Models có trong DbSet nh?ng không t?n t?i:" -ForegroundColor Red
$missingModels = @()
foreach ($modelType in $dbSets.Keys) {
    if ($actualModels -notcontains $modelType) {
        $missingModels += $modelType
        Write-Host "   - $modelType (DbSet: $($dbSets[$modelType]))" -ForegroundColor Red
    }
}

# Ki?m tra models t?n t?i nh?ng không có DbSet
Write-Host "`n??  Models t?n t?i nh?ng không có DbSet:" -ForegroundColor Yellow
$extraModels = @()
foreach ($model in $actualModels) {
    if ($dbSets.Keys -notcontains $model) {
        $extraModels += $model
        Write-Host "   - $model" -ForegroundColor Yellow
    }
}

# Tóm t?t
Write-Host "`n?? Tóm t?t phân tích:" -ForegroundColor Cyan
if ($missingModels.Count -eq 0 -and $extraModels.Count -eq 0) {
    Write-Host "   ? LexiFlowContext hoàn toàn kh?p v?i Models" -ForegroundColor Green
} else {
    Write-Host "   ? Có $($missingModels.Count) models thi?u và $($extraModels.Count) models th?a" -ForegroundColor Red
    Write-Host "   ?? C?n c?p nh?t LexiFlowContext ?? ??ng b? v?i Models" -ForegroundColor Yellow
}

Write-Host "`n?? Phân tích hoàn t?t!" -ForegroundColor Green