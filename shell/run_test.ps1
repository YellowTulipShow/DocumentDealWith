$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

dotnet run --project .\src\DocumentDealWithCommand\ -- rename --root "D:\\Work\\YTS.ZRQ\\DocumentDealWith\\_test\\rename" --rule "test_#" --path "./"

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
