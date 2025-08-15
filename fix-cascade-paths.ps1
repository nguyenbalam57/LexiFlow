# Fix all cascade paths issues in LexiFlowContext.cs
# PowerShell script ?? fix t?t c? các l?i cascade paths

Write-Host "?? Fix t?t c? cascade paths issues..." -ForegroundColor Green

# ??c file LexiFlowContext.cs
$contextFile = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs"
$content = Get-Content $contextFile -Raw

# List of patterns to fix cascade paths
$patterns = @{
    # Fix all User relationships to avoid cascade conflicts
    'OnDelete\(DeleteBehavior\.SetNull\)' = 'OnDelete(DeleteBehavior.NoAction)'
    
    # Keep only essential cascades
    'entity\.HasOne\(([^)]+)\)\s+\.WithMany\(\)\s+\.HasForeignKey\(([^)]+)\)\s+\.OnDelete\(DeleteBehavior\.Cascade\)' = 'entity.HasOne($1).WithMany().HasForeignKey($2).OnDelete(DeleteBehavior.NoAction)'
}

# Apply pattern replacements
foreach ($pattern in $patterns.Keys) {
    $replacement = $patterns[$pattern]
    $content = $content -replace $pattern, $replacement
}

# Write back to file
$content | Set-Content $contextFile -Encoding UTF8

Write-Host "? ?ã fix cascade paths issues" -ForegroundColor Green

# Rebuild project
Write-Host "??? Rebuild project..." -ForegroundColor Yellow
dotnet build LexiFlow.Infrastructure --verbosity minimal

if ($LASTEXITCODE -eq 0) {
    Write-Host "? Build thành công" -ForegroundColor Green
} else {
    Write-Host "? Build th?t b?i" -ForegroundColor Red
    exit 1
}

Write-Host "?? Hoàn thành fix cascade paths!" -ForegroundColor Green