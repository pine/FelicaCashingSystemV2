$DotnetDir = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319"
$env:Path += ";$DotNetDir"

# Build FelicaCashingSystemV2
Push-Location "FelicaDataV2"
nuget restore
Pop-Location

# Build KutDormitoryReport
Push-Location "KutDormitoryReport"
nuget restore
Pop-Location

# Build FelicaCashingSystemV2
Push-Location "FelicaCashingSystemV2"

Push-Location "FelicaCashingSystemV2"
powershell -NoProfile -ExecutionPolicy Unrestricted -File "Scripts\RestoreSettings.ps1"
Pop-Location

nuget restore
msbuild

Pop-Location