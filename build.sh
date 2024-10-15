dotnet build -p:Configuration=Release
if [[ -d "Releases" ]]; then
    rm Releases/SettingsAPI.dll
else
    mkdir Releases
fi
cd bin/Release/net48
cp SettingsAPI.dll ../../../Releases
cd ..
cd ..
cd ..