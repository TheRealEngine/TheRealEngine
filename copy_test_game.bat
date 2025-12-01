@echo off
setlocal

set "platform=win-x64"

set "game_output_dir=TestGame\bin\Release\net8.0\publish"
set "engine_build_dir=TheRealEngine\bin\Debug\net8.0"

echo Cleaning previous builds...
if exist "%engine_build_dir%" rmdir /S /Q "%engine_build_dir%"
if exist "%game_output_dir%" rmdir /S /Q "%game_output_dir%"

echo Building projects...
dotnet build TheRealEngine\TheRealEngine.csproj
dotnet publish TestGame\TestGame.csproj

echo Setting up engine build directory.
mkdir "%engine_build_dir%\Scenes"
<nul set /p =.
mkdir "%engine_build_dir%\Assemblies\native"
echo.

<nul set /p ="Copying game files to engine build directory"
if exist "%game_output_dir%\project.json" copy /Y "%game_output_dir%\project.json" "%engine_build_dir%\project.json" >nul
<nul set /p =.
copy /Y "%game_output_dir%\*.dll" "%engine_build_dir%\Assemblies\" >nul
<nul set /p =.
if exist "TestGame\Assets" xcopy /E /I /Y "TestGame\Assets" "%engine_build_dir%\Assets" >nul
echo.

echo Copying native runtimes...
set "natives_dir=%game_output_dir%\runtimes\%platform%\native"
if exist "%natives_dir%" (
    copy /Y "%natives_dir%\*" "%engine_build_dir%\Assemblies\" >nul
)

endlocal