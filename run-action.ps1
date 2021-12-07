if($IsWindows -eq $True)
{
    $filePath = Join-Path $PSScriptRoot "/Action/win-x64/MarkDigger.exe"
}

if($IsLinux -eq $True)
{
    $filePath = Join-Path $PSScriptRoot "/Action/linux-x64/MarkDigger"
    chmod +x $filePath
}

if($IsMacOS -eq $True)
{
    $filePath = Join-Path $PSScriptRoot "/Action/osx-x64/MarkDigger"
    chmod +x $filePath
}

Write-Output "Running $filePath..."
& $filePath

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}