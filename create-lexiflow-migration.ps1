# ============================================================================
# SCRIPT T?O MIGRATION HO�N CH?NH CHO LEXIFLOW
# Phi�n b?n: .NET 9 - Entity Framework Core 9
# M� t?: T?o migration, x? l� l?i v� c?p nh?t database
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

# M�u s?c cho console output
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

# Ki?m tra m�i tr??ng
Write-Info "=== KI?M TRA M�I TR??NG ==="

# Ki?m tra .NET version
$dotnetVersion = dotnet --version
Write-Info "Phi�n b?n .NET: $dotnetVersion"

if ($dotnetVersion -notmatch "^9\.") {
    Write-Warning "C?nh b�o: ?ang s? d?ng .NET $dotnetVersion, khuy?n ngh? s? d?ng .NET 9"
}

# Ki?m tra EF tools
try {
    $efVersion = dotnet ef --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Info "Entity Framework Tools: $efVersion"
    } else {
        Write-Error "Entity Framework Tools ch?a ???c c�i ??t!"
        Write-Info "C�i ??t b?ng l?nh: dotnet tool install --global dotnet-ef"
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

Write-Info "Th? m?c l�m vi?c: $WorkingDirectory"
Write-Info "Infrastructure Project: $InfrastructureProject"
Write-Info "Startup Project: $StartupProject"

# Ki?m tra project t?n t?i
if (-not (Test-Path "$InfrastructureProject\LexiFlow.Infrastructure.csproj")) {
    Write-Error "Kh�ng t�m th?y project Infrastructure: $InfrastructureProject\LexiFlow.Infrastructure.csproj"
    exit 1
}

if (-not (Test-Path "$StartupProject\LexiFlow.API.csproj")) {
    Write-Error "Kh�ng t�m th?y project Startup: $StartupProject\LexiFlow.API.csproj"
    exit 1
}

# Ki?m tra LexiFlowContext
$contextPath = "$InfrastructureProject\Data\LexiFlowContext.cs"
if (-not (Test-Path $contextPath)) {
    Write-Error "Kh�ng t�m th?y LexiFlowContext: $contextPath"
    exit 1
}
Write-Success "? LexiFlowContext t?n t?i"

# Ki?m tra connection string trong appsettings
$appSettingsPath = "$StartupProject\appsettings.json"
if (Test-Path $appSettingsPath) {
    $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
    if ($appSettings.ConnectionStrings -and $appSettings.ConnectionStrings.DefaultConnection) {
        Write-Success "? Connection string ???c c?u h�nh"
        Write-Info "Connection: $($appSettings.ConnectionStrings.DefaultConnection)"
    } else {
        Write-Warning "? Kh�ng t�m th?y connection string trong appsettings.json"
    }
} else {
    Write-Warning "? Kh�ng t�m th?y appsettings.json"
}

# Validation (n?u kh�ng b? qua)
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
    Write-Success "? Build th�nh c�ng"
}

# Backup migrations hi?n t?i (n?u c�)
$migrationsPath = "$InfrastructureProject\Migrations"
if (Test-Path $migrationsPath) {
    $backupPath = "$migrationsPath.backup.$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    if ($Force -or (Read-Host "T�m th?y migrations c?. Backup tr??c khi t?o m?i? (y/n)") -eq 'y') {
        try {
            Copy-Item $migrationsPath $backupPath -Recurse -Force
            Write-Success "? ?� backup migrations c? v�o: $backupPath"
            Remove-Item $migrationsPath -Recurse -Force
            Write-Info "? ?� x�a migrations c?"
        } catch {
            Write-Error "? L?i khi backup: $_"
            if (-not $Force) { exit 1 }
        }
    }
}

# T?o migration m?i
Write-Info "=== T?O MIGRATION ==="
Write-Info "T�n migration: $MigrationName"

try {
    Write-Info "?ang t?o migration..."
    
    $migrationCmd = "dotnet ef migrations add `"$MigrationName`" --project $InfrastructureProject --startup-project $StartupProject --verbose"
    Write-Info "L?nh: $migrationCmd"
    
    $migrationResult = Invoke-Expression $migrationCmd 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "? T?o migration th�nh c�ng!"
        Write-Info $migrationResult
    } else {
        Write-Error "? T?o migration th?t b?i!"
        Write-Error $migrationResult
        
        # Ph�n t�ch l?i v� ??a ra g?i �
        if ($migrationResult -match "pending model changes") {
            Write-Warning "?? G?i �: C� thay ??i model ch?a ???c sync"
            Write-Info "Th? ch?y l?i v?i t�n migration kh�c ho?c x�a migrations c?"
        }
        
        if ($migrationResult -match "SqlException") {
            Write-Warning "?? G?i �: L?i k?t n?i database"
            Write-Info "Ki?m tra connection string v� ??m b?o SQL Server ?ang ch?y"
        }
        
        if ($migrationResult -match "foreign key") {
            Write-Warning "?? G?i �: L?i foreign key constraint"
            Write-Info "Ki?m tra relationships trong LexiFlowContext"
        }
        
        if ($migrationResult -match "duplicate") {
            Write-Warning "?? G?i �: Tr�ng l?p t�n b?ng/c?t"
            Write-Info "Ki?m tra t�n entities v� properties trong models"
        }
        
        if (-not $Force) {
            Write-Info "S? d?ng -Force ?? ti?p t?c khi c� l?i"
            exit 1
        }
    }
} catch {
    Write-Error "? Exception khi t?o migration: $_"
    if (-not $Force) { exit 1 }
}

# C?p nh?t database (n?u ???c y�u c?u)
if ($UpdateDatabase -and $LASTEXITCODE -eq 0) {
    Write-Info "=== C?P NH?T DATABASE ==="
    
    try {
        Write-Info "?ang c?p nh?t database..."
        
        $updateCmd = "dotnet ef database update --project $InfrastructureProject --startup-project $StartupProject --verbose"
        Write-Info "L?nh: $updateCmd"
        
        $updateResult = Invoke-Expression $updateCmd 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "? C?p nh?t database th�nh c�ng!"
            Write-Info $updateResult
        } else {
            Write-Error "? C?p nh?t database th?t b?i!"
            Write-Error $updateResult
            
            # Ph�n t�ch l?i database
            if ($updateResult -match "connection") {
                Write-Warning "?? G?i �: L?i k?t n?i"
                Write-Info "Ki?m tra SQL Server v� connection string"
            }
            
            if ($updateResult -match "constraint") {
                Write-Warning "?? G?i �: L?i constraint"
                Write-Info "C� th? c?n x�a d? li?u test ho?c s?a foreign key"
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

# Seed data (n?u ???c y�u c?u v� migration th�nh c�ng)
if ($SeedData -and $LASTEXITCODE -eq 0) {
    Write-Info "=== SEED DATA ==="
    
    try {
        Write-Info "Ki?m tra DatabaseSeeder..."
        
        # T�m DatabaseSeeder
        $seederPath = "$InfrastructureProject\Data\Seed\DatabaseSeeder.cs"
        if (Test-Path $seederPath) {
            Write-Success "? T�m th?y DatabaseSeeder"
            
            # Ch?y application ?? seed data
            Write-Info "Seed data s? ???c th?c hi?n khi ch?y application l?n ??u"
            Write-Info "Ho?c c� th? ch?y th? c�ng qua API endpoint /api/admin/seed"
            
        } else {
            Write-Warning "? Kh�ng t�m th?y DatabaseSeeder"
            Write-Info "Seed data s? ???c th?c hi?n qua SeedDataAsync() trong LexiFlowContext"
        }
        
    } catch {
        Write-Warning "? L?i khi ki?m tra seed data: $_"
    }
}

# T�m t?t k?t qu?
Write-Info "=== T�M T?T ==="

if (Test-Path "$InfrastructureProject\Migrations") {
    $migrationFiles = Get-ChildItem "$InfrastructureProject\Migrations" -Filter "*.cs"
    Write-Success "? Migration ???c t?o th�nh c�ng"
    Write-Info "S? l??ng migration files: $($migrationFiles.Count)"
    Write-Info "Migration m?i nh?t: $($migrationFiles | Sort-Object Name -Descending | Select-Object -First 1 | Select-Object -ExpandProperty Name)"
} else {
    Write-Warning "? Th? m?c Migrations kh�ng t?n t?i"
}

# H??ng d?n ti?p theo
Write-Info "=== H??NG D?N TI?P THEO ==="
Write-Info "1. Ki?m tra migration files trong: $InfrastructureProject\Migrations\"
Write-Info "2. Review migration code tr??c khi deploy production"
Write-Info "3. Ch?y application ?? test: dotnet run --project $StartupProject"
Write-Info "4. Truy c?p Swagger UI ?? test API: https://localhost:7041/swagger"

if ($LASTEXITCODE -eq 0) {
    Write-Success "?? HO�N TH�NH TH�NH C�NG!"
} else {
    Write-Warning "? HO�N TH�NH V?I C?NH B�O - Ki?m tra logs ? tr�n"
}

# Debug information
Write-Info "=== DEBUG INFO ==="
Write-Info "Exit code: $LASTEXITCODE"
Write-Info "Script completed at: $(Get-Date)"

exit $LASTEXITCODE