$DotnetDir = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319"
$env:Path += ";$DotNetDir"

# Build FelicaCashingSystemV2
Push-Location "FelicaDataV2"
nuget restore
Pop-Location

# Build KutDormitoryReport
Push-Location "KutDormitoryReport\KutDormitoryReport"
nuget restore
Pop-Location

# Build FelicaCashingSystemV2
Push-Location "FelicaCashingSystemV2"

nuget restore
msbuild /t:Build /p:Configuration=Debug

if (!$?) { exit 1 }

msbuild /t:Build /p:Configuration=Release

Pop-Location