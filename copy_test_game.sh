#!/bin/bash

platform="linux-x64"

game_output_dir="TestGame/bin/Release/net8.0/publish"
engine_build_dir="TheRealEngine/bin/Debug/net8.0"

echo Cleaning previous builds...
rm -rf $engine_build_dir
rm -rf $game_output_dir

echo Building projects...
dotnet build TheRealEngine/TheRealEngine.csproj
dotnet publish TestGame/TestGame.csproj

echo Setting up engine build directory...
mkdir -p $engine_build_dir/Scenes/
mkdir -p $engine_build_dir/Assemblies/native

echo Copying game files to engine build directory...
cp $game_output_dir/Scenes/*.json $engine_build_dir/Scenes/
cp $game_output_dir/project.json $engine_build_dir/project.json
cp $game_output_dir/*.dll $engine_build_dir/Assemblies/

echo Copying native runtimes...
natives_dir="$game_output_dir/runtimes/$platform/native"
[ -d "$natives_dir" ] && cp $natives_dir/* $engine_build_dir/Assemblies/ || true

echo Copying game assets...
cp -r TestGame/Assets $engine_build_dir/Assets

