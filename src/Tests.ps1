Import-Module .\ICSharpCode.Decompiler.PSCore\bin\Debug\netstandard2.0\ICSharpCode.Decompiler.PSCore.dll
$decompiler = Get-Decompiler .\ICSharpCode.Decompiler.PSCore\bin\Debug\netstandard2.0\ICSharpCode.Decompiler.PSCore.dll

Get-DecompiledSource $decompiler -TypeName ICSharpCode.Decompiler.PSCore.GetDecompilerCmdlet

$classes = Get-DecompiledTypes $decompiler -Types class
$classes.Count

foreach ($c in $classes)
{
    Write-Output $c.FullName
}

Get-DecompiledProject $decompiler -OutputPath .\decomptest