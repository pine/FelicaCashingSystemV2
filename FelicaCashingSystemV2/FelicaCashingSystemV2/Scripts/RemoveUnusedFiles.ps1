Write "Start removing unused files"

$files = @("*.pdb","*.xml","*vshost*")
$folders = @("de","es","fr","it","ko","ru","zh-*")

$files | % { Remove-Item -Force $_ }
$folders | % { Remove-Item -Force -Recurse $_ }
