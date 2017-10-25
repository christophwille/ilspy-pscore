using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text;
using ICSharpCode.Decompiler.Extensions;
using ICSharpCode.Decompiler.TypeSystem;
using Mono.Cecil;

namespace ICSharpCode.Decompiler.PSCore
{
	[Cmdlet(VerbsCommon.Get, "DecompiledTypes")]
	[OutputType(typeof(ITypeDefinition[]))]
	public class GetDecompiledTypesCmdlet : PSCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		public ModuleDefinition Assembly { get; set; }

		[Parameter(Mandatory = true)]
		public string[] Types { get; set; }

		protected override void ProcessRecord()
		{
			HashSet<TypeKind> kinds = TypesParser.ParseSelection(Types);

			var decompiler = SimpleDecompiler.Create(Assembly);
			var result = decompiler.ListContent(kinds);

			WriteObject(result.ToArray());
		}
	}
}
