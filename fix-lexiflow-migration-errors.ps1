# ============================================================================
# SCRIPT X? LÝ L?I MIGRATION CHO LEXIFLOW
# Phiên b?n: .NET 9 - Entity Framework Core 9
# Mô t?: T? ??ng phát hi?n và s?a các l?i migration th??ng g?p
# ============================================================================

param(
    [Parameter(Mandatory=$false)]
    [switch]$AutoFix = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$ResetMigrations = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$FixForeignKeys = $true,
    
    [Parameter(Mandatory=$false)]
    [switch]$FixDuplicates = $true
)

# Màu s?c cho console output
function Write-ColorOutput($ForegroundColor, $Message) {
    $fc = $host.UI.RawUI.ForegroundColor
    $host.UI.RawUI.ForegroundColor = $ForegroundColor
    Write-Output $Message
    $host.UI.RawUI.ForegroundColor = $fc
}

function Write-Success($Message) { Write-ColorOutput Green $Message }
function Write-Warning($Message) { Write-ColorOutput Yellow $Message }
function Write-Error($Message) { Write-ColorOutput Red $Message }
function Write-Info($Message) { Write-ColorOutput Cyan $Message }

Write-Info "=== LEXIFLOW MIGRATION ERROR FIXER ==="

# ???ng d?n projects
$InfrastructureProject = "LexiFlow.Infrastructure"
$StartupProject = "LexiFlow.API"
$ContextFile = "$InfrastructureProject\Data\LexiFlowContext.cs"

# 1. KI?M TRA VÀ S?A L?I TRONG LEXIFLOWCONTEXT
Write-Info "=== KI?M TRA LEXIFLOWCONTEXT ==="

if (-not (Test-Path $ContextFile)) {
    Write-Error "? Không tìm th?y LexiFlowContext.cs"
    exit 1
}

$contextContent = Get-Content $ContextFile -Raw

# S?a l?i duplicate OnDelete behaviors
if ($FixForeignKeys) {
    Write-Info "S?a l?i Foreign Key configurations..."
    
    # T?o backup
    $backupFile = "$ContextFile.backup.$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    Copy-Item $ContextFile $backupFile
    Write-Info "? Backup t?o t?i: $backupFile"
    
    # Danh sách các s?a l?i
    $fixes = @(
        @{
            Pattern = '\.OnDelete\(DeleteBehavior\.Cascade\)'
            Replacement = '.OnDelete(DeleteBehavior.NoAction)'
            Description = 'Thay ??i Cascade thành NoAction ?? tránh xung ??t'
        },
        @{
            Pattern = '\.OnDelete\(DeleteBehavior\.SetNull\)([^;]+?\.OnDelete\(DeleteBehavior\.SetNull\))'
            Replacement = '.OnDelete(DeleteBehavior.NoAction)'
            Description = 'S?a duplicate SetNull'
        }
    )
    
    $modified = $false
    foreach ($fix in $fixes) {
        if ($contextContent -match $fix.Pattern) {
            Write-Warning "Tìm th?y v?n ??: $($fix.Description)"
            if ($AutoFix) {
                $contextContent = $contextContent -replace $fix.Pattern, $fix.Replacement
                $modified = $true
                Write-Success "? ?ã s?a: $($fix.Description)"
            } else {
                Write-Info "?? t? ??ng s?a, s? d?ng -AutoFix"
            }
        }
    }
    
    if ($modified) {
        Set-Content $ContextFile $contextContent -Encoding UTF8
        Write-Success "? ?ã c?p nh?t LexiFlowContext.cs"
    }
}

# 2. KI?M TRA VÀ S?A L?I DUPLICATE DBSETS
if ($FixDuplicates) {
    Write-Info "=== KI?M TRA DUPLICATE DBSETS ==="
    
    $dbSetPattern = 'public DbSet<(\w+)> (\w+) { get; set; }'
    $dbSetMatches = [regex]::Matches($contextContent, $dbSetPattern)
    
    $dbSetNames = @{}
    $duplicates = @()
    
    foreach ($match in $dbSetMatches) {
        $entityName = $match.Groups[1].Value
        $dbSetName = $match.Groups[2].Value
        
        if ($dbSetNames.ContainsKey($dbSetName)) {
            $duplicates += $dbSetName
            Write-Warning "? Duplicate DbSet tìm th?y: $dbSetName"
        } else {
            $dbSetNames[$dbSetName] = $entityName
        }
    }
    
    if ($duplicates.Count -eq 0) {
        Write-Success "? Không tìm th?y duplicate DbSets"
    } else {
        Write-Error "? Tìm th?y $($duplicates.Count) duplicate DbSets"
        if ($AutoFix) {
            Write-Info "S? d?ng -ResetMigrations ?? xóa migrations và t?o l?i"
        }
    }
}

# 3. KI?M TRA VÀ S?A L?I MODEL RELATIONSHIPS
Write-Info "=== KI?M TRA MODEL RELATIONSHIPS ==="

# Ki?m tra circular references
$circularPatterns = @(
    'entity\.HasOne.*\.WithMany.*\.HasForeignKey.*\.HasOne',
    'entity\.HasMany.*\.WithOne.*\.HasForeignKey.*\.HasMany'
)

foreach ($pattern in $circularPatterns) {
    if ($contextContent -match $pattern) {
        Write-Warning "? Có th? có circular reference trong relationships"
        Write-Info "Ki?m tra th? công các relationships trong OnModelCreating"
        break
    }
}

# 4. X? LÝ RESET MIGRATIONS
if ($ResetMigrations) {
    Write-Info "=== RESET MIGRATIONS ==="
    
    $migrationsPath = "$InfrastructureProject\Migrations"
    if (Test-Path $migrationsPath) {
        $confirm = Read-Host "? C?NH BÁO: S? xóa t?t c? migrations. Ti?p t?c? (yes/no)"
        if ($confirm -eq 'yes') {
            try {
                # Backup tr??c khi xóa
                $backupMigrationsPath = "$migrationsPath.backup.$(Get-Date -Format 'yyyyMMdd_HHmmss')"
                Copy-Item $migrationsPath $backupMigrationsPath -Recurse
                Write-Info "? Backup migrations: $backupMigrationsPath"
                
                # Xóa migrations
                Remove-Item $migrationsPath -Recurse -Force
                Write-Success "? ?ã xóa migrations c?"
                
                # Xóa database (n?u s? d?ng LocalDB)
                try {
                    $dropDbResult = dotnet ef database drop --project $InfrastructureProject --startup-project $StartupProject --force 2>&1
                    if ($LASTEXITCODE -eq 0) {
                        Write-Success "? ?ã xóa database"
                    } else {
                        Write-Warning "? Không th? xóa database: $dropDbResult"
                    }
                } catch {
                    Write-Warning "? L?i khi xóa database: $_"
                }
                
            } catch {
                Write-Error "? L?i khi reset migrations: $_"
                exit 1
            }
        } else {
            Write-Info "H?y reset migrations"
        }
    } else {
        Write-Info "Không tìm th?y migrations ?? reset"
    }
}

# 5. KI?M TRA CONNECTION STRING
Write-Info "=== KI?M TRA CONNECTION STRING ==="

$appSettingsPath = "$StartupProject\appsettings.json"
if (Test-Path $appSettingsPath) {
    try {
        $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
        $connectionString = $appSettings.ConnectionStrings.DefaultConnection
        
        if ($connectionString) {
            Write-Success "? Connection string có s?n"
            
            # Ki?m tra lo?i database
            if ($connectionString -match "LocalDB|\\SQLEXPRESS") {
                Write-Info "Database: SQL Server LocalDB/Express"
                
                # Ki?m tra LocalDB instance
                try {
                    $sqllocaldbInfo = sqllocaldb info 2>&1
                    if ($LASTEXITCODE -eq 0) {
                        Write-Success "? SQL LocalDB available"
                    } else {
                        Write-Warning "? SQL LocalDB có th? không available"
                    }
                } catch {
                    Write-Warning "? Không th? ki?m tra LocalDB status"
                }
                
            } elseif ($connectionString -match "Server=") {
                Write-Info "Database: SQL Server"
            } else {
                Write-Warning "? Lo?i database không xác ??nh"
            }
            
        } else {
            Write-Error "? Connection string không t?n t?i"
        }
        
    } catch {
        Write-Error "? L?i khi ??c appsettings.json: $_"
    }
} else {
    Write-Error "? Không tìm th?y appsettings.json"
}

# 6. KI?M TRA ENTITY MODELS
Write-Info "=== KI?M TRA ENTITY MODELS ==="

$modelsPath = "LexiFlow.Models"
if (Test-Path $modelsPath) {
    # Tìm các model có [Key] attribute
    $modelFiles = Get-ChildItem $modelsPath -Recurse -Filter "*.cs"
    $modelsWithKey = @()
    $modelsWithoutKey = @()
    
    foreach ($file in $modelFiles) {
        $content = Get-Content $file.FullName -Raw
        if ($content -match '\[Key\]') {
            $modelsWithKey += $file.Name
        } elseif ($content -match 'public class \w+ : .*Entity') {
            $modelsWithoutKey += $file.Name
        }
    }
    
    Write-Info "Models v?i [Key] attribute: $($modelsWithKey.Count)"
    Write-Info "Models có th? thi?u [Key]: $($modelsWithoutKey.Count)"
    
    if ($modelsWithoutKey.Count -gt 0) {
        Write-Warning "? M?t s? models có th? thi?u [Key] attribute:"
        $modelsWithoutKey | Select-Object -First 5 | ForEach-Object { Write-Info "  - $_" }
    }
} else {
    Write-Warning "? Không tìm th?y th? m?c Models"
}

# 7. KI?M TRA BUILD
Write-Info "=== KI?M TRA BUILD ==="

try {
    Write-Info "?ang build Infrastructure project..."
    $buildResult = dotnet build $InfrastructureProject --verbosity quiet 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? Build Infrastructure thành công"
    } else {
        Write-Error "? Build Infrastructure th?t b?i!"
        Write-Error $buildResult
        
        # Phân tích l?i build
        if ($buildResult -match "CS0234") {
            Write-Warning "?? L?i namespace - ki?m tra using statements"
        }
        if ($buildResult -match "CS0246") {
            Write-Warning "?? L?i type not found - ki?m tra references"
        }
        if ($buildResult -match "CS0115") {
            Write-Warning "?? L?i override - ki?m tra method signatures"
        }
    }
} catch {
    Write-Error "? Exception khi build: $_"
}

# 8. T?O MIGRATION S?A L?I (n?u có AutoFix)
if ($AutoFix -and $LASTEXITCODE -eq 0) {
    Write-Info "=== T?O MIGRATION S?A L?I ==="
    
    $fixMigrationName = "FixNavigationAndConstraints_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    
    try {
        Write-Info "T?o migration: $fixMigrationName"
        $migrationResult = dotnet ef migrations add $fixMigrationName --project $InfrastructureProject --startup-project $StartupProject 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "? Migration s?a l?i ???c t?o thành công"
            
            # C?p nh?t database
            Write-Info "C?p nh?t database..."
            $updateResult = dotnet ef database update --project $InfrastructureProject --startup-project $StartupProject 2>&1
            
            if ($LASTEXITCODE -eq 0) {
                Write-Success "? Database ???c c?p nh?t thành công"
            } else {
                Write-Error "? C?p nh?t database th?t b?i: $updateResult"
            }
        } else {
            Write-Error "? T?o migration th?t b?i: $migrationResult"
        }
        
    } catch {
        Write-Error "? Exception khi t?o migration: $_"
    }
}

# TÓM T?T K?T QU?
Write-Info "=== TÓM T?T K?T QU? ==="

if (Test-Path "$InfrastructureProject\Migrations") {
    $migrationCount = (Get-ChildItem "$InfrastructureProject\Migrations" -Filter "*.cs").Count
    Write-Info "S? migrations hi?n t?i: $migrationCount"
} else {
    Write-Warning "Không có migrations"
}

Write-Info "=== H??NG D?N X? LÝ L?I ==="
Write-Info "1. L?i Foreign Key Constraint:"
Write-Info "   - S? d?ng: .\fix-lexiflow-migration-errors.ps1 -FixForeignKeys -AutoFix"
Write-Info ""
Write-Info "2. L?i Duplicate Entities:"
Write-Info "   - S? d?ng: .\fix-lexiflow-migration-errors.ps1 -FixDuplicates -AutoFix"
Write-Info ""
Write-Info "3. Reset hoàn toàn Migration:"
Write-Info "   - S? d?ng: .\fix-lexiflow-migration-errors.ps1 -ResetMigrations"
Write-Info ""
Write-Info "4. Sau khi s?a l?i, t?o migration m?i:"
Write-Info "   - S? d?ng: .\create-lexiflow-migration.ps1"

Write-Success "?? Script x? lý l?i migration hoàn thành!"