# ============================================================================
# SCRIPT KI?M TRA V� S?A L?I LEXIFLOWCONTEXT TR??C KHI T?O MIGRATION
# Phi�n b?n: .NET 9 - Entity Framework Core 9
# ============================================================================

param(
    [Parameter(Mandatory=$false)]
    [switch]$AutoFix = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$DetailedReport = $true
)

# M�u s?c console
function Write-ColorOutput($ForegroundColor, $Message) {
    $fc = $host.UI.RawUI.ForegroundColor
    $host.UI.RawUI.ForegroundColor = $ForegroundColor
    Write-Output $Message
    $host.UI.RawUI.ForegroundColor = $fc
}

function Write-Success($Message) { Write-ColorOutput Green "? $Message" }
function Write-Warning($Message) { Write-ColorOutput Yellow "?  $Message" }
function Write-Error($Message) { Write-ColorOutput Red "? $Message" }
function Write-Info($Message) { Write-ColorOutput Cyan "?  $Message" }
function Write-Title($Message) { Write-ColorOutput Magenta "?? $Message" }

Write-Title "LEXIFLOW CONTEXT VALIDATOR & FIXER"
Write-Info "Auto-fix enabled: $AutoFix"
Write-Info "Detailed report: $DetailedReport"
Write-Info ""

$contextFile = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs"
$issuesFound = @()
$fixesApplied = @()

# Ki?m tra file t?n t?i
if (-not (Test-Path $contextFile)) {
    Write-Error "LexiFlowContext.cs kh�ng t?n t?i t?i: $contextFile"
    exit 1
}

$originalContent = Get-Content $contextFile -Raw
$modifiedContent = $originalContent
$hasChanges = $false

Write-Title "1. KI?M TRA C� PH�P C? B?N"

# Ki?m tra basic syntax
if ($originalContent -match 'public class LexiFlowContext : DbContext') {
    Write-Success "Class definition ?�ng"
} else {
    Write-Error "Class definition c� v?n ??"
    $issuesFound += "Invalid class definition"
}

# Ki?m tra constructor
if ($originalContent -match 'public LexiFlowContext\(DbContextOptions<LexiFlowContext> options') {
    Write-Success "Constructor ?�ng format"
} else {
    Write-Warning "Constructor c� th? c� v?n ??"
    $issuesFound += "Constructor syntax issue"
}

Write-Title "2. KI?M TRA DBSETS"

# T�m t?t c? DbSets
$dbSetPattern = 'public DbSet<(\w+)> (\w+) \{ get; set; \}'
$dbSetMatches = [regex]::Matches($originalContent, $dbSetPattern)

$dbSets = @{}
$duplicateDbSets = @()

Write-Info "T�m th?y $($dbSetMatches.Count) DbSets"

foreach ($match in $dbSetMatches) {
    $entityType = $match.Groups[1].Value
    $dbSetName = $match.Groups[2].Value
    
    if ($dbSets.ContainsKey($dbSetName)) {
        $duplicateDbSets += $dbSetName
        Write-Error "Duplicate DbSet: $dbSetName"
        $issuesFound += "Duplicate DbSet: $dbSetName"
    } else {
        $dbSets[$dbSetName] = $entityType
        if ($DetailedReport) {
            Write-Info "  DbSet<$entityType> $dbSetName"
        }
    }
}

if ($duplicateDbSets.Count -eq 0) {
    Write-Success "Kh�ng c� duplicate DbSets"
} else {
    Write-Error "T�m th?y $($duplicateDbSets.Count) duplicate DbSets"
}

Write-Title "3. KI?M TRA FOREIGN KEY CONFIGURATIONS"

# T�m c�c configuration c� th? g�y xung ??t
$cascadePattern = '\.OnDelete\(DeleteBehavior\.Cascade\)'
$cascadeMatches = [regex]::Matches($originalContent, $cascadePattern)

if ($cascadeMatches.Count -gt 0) {
    Write-Warning "T�m th?y $($cascadeMatches.Count) Cascade delete configurations"
    Write-Info "Cascade delete c� th? g�y xung ??t trong relationships ph?c t?p"
    $issuesFound += "Multiple cascade delete configurations found"
    
    if ($AutoFix) {
        Write-Info "?ang chuy?n Cascade delete th�nh NoAction..."
        $modifiedContent = $modifiedContent -replace '\.OnDelete\(DeleteBehavior\.Cascade\)', '.OnDelete(DeleteBehavior.NoAction)'
        $hasChanges = $true
        $fixesApplied += "Changed Cascade to NoAction for safer relationships"
        Write-Success "?� chuy?n Cascade delete th�nh NoAction"
    }
}

# Ki?m tra SetNull configurations
$setNullPattern = '\.OnDelete\(DeleteBehavior\.SetNull\)'
$setNullMatches = [regex]::Matches($originalContent, $setNullPattern)

if ($setNullMatches.Count -gt 0) {
    Write-Info "T�m th?y $($setNullMatches.Count) SetNull delete configurations"
}

Write-Title "4. KI?M TRA INDEX CONFIGURATIONS"

# T�m c�c index configurations
$indexPattern = '\.HasIndex\([^)]+\)'
$indexMatches = [regex]::Matches($originalContent, $indexPattern)

Write-Info "T�m th?y $($indexMatches.Count) index configurations"

if ($DetailedReport) {
    foreach ($match in $indexMatches) {
        Write-Info "  Index: $($match.Value)"
    }
}

Write-Title "5. KI?M TRA NAVIGATION PROPERTIES"

# T�m c�c relationship configurations
$hasOnePattern = '\.HasOne\([^)]*\)'
$hasManyPattern = '\.HasMany\([^)]*\)'
$withOnePattern = '\.WithOne\([^)]*\)'
$withManyPattern = '\.WithMany\([^)]*\)'

$hasOneCount = [regex]::Matches($originalContent, $hasOnePattern).Count
$hasManyCount = [regex]::Matches($originalContent, $hasManyPattern).Count
$withOneCount = [regex]::Matches($originalContent, $withOnePattern).Count
$withManyCount = [regex]::Matches($originalContent, $withManyPattern).Count

Write-Info "Relationship configurations:"
Write-Info "  HasOne: $hasOneCount"
Write-Info "  HasMany: $hasManyCount"
Write-Info "  WithOne: $withOneCount"
Write-Info "  WithMany: $withManyCount"

# Ki?m tra balance
if ($hasOneCount -ne $withManyCount) {
    Write-Warning "C� th? c� v?n ?? v?i One-to-Many relationships (HasOne: $hasOneCount vs WithMany: $withManyCount)"
    $issuesFound += "Unbalanced One-to-Many relationships"
}

if ($hasManyCount -ne $withOneCount) {
    Write-Warning "C� th? c� v?n ?? v?i Many-to-One relationships (HasMany: $hasManyCount vs WithOne: $withOneCount)"
    $issuesFound += "Unbalanced Many-to-One relationships"
}

Write-Title "6. KI?M TRA GLOBAL DELETE BEHAVIOR"

if ($originalContent -match 'foreach \(var relationship in modelBuilder\.Model\.GetEntityTypes\(\)') {
    if ($originalContent -match 'relationship\.DeleteBehavior = DeleteBehavior\.NoAction') {
        Write-Success "Global cascade delete ?� ???c t?t ?�ng c�ch"
    } else {
        Write-Warning "Global delete behavior configuration c� th? c� v?n ??"
        $issuesFound += "Global delete behavior configuration issue"
    }
} else {
    Write-Warning "Kh�ng t�m th?y global delete behavior configuration"
    $issuesFound += "Missing global delete behavior configuration"
    
    if ($AutoFix) {
        Write-Info "?ang th�m global delete behavior configuration..."
        $globalDeleteConfig = @"

            // T?t cascade delete to�n c?c ?? tr�nh xung ??t trong relationships ph?c t?p
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
"@
        
        # T�m v? tr� OnModelCreating v� th�m config
        $onModelCreatingPattern = '(protected override void OnModelCreating\(ModelBuilder modelBuilder\)\s*\{\s*base\.OnModelCreating\(modelBuilder\);)'
        if ($modifiedContent -match $onModelCreatingPattern) {
            $modifiedContent = $modifiedContent -replace $onModelCreatingPattern, "`$1$globalDeleteConfig"
            $hasChanges = $true
            $fixesApplied += "Added global delete behavior configuration"
            Write-Success "?� th�m global delete behavior configuration"
        }
    }
}

Write-Title "7. KI?M TRA AUDIT ENTITY HANDLING"

if ($originalContent -match 'UpdateAuditableEntities\(\)') {
    Write-Success "Audit entity handling c� s?n"
    
    # Ki?m tra implementation
    if ($originalContent -match 'entity is AuditableEntity auditableEntity') {
        Write-Success "AuditableEntity handling ?�ng c�ch"
    } else {
        Write-Warning "AuditableEntity handling c� th? c� v?n ??"
        $issuesFound += "AuditableEntity handling issue"
    }
} else {
    Write-Warning "Kh�ng t�m th?y audit entity handling"
    $issuesFound += "Missing audit entity handling"
}

Write-Title "8. KI?M TRA USING STATEMENTS"

$requiredUsings = @(
    'using Microsoft.EntityFrameworkCore;',
    'using LexiFlow.Models.Core;',
    'using LexiFlow.Models.User;'
)

foreach ($using in $requiredUsings) {
    if ($originalContent -match [regex]::Escape($using)) {
        if ($DetailedReport) {
            Write-Success "Found: $using"
        }
    } else {
        Write-Warning "Missing: $using"
        $issuesFound += "Missing using statement: $using"
    }
}

Write-Title "9. KI?M TRA COMMENTS V� DOCUMENTATION"

$xmlCommentPattern = '/// <summary>'
$xmlCommentCount = [regex]::Matches($originalContent, $xmlCommentPattern).Count

Write-Info "T�m th?y $xmlCommentCount XML comments"

if ($xmlCommentCount -gt 10) {
    Write-Success "Documentation t?t - c� nhi?u XML comments"
} else {
    Write-Warning "N�n th�m nhi?u documentation/comments h?n"
}

# �p d?ng fixes n?u c� changes
if ($hasChanges -and $AutoFix) {
    Write-Title "�P D?NG FIXES"
    
    # T?o backup
    $backupFile = "$contextFile.backup.$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    Copy-Item $contextFile $backupFile
    Write-Success "Backup t?o t?i: $backupFile"
    
    # L?u changes
    Set-Content $contextFile $modifiedContent -Encoding UTF8
    Write-Success "?� �p d?ng t?t c? fixes v�o LexiFlowContext.cs"
    
    # Ki?m tra build sau khi fix
    Write-Info "Ki?m tra build sau khi fix..."
    $buildResult = dotnet build "LexiFlow.Infrastructure" --verbosity quiet 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Build th�nh c�ng sau khi fix!"
    } else {
        Write-Error "Build th?t b?i sau khi fix!"
        Write-Error $buildResult
        
        # Restore backup
        Copy-Item $backupFile $contextFile
        Write-Warning "?� restore backup do build th?t b?i"
    }
}

# B�o c�o t?ng k?t
Write-Title "T?NG K?T"

Write-Info "Issues found: $($issuesFound.Count)"
if ($issuesFound.Count -gt 0) {
    foreach ($issue in $issuesFound) {
        Write-Warning "  - $issue"
    }
}

if ($AutoFix) {
    Write-Info "Fixes applied: $($fixesApplied.Count)"
    if ($fixesApplied.Count -gt 0) {
        foreach ($fix in $fixesApplied) {
            Write-Success "  + $fix"
        }
    }
}

Write-Title "KHUY?N NGH?"

if ($issuesFound.Count -eq 0) {
    Write-Success "?? LexiFlowContext tr�ng t?t! S?n s�ng t?o migration."
    Write-Info "Ch?y l?nh sau ?? t?o migration:"
    Write-Info "  .\create-lexiflow-migration.ps1"
} else {
    Write-Warning "? C� $($issuesFound.Count) v?n ?? c?n x? l�"
    Write-Info "?? t? ??ng s?a, ch?y:"
    Write-Info "  .\validate-lexiflow-context.ps1 -AutoFix"
    Write-Info ""
    Write-Info "Ho?c s?a th? c�ng r?i ch?y:"
    Write-Info "  .\create-lexiflow-migration.ps1"
}

Write-Info ""
Write-Info "?? xem b�o c�o chi ti?t:"
Write-Info "  .\validate-lexiflow-context.ps1 -DetailedReport"
Write-Info ""
Write-Success "Validation completed!"

exit $(if ($issuesFound.Count -eq 0) { 0 } else { 1 })