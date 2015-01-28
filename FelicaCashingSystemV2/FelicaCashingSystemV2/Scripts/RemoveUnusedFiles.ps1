Write "Start removing unused files"

$files = @("*.pdb","*.xml","*vshost*")
$folders = @("de","es","fr","it","ko","ru","zh-*")

$files | % {
  if (Test-Path $_) {
    Remove-Item -Force $_
  }
}

$folders | % {
  if (Test-Path $_) {
    Remove-Item -Force -Recurse $_
  }
}