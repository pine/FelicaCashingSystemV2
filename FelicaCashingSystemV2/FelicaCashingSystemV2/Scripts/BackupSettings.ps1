$from = ".\"
$to = "..\..\..\FelicaCashingSystemV2_Settings\FelicaCashingSystemV2\FelicaCashingSystemV2"

Write "Start BackupSettings.ps1"

if (-Not(Test-Path -Path "$to" -Type Container)) {
	New-Item -Type Directory -Force "$to"
}

if (-Not(Test-Path -Path "$to\Properties" -Type Container)) {
	New-Item -Type Directory -Force "$to\Properties"
}

if (-Not(Test-Path -Path "$to\Resources" -Type Container)) {
	New-Item -Type Directory -Force "$to\Resources"
}

Copy-Item -Force "$from\App.config" "$to\App.config"
Copy-Item -Force "$from\MahApps.Metro.*" "$to\"
Copy-Item -Force "$from\Properties\Settings.*" "$to\Properties\"
Copy-Item -Force "$from\Resources\*.png" "$to\Resources\"
Copy-Item -Force "$from\Resources\*.ico" "$to\Resources\"
