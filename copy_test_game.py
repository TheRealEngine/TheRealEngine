import os
import shutil
import subprocess
import sys

# Platform detection
if sys.platform.startswith("win"):
    platform = "win-x64"
elif sys.platform.startswith("darwin"):
    platform = "osx-x64"
elif sys.platform.startswith("linux"):
    platform = "linux-x64"
else:
    print("Unsupported platform:", sys.platform)
    sys.exit(1)

game_output_dir = os.path.join("TestGame", "bin", "Release", "net8.0", "publish")
engine_build_dir = os.path.join("TheRealEngine", "bin", "Debug", "net8.0")

def rmrf(path):
    if os.path.isdir(path):
        shutil.rmtree(path)
    elif os.path.exists(path):
        os.remove(path)

print("Cleaning previous builds...")
rmrf(engine_build_dir)
rmrf(game_output_dir)

print("Building projects...")
subprocess.run(["dotnet", "build", os.path.join("TheRealEngine", "TheRealEngine.csproj")], check=True)
subprocess.run(["dotnet", "publish", os.path.join("TestGame", "TestGame.csproj")], check=True)

print("Setting up engine build directory.")
os.makedirs(os.path.join(engine_build_dir, "Scenes"), exist_ok=True)
print(".", end="", flush=True)
os.makedirs(os.path.join(engine_build_dir, "Assemblies", "native"), exist_ok=True)
print(".")

print("Copying game files to engine build directory", end="")
# project.json
src_project_json = os.path.join(game_output_dir, "project.json")
dst_project_json = os.path.join(engine_build_dir, "project.json")
if os.path.exists(src_project_json):
    shutil.copy2(src_project_json, dst_project_json)
print(".", end="", flush=True)
# *.dll files
dll_src = game_output_dir
dll_dst = os.path.join(engine_build_dir, "Assemblies")
if os.path.isdir(dll_src):
    for f in os.listdir(dll_src):
        if f.lower().endswith(".dll"):
            shutil.copy2(os.path.join(dll_src, f), dll_dst)
print(".", end="", flush=True)
# Assets directory
testgame_assets = os.path.join("TestGame", "Assets")
assets_dst = os.path.join(engine_build_dir, "Assets")
if os.path.exists(testgame_assets):
    if os.path.exists(assets_dst):
        shutil.rmtree(assets_dst)
    shutil.copytree(testgame_assets, assets_dst)
print(".")

print("Copying native runtimes...")
natives_dir = os.path.join(game_output_dir, "runtimes", platform, "native")
if os.path.isdir(natives_dir):
    for f in os.listdir(natives_dir):
        src_file = os.path.join(natives_dir, f)
        if os.path.isfile(src_file):
            shutil.copy2(src_file, dll_dst)

print("Done.")