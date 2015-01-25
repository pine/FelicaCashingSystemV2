$to = ".\"
$from = "..\..\..\FelicaCashingSystemV2_Settings\FelicaCashingSystemV2\FelicaCashingSystemV2"

Write "Start RestoreSettings.ps1"

if (-Not(Test-Path -Path "$to\App.config")) {
	Write "Restore App.config"
	Copy-Item "$from\App.config" "$to\App.config"
}

if (-Not(Test-Path -Path "$to\MahApps.Metro.dll")) {
	Write "Restore MashApps.Metro.*"
	Copy-Item -Force "$from\MahApps.Metro.*" "$to\"
}

if (-Not(Test-Path -Path "$to\Properties\Settings.settings")) {
	Write "Restore Properties\Settings.*"
	Copy-Item -Force "$from\Properties\Settings.*" "$to\Properties\"
}

if (-Not(Test-Path -Path "$to\Resources\FelicaImage.png")) {
	Write "Restore Resources\*.png"
	Copy-Item -Force "$from\Resources\*.png" "$to\Resources\"
}

if (-Not(Test-Path -Path "$to\Resources\FelicaIcon.ico")) {
	Write "Restore Resources\*.ico"
	Copy-Item -Force "$from\Resources\*.ico" "$to\Resources\"
}
