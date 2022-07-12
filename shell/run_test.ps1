$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

dotnet run --project .\src\DocumentDealWithCommand\ -- rename --rule "test_#" --path "D:\\Work\\YTS.ZRQ\\DocumentDealWith\\_test\\rename"

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
