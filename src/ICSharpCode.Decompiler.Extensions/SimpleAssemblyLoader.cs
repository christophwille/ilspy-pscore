using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ICSharpCode.Decompiler.Extensions
{
	public class SimpleAssemblyLoader
	{
		public static ModuleDefinition LoadModule(string assemblyFileName)
		{
			var resolver = new CustomAssemblyResolver(assemblyFileName);

			var module = ModuleDefinition.ReadModule(assemblyFileName, new ReaderParameters {
				AssemblyResolver = resolver,
				InMemory = true
			});

			resolver.TargetFramework = module.Assembly.DetectTargetFrameworkId();

			return module;
		}
	}
}
