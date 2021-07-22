$path = "D:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\RealTimeAutoSave"

Copy-Item "$path\About\PublishedFileId.txt" ..\About
Remove-Item -Recurse "$path\*"
mkdir $path
@(
	".",
	"1.2",
	"1.3"
) | %{
	$base = $_
	@(
		"About",
		"Assemblies",
		"Defs",
		"Languages",
		"Patches",
		"Source",
		"Textures"
	) | %{ Copy-Item -Recurse "$PSScriptRoot\$base\$_" "$path\$base\$_" }
	Remove-Item -Recurse "$path\$base\Source\bin"
	Remove-Item -Recurse "$path\$base\Source\obj"
	Remove-Item "$path\$base\Source\packages.config"
	Remove-Item "$path\$base\\About\PublishedFileId.txt"
}
