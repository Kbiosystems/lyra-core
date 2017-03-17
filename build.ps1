dotnet --version

dotnet restore LyraElectronics.Core

$binfo = "$($env:LIB_VERSION)$(".")$($env:APPVEYOR_BUILD_NUMBER)"

Write-Host "Assembly version info" $binfo

dotnet build -c Release LyraElectronics.Core /p:Version=$binfo

dotnet pack LyraElectronics.Core -c Release --no-build --include-symbols /p:Version=$env:APPVEYOR_BUILD_VERSION