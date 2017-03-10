dotnet --version

dotnet restore Lyra.Core

$binfo = "$($env:LIB_VERSION)$(".")$($env:APPVEYOR_BUILD_NUMBER)"

Write-Host "Assembly version info" $binfo

dotnet build Lyra.Core /p:Version=$binfo

dotnet pack Lyra.Core --include-symbols /p:Version=$env:APPVEYOR_BUILD_VERSION