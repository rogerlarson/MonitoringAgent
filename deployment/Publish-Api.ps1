param(
[string]$Configuration = "Release",
[string]$OutputPath = ".\Publish\Api"
)

Write-Host "Publishing MonitoringAgent.Api..."

dotnet publish `    ..\MonitoringAgent.Api`
-c $Configuration `
-o $OutputPath

Write-Host "API publish complete."
