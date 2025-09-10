# ============================================================================
# SCRIPT T?O MIGRATION HOÀN CH?NH CHO LEXIFLOW
# Phiên b?n: .NET 9 - Entity Framework Core 9
# Mô t?: T?o migration, x? lý l?i và c?p nh?t database
# ============================================================================

param(
    [Parameter(Mandatory=$false)]
    [string]$MigrationName = "LexiFlowInitialMigration",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipValidation = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$Force = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$UpdateDatabase = $true,
    
    [Parameter(Mandatory=$false)]
    [switch]$SeedData = $true
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

# Ki?m tra môi tr??ng
Write-Info "=== KI?M TRA MÔI TR??NG ==="

# Ki?m tra .NET version
$dotnetVersion = dotnet --version
Write-Info "Phiên b?n .NET: $dotnetVersion"

if ($dotnetVersion -notmatch "^9\.") {
    Write-Warning "C?nh báo: ?ang s? d?ng .NET $dotnetVersion, khuy?n ngh? s? d?ng .NET 9"
}

# Ki?m tra EF tools
try {
    $efVersion = dotnet ef --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Info "Entity Framework Tools: $efVersion"
    } else {
        Write-Error "Entity Framework Tools ch?a ???c cài ??t!"
        Write-Info "Cài ??t b?ng l?nh: dotnet tool install --global dotnet-ef"
        exit 1
    }
} catch {
    Write-Error "L?i khi ki?m tra EF Tools: $_"
    exit 1
}

# ???ng d?n projects
$InfrastructureProject = "LexiFlow.Infrastructure"
$StartupProject = "LexiFlow.API"
$WorkingDirectory = Get-Location

Write-Info "Th? m?c làm vi?c: $WorkingDirectory"
Write-Info "Infrastructure Project: $InfrastructureProject"
Write-Info "Startup Project: $StartupProject"

# Ki?m tra project t?n t?i
if (-not (Test-Path "$InfrastructureProject\LexiFlow.Infrastructure.csproj")) {
    Write-Error "Không tìm th?y project Infrastructure: $InfrastructureProject\LexiFlow.Infrastructure.csproj"
    exit 1
}

if (-not (Test-Path "$StartupProject\LexiFlow.API.csproj")) {
    Write-Error "Không tìm th?y project Startup: $StartupProject\LexiFlow.API.csproj"
    exit 1
}

# Ki?m tra LexiFlowContext
$contextPath = "$InfrastructureProject\Data\LexiFlowContext.cs"
if (-not (Test-Path $contextPath)) {
    Write-Error "Không tìm th?y LexiFlowContext: $contextPath"
    exit 1
}
Write-Success "? LexiFlowContext t?n t?i"

# Ki?m tra connection string trong appsettings
$appSettingsPath = "$StartupProject\appsettings.json"
if (Test-Path $appSettingsPath) {
    $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
    if ($appSettings.ConnectionStrings -and $appSettings.ConnectionStrings.DefaultConnection) {
        Write-Success "? Connection string ???c c?u hình"
        Write-Info "Connection: $($appSettings.ConnectionStrings.DefaultConnection)"
    } else {
        Write-Warning "? Không tìm th?y connection string trong appsettings.json"
    }
} else {
    Write-Warning "? Không tìm th?y appsettings.json"
}

# Validation (n?u không b? qua)
if (-not $SkipValidation) {
    Write-Info "=== VALIDATION ==="
    
    # Build ?? ki?m tra l?i compilation
    Write-Info "Ki?m tra compilation..."
    $buildResult = dotnet build $InfrastructureProject --verbosity quiet 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error "? Build th?t b?i!"
        Write-Error $buildResult
        Write-Info "S? d?ng -SkipValidation ?? b? qua validation"
        exit 1
    }
    Write-Success "? Build thành công"
}

# Backup migrations hi?n t?i (n?u có)
$migrationsPath = "$InfrastructureProject\Migrations"
if (Test-Path $migrationsPath) {
    $backupPath = "$migrationsPath.backup.$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    if ($Force -or (Read-Host "Tìm th?y migrations c?. Backup tr??c khi t?o m?i? (y/n)") -eq 'y') {
        try {
            Copy-Item $migrationsPath $backupPath -Recurse -Force
            Write-Success "? ?ã backup migrations c? vào: $backupPath"
            Remove-Item $migrationsPath -Recurse -Force
            Write-Info "? ?ã xóa migrations c?"
        } catch {
            Write-Error "? L?i khi backup: $_"
            if (-not $Force) { exit 1 }
        }
    }
}

# T?o migration m?i
Write-Info "=== T?O MIGRATION ==="
Write-Info "Tên migration: $MigrationName"

try {
    Write-Info "?ang t?o migration..."
    
    $migrationCmd = "dotnet ef migrations add `"$MigrationName`" --project $InfrastructureProject --startup-project $StartupProject --verbose"
    Write-Info "L?nh: $migrationCmd"
    
    $migrationResult = Invoke-Expression $migrationCmd 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? T?o migration thành công!"
        Write-Info $migrationResult
    } else {
        Write-Error "? T?o migration th?t b?i!"
        Write-Error $migrationResult
        
        # Phân tích l?i và ??a ra g?i ý
        if ($migrationResult -match "pending model changes") {
            Write-Warning "?? G?i ý: Có thay ??i model ch?a ???c sync"
            Write-Info "Th? ch?y l?i v?i tên migration khác ho?c xóa migrations c?"
        }
        
        if ($migrationResult -match "SqlException") {
            Write-Warning "?? G?i ý: L?i k?t n?i database"
            Write-Info "Ki?m tra connection string và ??m b?o SQL Server ?ang ch?y"
        }
        
        if ($migrationResult -match "foreign key") {
            Write-Warning "?? G?i ý: L?i foreign key constraint"
            Write-Info "Ki?m tra relationships trong LexiFlowContext"
        }
        
        if ($migrationResult -match "duplicate") {
            Write-Warning "?? G?i ý: Trùng l?p tên b?ng/c?t"
            Write-Info "Ki?m tra tên entities và properties trong models"
        }
        
        if (-not $Force) {
            Write-Info "S? d?ng -Force ?? ti?p t?c khi có l?i"
            exit 1
        }
    }
} catch {
    Write-Error "? Exception khi t?o migration: $_"
    if (-not $Force) { exit 1 }
}

# C?p nh?t database (n?u ???c yêu c?u)
if ($UpdateDatabase -and $LASTEXITCODE -eq 0) {
    Write-Info "=== C?P NH?T DATABASE ==="
    
    try {
        Write-Info "?ang c?p nh?t database..."
        
        $updateCmd = "dotnet ef database update --project $InfrastructureProject --startup-project $StartupProject --verbose"
        Write-Info "L?nh: $updateCmd"
        
        $updateResult = Invoke-Expression $updateCmd 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "? C?p nh?t database thành công!"
            Write-Info $updateResult
        } else {
            Write-Error "? C?p nh?t database th?t b?i!"
            Write-Error $updateResult
            
            # Phân tích l?i database
            if ($updateResult -match "connection") {
                Write-Warning "?? G?i ý: L?i k?t n?i"
                Write-Info "Ki?m tra SQL Server và connection string"
            }
            
            if ($updateResult -match "constraint") {
                Write-Warning "?? G?i ý: L?i constraint"
                Write-Info "Có th? c?n xóa d? li?u test ho?c s?a foreign key"
            }
            
            if (-not $Force) {
                exit 1
            }
        }
    } catch {
        Write-Error "? Exception khi c?p nh?t database: $_"
        if (-not $Force) { exit 1 }
    }
}

# Seed data (n?u ???c yêu c?u và migration thành công)
if ($SeedData -and $LASTEXITCODE -eq 0) {
    Write-Info "=== SEED DATA ==="
    
    try {
        Write-Info "Ki?m tra DatabaseSeeder..."
        
        # Tìm DatabaseSeeder
        $seederPath = "$InfrastructureProject\Data\Seed\DatabaseSeeder.cs"
        if (Test-Path $seederPath) {
            Write-Success "? Tìm th?y DatabaseSeeder"
            
            # Ch?y application ?? seed data
            Write-Info "Seed data s? ???c th?c hi?n khi ch?y application l?n ??u"
            Write-Info "Ho?c có th? ch?y th? công qua API endpoint /api/admin/seed"
            
        } else {
            Write-Warning "? Không tìm th?y DatabaseSeeder"
            Write-Info "Seed data s? ???c th?c hi?n qua SeedDataAsync() trong LexiFlowContext"
        }
        
    } catch {
        Write-Warning "? L?i khi ki?m tra seed data: $_"
    }
}

# Tóm t?t k?t qu?
Write-Info "=== TÓM T?T ==="

if (Test-Path "$InfrastructureProject\Migrations") {
    $migrationFiles = Get-ChildItem "$InfrastructureProject\Migrations" -Filter "*.cs"
    Write-Success "? Migration ???c t?o thành công"
    Write-Info "S? l??ng migration files: $($migrationFiles.Count)"
    Write-Info "Migration m?i nh?t: $($migrationFiles | Sort-Object Name -Descending | Select-Object -First 1 | Select-Object -ExpandProperty Name)"
} else {
    Write-Warning "? Th? m?c Migrations không t?n t?i"
}

# H??ng d?n ti?p theo
Write-Info "=== H??NG D?N TI?P THEO ==="
Write-Info "1. Ki?m tra migration files trong: $InfrastructureProject\Migrations\"
Write-Info "2. Review migration code tr??c khi deploy production"
Write-Info "3. Ch?y application ?? test: dotnet run --project $StartupProject"
Write-Info "4. Truy c?p Swagger UI ?? test API: https://localhost:7041/swagger"

if ($LASTEXITCODE -eq 0) {
    Write-Success "?? HOÀN THÀNH THÀNH CÔNG!"
} else {
    Write-Warning "? HOÀN THÀNH V?I C?NH BÁO - Ki?m tra logs ? trên"
}

# Debug information
Write-Info "=== DEBUG INFO ==="
Write-Info "Exit code: $LASTEXITCODE"
Write-Info "Script completed at: $(Get-Date)"

exit $LASTEXITCODE