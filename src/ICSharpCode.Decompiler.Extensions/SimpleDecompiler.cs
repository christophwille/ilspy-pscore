using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.CSharp.Transforms;
using ICSharpCode.Decompiler.TypeSystem;
using Mono.Cecil;

namespace ICSharpCode.Decompiler.Extensions
{
	public class SimpleDecompiler
	{
		private readonly ModuleDefinition _module;

		public SimpleDecompiler(ModuleDefinition module)
		{
			_module = module;
		}

		public void ListContent(TextWriter output, ISet<TypeKind> kinds)
		{
			var typeSystem = new DecompilerTypeSystem(_module);

			foreach (var type in typeSystem.MainAssembly.GetAllTypeDefinitions()) {
				if (!kinds.Contains(type.Kind))
					continue;
				output.WriteLine($"{type.Kind} {type.FullName}");
			}
		}

		public void DecompileAsProject(string outputDirectory)
		{
			WholeProjectDecompiler decompiler = new WholeProjectDecompiler();
			decompiler.DecompileProject(_module, outputDirectory);
		}

		public void Decompile(TextWriter output, string typeName = null)
		{
			var typeSystem = new DecompilerTypeSystem(_module);
			CSharpDecompiler decompiler = new CSharpDecompiler(typeSystem, new DecompilerSettings());

			decompiler.AstTransforms.Add(new EscapeInvalidIdentifiers());
			SyntaxTree syntaxTree;
			if (typeName == null)
				syntaxTree = decompiler.DecompileWholeModuleAsSingleFile();
			else
				syntaxTree = decompiler.DecompileTypes(_module.GetTypes().Where(td => string.Equals(td.FullName, typeName, StringComparison.OrdinalIgnoreCase)));

			var visitor = new CSharpOutputVisitor(output, FormattingOptionsFactory.CreateSharpDevelop());
			syntaxTree.AcceptVisitor(visitor);
		}
	}
}
