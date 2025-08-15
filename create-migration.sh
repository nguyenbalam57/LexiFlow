#!/bin/bash
# LexiFlow Migration Script for .NET 9

echo "?? LexiFlow Migration Creator"
echo "============================="

# Check if in correct directory
if [ ! -f "LexiFlow.sln" ]; then
    echo "? Please run this script from the solution root directory"
    exit 1
fi

# Check EF Core Tools
echo "?? Checking EF Core Tools..."
if ! dotnet tool list -g | grep -q "dotnet-ef"; then
    echo "??  Installing EF Core Tools..."
    dotnet tool install --global dotnet-ef
fi

# Function to create migration
create_migration() {
    local name=$1
    local description=$2
    
    echo "?? Creating migration: $name"
    cd LexiFlow.API
    
    dotnet ef migrations add "$name" \
        --project ../LexiFlow.Infrastructure \
        --startup-project . \
        --context LexiFlowContext \
        --verbose
    
    if [ $? -eq 0 ]; then
        echo "? Migration created successfully!"
        
        read -p "Update database now? (y/n): " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Yy]$ ]]; then
            dotnet ef database update \
                --project ../LexiFlow.Infrastructure \
                --startup-project . \
                --context LexiFlowContext
        fi
    else
        echo "? Failed to create migration"
    fi
    
    cd ..
}

# Menu
echo ""
echo "Select migration type:"
echo "1. Initial Create"
echo "2. Performance Indexes"
echo "3. Custom Migration"
echo "4. Update Database"
echo "5. Show Migration History"
echo "6. Generate SQL Script"

read -p "Enter choice (1-6): " choice

case $choice in
    1)
        create_migration "InitialCreate" "Initial database schema"
        ;;
    2)
        create_migration "AddPerformanceIndexes" "Performance optimization indexes"
        ;;
    3)
        read -p "Enter migration name: " name
        create_migration "$name" "Custom migration"
        ;;
    4)
        echo "?? Updating database..."
        cd LexiFlow.API
        dotnet ef database update \
            --project ../LexiFlow.Infrastructure \
            --startup-project . \
            --context LexiFlowContext
        cd ..
        ;;
    5)
        echo "?? Migration History:"
        cd LexiFlow.API
        dotnet ef migrations list \
            --project ../LexiFlow.Infrastructure \
            --startup-project . \
            --context LexiFlowContext
        cd ..
        ;;
    6)
        echo "?? Generating SQL script..."
        cd LexiFlow.API
        dotnet ef migrations script \
            --project ../LexiFlow.Infrastructure \
            --startup-project . \
            --context LexiFlowContext \
            --output ../lexiflow-migration.sql
        cd ..
        echo "? SQL script generated: lexiflow-migration.sql"
        ;;
    *)
        echo "? Invalid choice"
        ;;
esac

echo "?? Migration script completed!"