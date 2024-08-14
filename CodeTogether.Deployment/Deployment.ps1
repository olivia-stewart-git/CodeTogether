param (
    [string]$WorkingDir
)

Write-Host "Setting the location to the working directory..."
Set-Location $WorkingDir
Write-Host "Location set to: $WorkingDir"

try 
{
    Write-Host "Restoring dotnet..."
    dotnet restore
    dotnet tool restore
    Write-Host "Dotnet restored successfully"

	Write-Host "Dropping the database..."
    dotnet ef database drop -f --project CodeTogether.Data
    Write-Host "Database dropped successfully"

    Write-Host "Updating the database..."
    dotnet ef database update --project CodeTogether.Data
    Write-Host "Database updated successfully"

    Write-Host "The script has successfully completed!"
}
catch {
    Write-Host "An error occurred: $($_.Exception)"
    exit 1
}