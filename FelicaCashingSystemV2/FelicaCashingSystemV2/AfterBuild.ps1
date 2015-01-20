$to = "..\..\..\\FelicaCashingSystemV2_Settings\\FelicaCashingSystemV2\\FelicaCashingSystemV2"

Write "Start AfterBuild.ps1"

Push-Location '..\..\..\'

New-Item -Type Directory -Force "FelicaCashingSystemV2_Settings\\FelicaCashingSystemV2\\FelicaCashingSystemV2"
New-Item -Type Directory -Force "FelicaCashingSystemV2_Settings\\FelicaCashingSystemV2\\FelicaCashingSystemV2\\Properties"
New-Item -Type Directory -Force "FelicaCashingSystemV2_Settings\\FelicaCashingSystemV2\\FelicaCashingSystemV2\\Resources"

Pop-Location

Copy-Item -Force "App.config" "$to\\App.config"
Copy-Item -Force "MahApps.Metro.*" "$to\\"
Copy-Item -Force "Properties\\Settings.*" "$to\\Properties\\"
Copy-Item -Force "Resources\\*.png" "$to\\Resources\\"
Copy-Item -Force "Resources\\*.ico" "$to\\Resources\\"
