using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Extensions;
using ICSharpCode.Decompiler.TypeSystem;

namespace Demo.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var asm = SimpleAssemblyLoader.LoadModule("ICSharpCode.Decompiler.Extensions.dll");
			var dc = SimpleDecompiler.Create(asm);

			var tds = dc.ListContent(new HashSet<TypeKind>() { TypeKind.Class });
			ITypeDefinition c = tds.FirstOrDefault(t =>
				String.Equals(t.FullName, "ICSharpCode.Decompiler.Extensions.CustomAssemblyResolver", StringComparison.Ordinal));

			StringWriter sw = new StringWriter();
			dc.Decompile(sw, c.FullName);
			Console.WriteLine(sw.ToString());
			Console.WriteLine("-------------");

			IProperty prop = c.Properties.First();
			sw = new StringWriter();
			dc.Decompile(sw, prop);
			Console.WriteLine(sw.ToString());
			Console.WriteLine("-------------");

			Console.ReadKey();
		}
	}
}
