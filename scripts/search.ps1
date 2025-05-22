#!pwsh

param (
    [string]$Pattern
)

if (-not $Pattern) {
    Write-Output "Error: Please provide a search pattern as an argument."
    exit 1
}

$CurrWorkDir = Get-Location
$ScriptRootDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $ScriptRootDir -ErrorAction Stop
Set-Location ..

Get-ChildItem -Path . -Recurse -Filter "*.cs" | ForEach-Object {
    $file = $_
    Write-Output $file.FullName
    Get-Content $file.FullName | Select-String -Pattern $Pattern
}

Set-Location $CurrWorkDir -ErrorAction Stop
