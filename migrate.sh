#!/bin/bash

echo "PlayZone Database Migration Script"
echo "==================================="

# Check if dotnet-ef is installed
if ! command -v dotnet-ef &> /dev/null
then
    echo "dotnet-ef is not installed. Installing..."
    dotnet tool install --global dotnet-ef
fi

# Create migration
echo "Creating initial migration..."
dotnet ef migrations add InitialCreate

# Apply migration
echo "Applying migrations to database..."
dotnet ef database update

echo "Migration completed successfully!"
