Write "Start RestoreSettings.ps1"

Write "  Backup FelicaCashingSystemV2"
$to = ".\"
$from = "..\..\..\FelicaCashingSystemV2_Settings\FelicaCashingSystemV2\FelicaCashingSystemV2"

if (-Not(Test-Path -Path "$to\App.config")) {
	Write "Restore App.config"
	Copy-Item "$from\App.config" "$to\App.config"
}

if (-Not(Test-Path -Path "$to\MahApps.Metro.dll")) {
	Write "Restore MahApps.Metro.*"
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

if (-Not(Test-Path -Path "$to" -Type Container)) {
	New-Item -Type Directory -Force "$to"
}

Write "  Restore KutDormitoryReport"
$from = "..\..\..\FelicaCashingSystemV2_Settings\KutDormitoryReport\KutDormitoryReport\KutDormitoryReport"
$to = ".\..\..\KutDormitoryReport\KutDormitoryReport\KutDormitoryReport"

if (-Not(Test-Path -Path "$to\ipaexg.ttf" -Type Container)) {
	Copy-Item -Force "$from\*.ttf" "$to\"
}

