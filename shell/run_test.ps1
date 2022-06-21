$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

dotnet run --project ./src/DocumentDealWithCommand/ -- content encode -t UTF8 -p ./ -r

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
