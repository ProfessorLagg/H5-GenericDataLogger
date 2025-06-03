using namespace System.IO
using namespace System.Text

cls
cd $PSScriptRoot
[Environment]::CurrentDirectory = $PSScriptRoot

$colorsXamlPath = Join-Path -Path $PSScriptRoot -ChildPath "Resources\Styles\Colors.xaml"
$colorsAndroidPath = Join-Path -Path $PSScriptRoot -ChildPath "Platforms\Android\Resources\values\colors.xml"

$colorsXaml = Select-Xml -Path $colorsXamlPath -XPath "*" | Select-Object -ExpandProperty Node | Select-Object -ExpandProperty Color
$colorsAndroidStrBuilder = [StringBuilder]::new()

$colorsAndroidStrBuilder.AppendLine('<?xml version="1.0" encoding="utf-8"?>') | Out-Null
$colorsAndroidStrBuilder.AppendLine('<resources>') | Out-Null

foreach($xamlColor in $colorsXaml){ 
    $colorsAndroidStrBuilder.AppendLine("`t<color name=`"color$($xamlColor.Key)`">$($xamlColor.'#text')</color>") | Out-Null
}

$colorsAndroidStrBuilder.AppendLine('</resources>') | Out-Null
$colorsAndroidStr = $colorsAndroidStrBuilder.ToString().Trim()
$colorsAndroidBytes = [Encoding]::UTF8.GetBytes($colorsAndroidStr)
[File]::WriteAllBytes($colorsAndroidPath, $colorsAndroidBytes)

