Import-Module .\ICSharpCode.Decompiler.PSCore\bin\Debug\netstandard2.0\ICSharpCode.Decompiler.PSCore.dll
$asm = Get-Assembly .\ICSharpCode.Decompiler.PSCore\bin\Debug\netstandard2.0\ICSharpCode.Decompiler.Extensions.dll

Get-DecompiledSource $asm -TypeName ICSharpCode.Decompiler.Extensions.CustomAssemblyResolver

$classes = Get-DecompiledTypes $asm -Types class
$classes.Count

foreach ($c in $classes)
{
    Write-Output $c.FullName
}

Get-DecompiledProject $asm -OutputPath .\decomptest