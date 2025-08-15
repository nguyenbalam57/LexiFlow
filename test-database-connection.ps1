# Test Database Connection and API
# Ki?m tra k?t n?i database và API

Write-Host "=== KI?M TRA K?T N?I DATABASE VÀ API ===" -ForegroundColor Cyan

# 1. Ki?m tra SQL Server services
Write-Host "`n1. Ki?m tra SQL Server Services..." -ForegroundColor Yellow

$sqlServices = @(
    "MSSQL`$SQLEXPRESS",
    "MSSQLSERVER", 
    "SQLBrowser",
    "SQLSERVERAGENT"
)

foreach ($service in $sqlServices) {
    try {
        $svc = Get-Service -Name $service -ErrorAction SilentlyContinue
        if ($svc) {
            $status = $svc.Status
            $color = if ($status -eq "Running") { "Green" } else { "Red" }
            Write-Host "   $service : $status" -ForegroundColor $color
        }
    }
    catch {
        Write-Host "   $service : Not Found" -ForegroundColor Gray
    }
}

# 2. Test SQL Server connection
Write-Host "`n2. Test k?t n?i SQL Server..." -ForegroundColor Yellow

$connectionStrings = @(
    @{
        Name = "SQL Server Express"
        ConnectionString = "Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True"
    },
    @{
        Name = "LocalDB"
        ConnectionString = "Server=(localdb)\mssqllocaldb;Database=master;Trusted_Connection=True;TrustServerCertificate=True"
    },
    @{
        Name = "Local SQL Server"
        ConnectionString = "Server=.;Database=master;Trusted_Connection=True;TrustServerCertificate=True"
    }
)

foreach ($conn in $connectionStrings) {
    try {
        $sqlConnection = New-Object System.Data.SqlClient.SqlConnection($conn.ConnectionString)
        $sqlConnection.Open()
        Write-Host "   ? $($conn.Name): Connected" -ForegroundColor Green
        $sqlConnection.Close()
    }
    catch {
        Write-Host "   ? $($conn.Name): $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 3. Ki?m tra database LexiFlow có t?n t?i không
Write-Host "`n3. Ki?m tra database LexiFlow..." -ForegroundColor Yellow

$workingConnection = $null
foreach ($conn in $connectionStrings) {
    try {
        $sqlConnection = New-Object System.Data.SqlClient.SqlConnection($conn.ConnectionString)
        $sqlConnection.Open()
        
        $checkDbCmd = $sqlConnection.CreateCommand()
        $checkDbCmd.CommandText = "SELECT name FROM sys.databases WHERE name = 'LexiFlow'"
        $result = $checkDbCmd.ExecuteScalar()
        
        if ($result) {
            Write-Host "   ? Database LexiFlow exists on $($conn.Name)" -ForegroundColor Green
            $workingConnection = $conn.ConnectionString.Replace("Database=master", "Database=LexiFlow")
        } else {
            Write-Host "   ? Database LexiFlow not found on $($conn.Name)" -ForegroundColor Yellow
        }
        
        $sqlConnection.Close()
        break
    }
    catch {
        continue
    }
}

# 4. Ki?m tra tables trong database n?u có
if ($workingConnection) {
    Write-Host "`n4. Ki?m tra tables trong database LexiFlow..." -ForegroundColor Yellow
    
    try {
        $sqlConnection = New-Object System.Data.SqlClient.SqlConnection($workingConnection)
        $sqlConnection.Open()
        
        $tablesCmd = $sqlConnection.CreateCommand()
        $tablesCmd.CommandText = @"
            SELECT 
                t.TABLE_NAME,
                (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS c WHERE c.TABLE_NAME = t.TABLE_NAME) as ColumnCount
            FROM INFORMATION_SCHEMA.TABLES t 
            WHERE t.TABLE_TYPE = 'BASE TABLE' 
            ORDER BY t.TABLE_NAME
"@
        
        $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($tablesCmd)
        $dataSet = New-Object System.Data.DataSet
        $adapter.Fill($dataSet) | Out-Null
        
        if ($dataSet.Tables[0].Rows.Count -gt 0) {
            Write-Host "   ? Found $($dataSet.Tables[0].Rows.Count) tables:" -ForegroundColor Green
            
            # Show only important tables
            $importantTables = @("Users", "Vocabularies", "Kanjis", "Categories", "Grammars", "LearningProgresses")
            foreach ($row in $dataSet.Tables[0].Rows) {
                if ($importantTables -contains $row.TABLE_NAME) {
                    Write-Host "     ? $($row.TABLE_NAME) ($($row.ColumnCount) columns)" -ForegroundColor Green
                }
            }
            Write-Host "     ... and $($dataSet.Tables[0].Rows.Count - $importantTables.Count) other tables" -ForegroundColor Gray
        } else {
            Write-Host "   ? No tables found - database might be empty" -ForegroundColor Yellow
        }
        
        $sqlConnection.Close()
    }
    catch {
        Write-Host "   ? Error checking tables: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 5. Start API và test endpoints
Write-Host "`n5. Kh?i ??ng API và test endpoints..." -ForegroundColor Yellow

# Check if API is already running on the expected ports
$httpPort = 5117
$httpsPort = 7276

$httpRunning = Test-NetConnection -ComputerName localhost -Port $httpPort -InformationLevel Quiet -WarningAction SilentlyContinue
$httpsRunning = Test-NetConnection -ComputerName localhost -Port $httpsPort -InformationLevel Quiet -WarningAction SilentlyContinue

if ($httpRunning -or $httpsRunning) {
    Write-Host "   API ?ang ch?y" -ForegroundColor Green
    if ($httpRunning) { Write-Host "     HTTP: http://localhost:$httpPort" -ForegroundColor White }
    if ($httpsRunning) { Write-Host "     HTTPS: https://localhost:$httpsPort" -ForegroundColor White }
} else {
    Write-Host "   Kh?i ??ng API..." -ForegroundColor Yellow
    
    # Change to API directory and start
    Push-Location "LexiFlow.API"
    try {
        $apiJob = Start-Job -ScriptBlock { 
            Set-Location $using:PWD
            dotnet run --urls "https://localhost:7276;http://localhost:5117"
        }
        Start-Sleep 10  # Wait for API to start
        Write-Host "   API started (Job ID: $($apiJob.Id))" -ForegroundColor Green
    }
    finally {
        Pop-Location
    }
}

# 6. Test API endpoints
Write-Host "`n6. Test API endpoints..." -ForegroundColor Yellow

# Try both HTTP and HTTPS
$apiUrls = @("https://localhost:7276", "http://localhost:5117")
$workingUrl = $null

foreach ($url in $apiUrls) {
    try {
        $response = Invoke-RestMethod -Uri "$url/api/health" -Method Get -TimeoutSec 5 -SkipCertificateCheck
        $workingUrl = $url
        Write-Host "   ? API accessible at: $url" -ForegroundColor Green
        break
    }
    catch {
        Write-Host "   ? Failed to connect to: $url" -ForegroundColor Red
    }
}

if ($workingUrl) {
    $endpoints = @(
        @{ Name = "Health Check"; Url = "$workingUrl/api/health" },
        @{ Name = "Database Health"; Url = "$workingUrl/api/health/database" },
        @{ Name = "Database Schema"; Url = "$workingUrl/api/health/database/schema" }
    )

    foreach ($endpoint in $endpoints) {
        try {
            Write-Host "   Testing $($endpoint.Name)..." -ForegroundColor Gray
            
            $response = Invoke-RestMethod -Uri $endpoint.Url -Method Get -TimeoutSec 10 -SkipCertificateCheck
            
            if ($response.status -eq "Healthy" -or $response.status -eq "Success") {
                Write-Host "   ? $($endpoint.Name): OK" -ForegroundColor Green
                
                # Show some details for database endpoints
                if ($endpoint.Name -eq "Database Health" -and $response.databaseInfo) {
                    Write-Host "     Database: $($response.databaseInfo.name)" -ForegroundColor White
                    Write-Host "     Response time: $($response.responseTime)ms" -ForegroundColor White
                    if ($response.databaseInfo.statistics) {
                        Write-Host "     Users: $($response.databaseInfo.statistics.users)" -ForegroundColor White
                        Write-Host "     Vocabularies: $($response.databaseInfo.statistics.vocabularies)" -ForegroundColor White
                        Write-Host "     Kanjis: $($response.databaseInfo.statistics.kanjis)" -ForegroundColor White
                    }
                }
                
                if ($endpoint.Name -eq "Database Schema" -and $response.tableCount) {
                    Write-Host "     Tables: $($response.tableCount)" -ForegroundColor White
                    Write-Host "     Last migration: $($response.lastMigration)" -ForegroundColor White
                }
            } else {
                Write-Host "   ? $($endpoint.Name): $($response.status)" -ForegroundColor Yellow
                if ($response.message) {
                    Write-Host "     Message: $($response.message)" -ForegroundColor Yellow
                }
            }
        }
        catch {
            Write-Host "   ? $($endpoint.Name): $($_.Exception.Message)" -ForegroundColor Red
        }
    }

    # 7. Test CRUD operations
    Write-Host "`n7. Test CRUD operations..." -ForegroundColor Yellow

    try {
        $crudUrl = "$workingUrl/api/health/database/test-crud"
        $crudResponse = Invoke-RestMethod -Uri $crudUrl -Method Post -TimeoutSec 15 -SkipCertificateCheck
        
        if ($crudResponse.status -eq "Success") {
            Write-Host "   ? CRUD operations: OK" -ForegroundColor Green
            Write-Host "     Test ID: $($crudResponse.testData.id)" -ForegroundColor White
            Write-Host "     Operations: $($crudResponse.testData.operations -join ', ')" -ForegroundColor White
        } else {
            Write-Host "   ? CRUD operations failed: $($crudResponse.message)" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "   ? CRUD test failed: $($_.Exception.Message)" -ForegroundColor Red
    }
} else {
    Write-Host "   ? API không th? truy c?p ???c" -ForegroundColor Red
}

Write-Host "`n=== KI?M TRA HOÀN T?T ===" -ForegroundColor Cyan
Write-Host "`nH??ng d?n ti?p theo:" -ForegroundColor Yellow
Write-Host "1. N?u database ch?a có, ch?y: dotnet ef database update" -ForegroundColor White
if ($workingUrl) {
    Write-Host "2. ?? xem Swagger UI: $workingUrl/" -ForegroundColor White
    Write-Host "3. ?? test endpoints: $workingUrl/api/health" -ForegroundColor White
    Write-Host "4. ?? seed d? li?u m?u: POST $workingUrl/api/health/database/test-crud" -ForegroundColor White
} else {
    Write-Host "2. Kh?i ??ng API b?ng: cd LexiFlow.API && dotnet run" -ForegroundColor White
    Write-Host "3. Swagger UI s? có t?i: https://localhost:7276/" -ForegroundColor White
}

# Clean up background jobs
Get-Job | Where-Object { $_.State -eq "Running" -and $_.Name -like "*dotnet*" } | Stop-Job -PassThru | Remove-Job