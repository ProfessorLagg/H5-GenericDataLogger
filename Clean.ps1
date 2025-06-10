cd $PSScriptRoot
[Environment]::CurrentDirectory = $PSScriptRoot
$csproj_items = Get-ChildItem -Recurse -Filter "*.csproj"

$project_dirs = $csproj_items | Select-Object -ExpandProperty Directory

$remove_dirs = @()
foreach($dir in $project_dirs){
    $remove_dirs += $project_dirs[0].GetDirectories() | ?{($_.Name -ieq 'bin') -or ($_.Name -ieq 'obj')}
}
$remove_dirs = $remove_dirs | Select -ExpandProperty FullName | Sort-Object -Unique | Get-Item
$remove_dirs | %{([System.IO.DirectoryInfo]$_).Delete($true); Write-Host "Deleted $($_.FullName)"}