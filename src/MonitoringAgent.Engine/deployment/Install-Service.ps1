# ============================================================================
# Install-Service.ps1
# ============================================================================
#
# Installs the Monitoring Agent Engine as a Windows Service.
#
# ============================================================================

$serviceName =
    "MonitoringAgentEngine"

$displayName =
    "Monitoring Agent Engine"

$exePath =
    Join-Path `
        $PSScriptRoot `
        "MonitoringAgent.Engine.exe"

Write-Host ""
Write-Host "Installing Monitoring Agent Engine..."
Write-Host ""

if (!(Test-Path $exePath))
{
    throw "Executable not found: $exePath"
}

if (Get-Service `
        -Name $serviceName `
        -ErrorAction SilentlyContinue)
{
    Write-Host "Service already exists."
    exit
}

New-Service `
    -Name $serviceName `
    -DisplayName $displayName `
    -BinaryPathName $exePath `
    -StartupType Automatic

Start-Service `
    -Name $serviceName

sc.exe failure `
    $serviceName `
    reset= 86400 `
    actions= restart/60000/restart/300000/restart/900000

Write-Host ""
Write-Host "Service installed successfully."
Write-Host ""