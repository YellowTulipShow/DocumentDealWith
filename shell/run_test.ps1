$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

dotnet run --project ./src/DocumentDealWithCommand/ -- content encode -t UTF8 --Path "D:\\Work\\YTS.ZRQ\\-ArticleNotes" --Recurse

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
