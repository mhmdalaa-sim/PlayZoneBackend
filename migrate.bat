@echo off
echo PlayZone Database Migration Script
echo ===================================

REM Check if dotnet-ef is installed
where dotnet-ef >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo dotnet-ef is not installed. Installing...
    dotnet tool install --global dotnet-ef
)

REM Create migration
echo Creating initial migration...
dotnet ef migrations add InitialCreate

REM Apply migration
echo Applying migrations to database...
dotnet ef database update

echo Migration completed successfully!
pause
