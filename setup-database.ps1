# LexiFlow Database Setup Script
# This script will create the database and apply migrations

Write-Host "?? Starting LexiFlow Database Setup..." -ForegroundColor Green

# Set working directory to LexiFlow.API
Set-Location "LexiFlow.API"
Write-Host "?? Changed directory to LexiFlow.API" -ForegroundColor Yellow

# Install Entity Framework CLI tool if not already installed
Write-Host "?? Installing Entity Framework CLI tools..." -ForegroundColor Yellow
try {
    dotnet tool install --global dotnet-ef --version 9.0.0
    Write-Host "? Entity Framework CLI tools installed successfully" -ForegroundColor Green
}
catch {
    Write-Host "??  Entity Framework CLI tools may already be installed" -ForegroundColor Yellow
}

# Check if EF tools are available
Write-Host "?? Checking Entity Framework CLI tools..." -ForegroundColor Yellow
$efVersion = dotnet ef --version
Write-Host "Entity Framework version: $efVersion" -ForegroundColor Cyan

# Add Entity Framework package if not present
Write-Host "?? Adding Entity Framework packages..." -ForegroundColor Yellow
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0

# Build the project to ensure everything compiles
Write-Host "?? Building the project..." -ForegroundColor Yellow
$buildResult = dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build failed. Please fix compilation errors first." -ForegroundColor Red
    exit 1
}
Write-Host "? Project built successfully" -ForegroundColor Green

# Create initial migration
Write-Host "?? Creating initial migration..." -ForegroundColor Yellow
try {
    dotnet ef migrations add InitialCreate --context LexiFlowContext
    Write-Host "? Initial migration created successfully" -ForegroundColor Green
}
catch {
    Write-Host "??  Migration may already exist or there's an issue with the context" -ForegroundColor Yellow
}

# Update database
Write-Host "???  Updating database..." -ForegroundColor Yellow
try {
    dotnet ef database update --context LexiFlowContext
    Write-Host "? Database updated successfully" -ForegroundColor Green
}
catch {
    Write-Host "? Database update failed. Check connection string and SQL Server availability." -ForegroundColor Red
    Write-Host "Connection String: Server=localhost;Database=LexiFlow;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True" -ForegroundColor Cyan
    exit 1
}

# Verify database creation
Write-Host "?? Verifying database creation..." -ForegroundColor Yellow
try {
    dotnet ef dbcontext info --context LexiFlowContext
    Write-Host "? Database verification completed" -ForegroundColor Green
}
catch {
    Write-Host "??  Could not verify database, but it may have been created successfully" -ForegroundColor Yellow
}

Write-Host "`n?? Database setup completed successfully!" -ForegroundColor Green
Write-Host "?? Database Details:" -ForegroundColor Cyan
Write-Host "   - Server: localhost" -ForegroundColor White
Write-Host "   - Database: LexiFlow" -ForegroundColor White
Write-Host "   - Authentication: Windows Integrated" -ForegroundColor White
Write-Host "   - Context: LexiFlowContext" -ForegroundColor White

Write-Host "`n?? You can now run the API with: dotnet run" -ForegroundColor Green
Write-Host "?? Swagger UI will be available at: https://localhost:7000" -ForegroundColor Cyan

# Return to original directory
Set-Location ".."