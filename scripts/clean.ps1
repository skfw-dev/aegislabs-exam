#!pwsh

$CurrWorkDir = Get-Location
$ScriptRootDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $ScriptRootDir -ErrorAction Stop
Set-Location ..

$FILES = Get-ChildItem -Recurse -File | Where-Object { $_.Name -match '\~$' }

foreach ($FILE in $FILES) {
    Write-Output $FILE.FullName
    Remove-Item -Path $FILE.FullName -Force
}

Set-Location $CurrWorkDir -ErrorAction Stop
