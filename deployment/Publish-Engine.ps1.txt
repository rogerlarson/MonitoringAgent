param(
[string]$Configuration = "Release",
[string]$OutputPath = ".\Publish\Engine"
)

Write-Host "Publishing MonitoringAgent.Engine..."

dotnet publish `    ..\MonitoringAgent.Engine`
-c $Configuration `
-o $OutputPath

Write-Host "Engine publish complete."
