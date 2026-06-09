# ============================================================================
# Uninstall-Service.ps1
# ============================================================================
#
# Removes the Monitoring Agent Engine Windows Service.
#
# ============================================================================

$serviceName =
    "MonitoringAgentEngine"

Write-Host ""
Write-Host "Removing Monitoring Agent Engine..."
Write-Host ""

$service =
    Get-Service `
        -Name $serviceName `
        -ErrorAction SilentlyContinue

if ($service)
{
    if ($service.Status -ne "Stopped")
    {
        Stop-Service `
            -Name $serviceName `
            -Force
    }

    sc.exe delete $serviceName
}

Write-Host ""
Write-Host "Service removed."
Write-Host ""