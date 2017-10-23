Import-Module .\ICSharpCode.Decompiler.PSCore\bin\Debug\net461\ICSharpCode.Decompiler.PSCore.dll
$asm = Get-Assembly .\ICSharpCode.Decompiler.PSCore\bin\Debug\net461\ICSharpCode.Decompiler.Extensions.dll

Get-DecompiledSource $asm -TypeName ICSharpCode.Decompiler.Extensions.CustomAssemblyResolver

Get-DecompiledTypes $asm -Types interface,class