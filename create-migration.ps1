# ===============================================================================
# LexiFlow Migration Creation Script for .NET 9
# ===============================================================================
# Script t?o migration cho LexiFlow v?i Entity Framework Core
# H? tr? .NET 9 v?i các tính n?ng t?i ?u hóa m?i nh?t
# ===============================================================================

Write-Host "?? LexiFlow Migration Creation Script - .NET 9" -ForegroundColor Cyan
Write-Host "===============================================" -ForegroundColor Cyan

# Ki?m tra .NET version
Write-Host "`n?? Checking .NET version..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "? .NET Version: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "? .NET is not installed or not in PATH" -ForegroundColor Red
    exit 1
}

# Ki?m tra EF Core tools
Write-Host "`n?? Checking EF Core Tools..." -ForegroundColor Yellow
try {
    $efVersion = dotnet ef --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? EF Core Tools: Installed" -ForegroundColor Green
    } else {
        Write-Host "??  EF Core Tools not found. Installing..." -ForegroundColor Yellow
        dotnet tool install --global dotnet-ef
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? EF Core Tools: Successfully installed" -ForegroundColor Green
        } else {
            Write-Host "? Failed to install EF Core Tools" -ForegroundColor Red
            exit 1
        }
    }
} catch {
    Write-Host "? Error checking EF Core Tools" -ForegroundColor Red
    exit 1
}

# Thi?t l?p bi?n môi tr??ng
$API_PROJECT = "LexiFlow.API"
$INFRASTRUCTURE_PROJECT = "LexiFlow.Infrastructure"
$MODELS_PROJECT = "LexiFlow.Models"

Write-Host "`n?? Project Structure:" -ForegroundColor Cyan
Write-Host "   API Project: $API_PROJECT" -ForegroundColor White
Write-Host "   Infrastructure Project: $INFRASTRUCTURE_PROJECT" -ForegroundColor White
Write-Host "   Models Project: $MODELS_PROJECT" -ForegroundColor White

# Ki?m tra các project t?n t?i
$projectPaths = @(
    "$API_PROJECT\$API_PROJECT.csproj",
    "$INFRASTRUCTURE_PROJECT\$INFRASTRUCTURE_PROJECT.csproj",
    "$MODELS_PROJECT\$MODELS_PROJECT.csproj"
)

foreach ($path in $projectPaths) {
    if (Test-Path $path) {
        Write-Host "? Found: $path" -ForegroundColor Green
    } else {
        Write-Host "? Missing: $path" -ForegroundColor Red
        Write-Host "Please ensure all projects are in the correct structure" -ForegroundColor Yellow
        exit 1
    }
}

# Function ?? t?o migration
function Create-Migration {
    param(
        [string]$MigrationName,
        [string]$Description = ""
    )
    
    Write-Host "`n?? Creating Migration: $MigrationName" -ForegroundColor Yellow
    if ($Description) {
        Write-Host "?? Description: $Description" -ForegroundColor Gray
    }
    
    try {
        # Chuy?n ??n th? m?c API project
        Push-Location $API_PROJECT
        
        # T?o migration
        $command = "dotnet ef migrations add $MigrationName --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --verbose"
        Write-Host "?? Running: $command" -ForegroundColor Gray
        
        Invoke-Expression $command
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? Migration '$MigrationName' created successfully!" -ForegroundColor Green
            
            # Hi?n th? file migration ???c t?o
            $migrationFiles = Get-ChildItem "../$INFRASTRUCTURE_PROJECT/Migrations" -Filter "*$MigrationName*" | Sort-Object LastWriteTime -Descending | Select-Object -First 3
            if ($migrationFiles) {
                Write-Host "?? Generated files:" -ForegroundColor Cyan
                foreach ($file in $migrationFiles) {
                    Write-Host "   - $($file.Name)" -ForegroundColor White
                }
            }
        } else {
            Write-Host "? Failed to create migration '$MigrationName'" -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Host "? Error creating migration: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    } finally {
        Pop-Location
    }
    
    return $true
}

# Function ?? update database
function Update-Database {
    param(
        [string]$TargetMigration = ""
    )
    
    Write-Host "`n?? Updating Database..." -ForegroundColor Yellow
    
    try {
        Push-Location $API_PROJECT
        
        $command = if ($TargetMigration) {
            "dotnet ef database update $TargetMigration --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --verbose"
        } else {
            "dotnet ef database update --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --verbose"
        }
        
        Write-Host "?? Running: $command" -ForegroundColor Gray
        Invoke-Expression $command
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? Database updated successfully!" -ForegroundColor Green
        } else {
            Write-Host "? Failed to update database" -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Host "? Error updating database: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    } finally {
        Pop-Location
    }
    
    return $true
}

# Function ?? xem migration history
function Show-MigrationHistory {
    Write-Host "`n?? Migration History:" -ForegroundColor Yellow
    
    try {
        Push-Location $API_PROJECT
        dotnet ef migrations list --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext
    } catch {
        Write-Host "? Error retrieving migration history: $($_.Exception.Message)" -ForegroundColor Red
    } finally {
        Pop-Location
    }
}

# Function ?? t?o migration script
function Generate-MigrationScript {
    param(
        [string]$FromMigration = "",
        [string]$ToMigration = "",
        [string]$OutputFile = "migration-script.sql"
    )
    
    Write-Host "`n?? Generating Migration Script..." -ForegroundColor Yellow
    
    try {
        Push-Location $API_PROJECT
        
        $command = if ($FromMigration -and $ToMigration) {
            "dotnet ef migrations script $FromMigration $ToMigration --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --output $OutputFile"
        } elseif ($FromMigration) {
            "dotnet ef migrations script $FromMigration --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --output $OutputFile"
        } else {
            "dotnet ef migrations script --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --output $OutputFile"
        }
        
        Write-Host "?? Running: $command" -ForegroundColor Gray
        Invoke-Expression $command
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? Migration script generated: $OutputFile" -ForegroundColor Green
        } else {
            Write-Host "? Failed to generate migration script" -ForegroundColor Red
        }
    } catch {
        Write-Host "? Error generating migration script: $($_.Exception.Message)" -ForegroundColor Red
    } finally {
        Pop-Location
    }
}

# Main Menu
function Show-Menu {
    Write-Host "`n?? What would you like to do?" -ForegroundColor Cyan
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host "1. Create Initial Migration" -ForegroundColor White
    Write-Host "2. Create Performance Optimization Migration" -ForegroundColor White
    Write-Host "3. Create Custom Migration" -ForegroundColor White
    Write-Host "4. Update Database" -ForegroundColor White
    Write-Host "5. Show Migration History" -ForegroundColor White
    Write-Host "6. Generate Migration Script" -ForegroundColor White
    Write-Host "7. Remove Last Migration" -ForegroundColor White
    Write-Host "8. Reset Database (Drop and Recreate)" -ForegroundColor White
    Write-Host "9. Exit" -ForegroundColor White
    Write-Host "================================" -ForegroundColor Cyan
}

# Main execution
do {
    Show-Menu
    $choice = Read-Host "`nEnter your choice (1-9)"
    
    switch ($choice) {
        "1" {
            $success = Create-Migration -MigrationName "InitialCreate" -Description "Initial database schema creation for LexiFlow"
            if ($success) {
                $updateChoice = Read-Host "`nWould you like to update the database now? (y/n)"
                if ($updateChoice -eq "y" -or $updateChoice -eq "Y") {
                    Update-Database
                }
            }
        }
        "2" {
            $success = Create-Migration -MigrationName "AddPerformanceIndexes" -Description "Add performance optimization indexes for .NET 9"
            if ($success) {
                $updateChoice = Read-Host "`nWould you like to update the database now? (y/n)"
                if ($updateChoice -eq "y" -or $updateChoice -eq "Y") {
                    Update-Database
                }
            }
        }
        "3" {
            $migrationName = Read-Host "`nEnter migration name"
            if ($migrationName) {
                $description = Read-Host "Enter description (optional)"
                $success = Create-Migration -MigrationName $migrationName -Description $description
                if ($success) {
                    $updateChoice = Read-Host "`nWould you like to update the database now? (y/n)"
                    if ($updateChoice -eq "y" -or $updateChoice -eq "Y") {
                        Update-Database
                    }
                }
            }
        }
        "4" {
            $targetMigration = Read-Host "`nEnter target migration name (press Enter for latest)"
            Update-Database -TargetMigration $targetMigration
        }
        "5" {
            Show-MigrationHistory
        }
        "6" {
            $fromMigration = Read-Host "`nEnter FROM migration name (press Enter to start from beginning)"
            $toMigration = Read-Host "Enter TO migration name (press Enter for latest)"
            $outputFile = Read-Host "Enter output file name (press Enter for 'migration-script.sql')"
            if (-not $outputFile) { $outputFile = "migration-script.sql" }
            Generate-MigrationScript -FromMigration $fromMigration -ToMigration $toMigration -OutputFile $outputFile
        }
        "7" {
            Write-Host "`n??  Removing last migration..." -ForegroundColor Yellow
            try {
                Push-Location $API_PROJECT
                dotnet ef migrations remove --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --force
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "? Last migration removed successfully!" -ForegroundColor Green
                } else {
                    Write-Host "? Failed to remove last migration" -ForegroundColor Red
                }
            } catch {
                Write-Host "? Error removing migration: $($_.Exception.Message)" -ForegroundColor Red
            } finally {
                Pop-Location
            }
        }
        "8" {
            Write-Host "`n??  WARNING: This will drop and recreate the entire database!" -ForegroundColor Red
            $confirmChoice = Read-Host "Are you sure? Type 'YES' to confirm"
            if ($confirmChoice -eq "YES") {
                try {
                    Push-Location $API_PROJECT
                    dotnet ef database drop --project ../$INFRASTRUCTURE_PROJECT --startup-project . --context LexiFlowContext --force
                    if ($LASTEXITCODE -eq 0) {
                        Write-Host "? Database dropped successfully!" -ForegroundColor Green
                        Update-Database
                    } else {
                        Write-Host "? Failed to drop database" -ForegroundColor Red
                    }
                } catch {
                    Write-Host "? Error dropping database: $($_.Exception.Message)" -ForegroundColor Red
                } finally {
                    Pop-Location
                }
            } else {
                Write-Host "Operation cancelled." -ForegroundColor Yellow
            }
        }
        "9" {
            Write-Host "`n?? Goodbye!" -ForegroundColor Green
            break
        }
        default {
            Write-Host "`n? Invalid choice. Please enter 1-9." -ForegroundColor Red
        }
    }
    
    if ($choice -ne "9") {
        Read-Host "`nPress Enter to continue..."
    }
} while ($choice -ne "9")

Write-Host "`n?? Script completed!" -ForegroundColor Green