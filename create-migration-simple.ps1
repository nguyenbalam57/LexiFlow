# LexiFlow Migration Creation Script for .NET 9
Write-Host "?? LexiFlow Migration Script" -ForegroundColor Cyan

# Function to create migration
function Create-LexiFlowMigration {
    param(
        [string]$Name,
        [string]$Description = ""
    )
    
    Write-Host "Creating migration: $Name" -ForegroundColor Yellow
    
    # Navigate to API project
    Set-Location "LexiFlow.API"
    
    # Create migration
    dotnet ef migrations add $Name --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Migration created successfully!" -ForegroundColor Green
        
        # Ask to update database
        $update = Read-Host "Update database now? (y/n)"
        if ($update -eq "y") {
            dotnet ef database update --project ../LexiFlow.Infrastructure --startup-project . --context LexiFlowContext
            if ($LASTEXITCODE -eq 0) {
                Write-Host "? Database updated!" -ForegroundColor Green
            }
        }
    } else {
        Write-Host "? Migration failed!" -ForegroundColor Red
    }
    
    Set-Location ..
}

# Menu
Write-Host "Select migration type:"
Write-Host "1. Initial Migration"
Write-Host "2. Performance Indexes"
Write-Host "3. Custom Migration"

$choice = Read-Host "Enter choice (1-3)"

switch ($choice) {
    "1" { Create-LexiFlowMigration -Name "InitialCreate" -Description "Initial database schema" }
    "2" { Create-LexiFlowMigration -Name "AddPerformanceIndexes" -Description "Performance optimization indexes" }
    "3" { 
        $name = Read-Host "Enter migration name"
        Create-LexiFlowMigration -Name $name 
    }
    default { Write-Host "Invalid choice" -ForegroundColor Red }
}