dotnet build TheRealEngine\TheRealEngine.csproj
dotnet publish TestGame\TestGame.csproj
New-Item -Path "TheRealEngine\bin\Debug\net8.0\Scenes\" -ItemType Directory -Force | Out-Null
New-Item -Path "TheRealEngine\bin\Debug\net8.0\Assemblies\" -ItemType Directory -Force | Out-Null
Copy-Item -Path "TestGame\bin\Release\net8.0\publish\Scenes\*.json" -Destination "TheRealEngine\bin\Debug\net8.0\Scenes\" -Force
Copy-Item -Path "TestGame\bin\Release\net8.0\publish\project.json" -Destination "TheRealEngine\bin\Debug\net8.0\project.json" -Force
Copy-Item -Path "TestGame\bin\Release\net8.0\publish\*.dll" -Destination "TheRealEngine\bin\Debug\net8.0\Assemblies\" -Force
