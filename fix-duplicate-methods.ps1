```powershell
# Script ?? s?a l?i duplicate methods trong LexiFlowContext

Write-Host "B?t ??u s?a l?i duplicate methods trong LexiFlowContext..." -ForegroundColor Yellow

$contextFile = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs"

if (-not (Test-Path $contextFile)) {
    Write-Host "Không tìm th?y file $contextFile" -ForegroundColor Red
    exit 1
}

Write-Host "?ang s?a file $contextFile..." -ForegroundColor Green

# ??c n?i dung file
$content = Get-Content $contextFile -Raw

# 1. S?a OnModelCreating ?? lo?i b? duplicate ConfigureQueryFilters và g?i các ph??ng th?c Fixed
$onModelCreatingPattern = '(?s)protected override void OnModelCreating\(ModelBuilder modelBuilder\).*?ConfigureQueryFilters\(modelBuilder\);\s*}'
$newOnModelCreating = @'
protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure core behavior
            ConfigureCoreEntities(modelBuilder);

            // Configure entity mappings
            ConfigureUserEntities(modelBuilder);
            ConfigureVocabularyEntities(modelBuilder);
            ConfigureGrammarEntities(modelBuilder);
            ConfigureTechnicalTermEntities(modelBuilder);
            ConfigureMediaEntities(modelBuilder);
            ConfigureSchedulingEntities(modelBuilder);
            ConfigureNotificationEntities(modelBuilder);
            ConfigureSystemEntities(modelBuilder);
            
            // Configure Priority 1 entities - C?N THI?T (Fixed versions)
            ConfigureProgressEntitiesFixed(modelBuilder);
            ConfigureExamEntitiesFixed(modelBuilder);
            ConfigurePlanningEntitiesFixed(modelBuilder);
            
            ConfigureQueryFilters(modelBuilder);
        }
'@

$content = $content -replace $onModelCreatingPattern, $newOnModelCreating

# 2. Xóa các ph??ng th?c duplicate c?
$duplicatePattern1 = '(?s)/// <summary>\s*/// Configure progress tracking entities.*?}\s*(?=\s*/// <summary>\s*/// Configure exam and testing entities)'
$content = $content -replace $duplicatePattern1, ''

$duplicatePattern2 = '(?s)/// <summary>\s*/// Configure exam and testing entities.*?}\s*(?=\s*/// <summary>\s*/// Configure planning and study entities)'
$content = $content -replace $duplicatePattern2, ''

$duplicatePattern3 = '(?s)/// <summary>\s*/// Configure planning and study entities.*?}\s*(?=\s*/// <summary>\s*/// Override SaveChanges)'
$content = $content -replace $duplicatePattern3, ''

# 3. Ghi l?i file
$content | Set-Content $contextFile -Encoding UTF8

Write-Host "?ã s?a xong file $contextFile" -ForegroundColor Green

# 4. Ki?m tra build
Write-Host "?ang ki?m tra build..." -ForegroundColor Yellow

try {
    dotnet build --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Build thành công! ?" -ForegroundColor Green
    } else {
        Write-Host "Build v?n có l?i. Ki?m tra chi ti?t v?i: dotnet build" -ForegroundColor Yellow
    }
} catch {
    Write-Host "L?i khi build: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Hoàn thành vi?c s?a l?i duplicate methods." -ForegroundColor Cyan
'@