dotnet --version

dotnet restore Lyra.Core

$binfo = "$($env:LIB_VERSION)$(".")$($env:APPVEYOR_BUILD_NUMBER)"

Write-Host "Assembly version info" $binfo

dotnet build -c Release Lyra.Core /p:Version=$binfo

dotnet pack Lyra.Core -c Release --no-build --include-symbols /p:Version=$env:APPVEYOR_BUILD_VERSION