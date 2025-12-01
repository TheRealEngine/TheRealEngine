$platform = "win-x64"

$game_output_dir = "TestGame/bin/Release/net8.0/publish"
$engine_build_dir = "TheRealEngine/bin/Debug/net8.0"

Write-Host "Cleaning previous builds..."
Remove-Item -Recurse -Force $engine_build_dir -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force $game_output_dir -ErrorAction SilentlyContinue

Write-Host "Building projects..."
dotnet build TheRealEngine/TheRealEngine.csproj
dotnet publish TestGame/TestGame.csproj

Write-Host "Setting up engine build directory."
New-Item -ItemType Directory -Force -Path "$engine_build_dir/Scenes/" | Out-Null
Write-Host -NoNewline "."
New-Item -ItemType Directory -Force -Path "$engine_build_dir/Assemblies/native" | Out-Null
Write-Host "."

Write-Host -NoNewline "Copying game files to engine build directory"
Copy-Item "$game_output_dir/project.json" "$engine_build_dir/project.json" -Force
Write-Host -NoNewline "."
Copy-Item "$game_output_dir/*.dll" "$engine_build_dir/Assemblies/" -Force
Write-Host -NoNewline "."
Copy-Item "TestGame/Assets" "$engine_build_dir/Assets" -Recurse -Force
Write-Host "."

Write-Host "Copying native runtimes..."
$natives_dir = "$game_output_dir/runtimes/$platform/native"
if (Test-Path $natives_dir) {
    Copy-Item "$natives_dir/*" "$engine_build_dir/Assemblies/" -Force
}
