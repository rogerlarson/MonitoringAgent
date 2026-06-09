param(
    [string]$ServiceName =
        "MonitoringAgentAgent"
)

Write-Host ""
Write-Host "Starting Agent Service..."
Write-Host ""

Start-Service `
    -Name $ServiceName

Get-Service `
    -Name $ServiceName

Write-Host ""
Write-Host "Agent started."
