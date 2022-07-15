$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

dotnet run --project .\src\DocumentDealWithCommand\ -- rename --rule "#" --path "D:\\Work\\YTS.IMG\\Bing\\2021\\12"

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
