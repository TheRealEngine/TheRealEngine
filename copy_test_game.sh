#!/bin/bash
dotnet build TheRealEngine/TheRealEngine.csproj
dotnet publish TestGame/TestGame.csproj
mkdir -p TheRealEngine/bin/Debug/net8.0/Scenes/
mkdir -p TheRealEngine/bin/Debug/net8.0/Assemblies/
cp TestGame/bin/Release/net8.0/publish/Scenes/*.json TheRealEngine/bin/Debug/net8.0/Scenes/
cp TestGame/bin/Release/net8.0/publish/project.json TheRealEngine/bin/Debug/net8.0/project.json
cp TestGame/bin/Release/net8.0/publish/*.dll TheRealEngine/bin/Debug/net8.0/Assemblies/
