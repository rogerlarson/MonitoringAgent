param(
    [string]$ServiceName =
        "MonitoringAgentAgent"
)

Write-Host ""
Write-Host "Stopping Agent Service..."
Write-Host ""

Stop-Service `
    -Name $ServiceName `
    -Force

Get-Service `
    -Name $ServiceName

Write-Host ""
Write-Host "Agent stopped."
