﻿using System;
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
				0 == String.Compare(t.FullName, "ICSharpCode.Decompiler.Extensions.CustomAssemblyResolver", StringComparison.OrdinalIgnoreCase));

			StringWriter sw = new StringWriter();
			dc.Decompile(sw, c.FullName);
			Console.WriteLine(sw.ToString());

			sw = new StringWriter();
			var csharpDC = dc.InitializeDecompiler();

			// Via ICSharpCode.Decompiler.TypeSystem
			IMethod method = c.Methods.Last();
			var memberRef = dc.TypeSystem.GetCecil(method);
			SyntaxTree st = csharpDC.Decompile(memberRef.Resolve());
			var visitor = new CSharpOutputVisitor(sw, FormattingOptionsFactory.CreateSharpDevelop());
			st.AcceptVisitor(visitor);
			Console.WriteLine(sw.ToString());


			Console.ReadKey();
		}
	}
}
