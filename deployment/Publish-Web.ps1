param(
[string]$OutputPath = ".\Publish\Web"
)

Write-Host "Installing dependencies..."

npm install

Write-Host "Building React application..."

npm run build

if (Test-Path $OutputPath)
{
Remove-Item $OutputPath -Recurse -Force
}

Copy-Item `    "..\MonitoringAgent.Web\dist"`
$OutputPath `
-Recurse

Write-Host "Web publish complete."
