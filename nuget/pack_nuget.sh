# Restore NuGet packages
nuget restore source/CrossLocalization/CrossLocalization.csproj

# Clean build output
msbuild source/CrossLocalization/CrossLocalization.csproj /t:Clean /p:Configuration=Release

# Build library
msbuild source/CrossLocalization/CrossLocalization.csproj /p:OutputType=Library /p:Configuration=Release

# Pack library to NuGet package
msbuild -t:pack source/CrossLocalization/CrossLocalization.csproj /p:PackageOutputPath="..\..\nuget" /p:Configuration=Release
