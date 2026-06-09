# ============================================================================
# Publish.ps1
# ============================================================================

dotnet publish `
    "..\src\MonitoringAgent.Engine\MonitoringAgent.Engine.csproj" `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -o ".\publish"