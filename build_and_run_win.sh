rm -rf build/win/
mkdir -p build/win/

"F:\PortablePrograms\Godot\godot4_console.exe"  --path godot --export-release "Windows Desktop"

"build/win/Mosh Pint.exe" --path "build/win"