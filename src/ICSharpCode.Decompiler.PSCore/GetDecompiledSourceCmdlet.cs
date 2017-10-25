using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using ICSharpCode.Decompiler.Extensions;
using Mono.Cecil;

namespace ICSharpCode.Decompiler.PSCore
{
	[Cmdlet(VerbsCommon.Get, "DecompiledSource")]
	[OutputType(typeof(string))]
	public class GetDecompiledSourceCmdlet : PSCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		public ModuleDefinition Assembly { get; set; }

		[Parameter]
		public string TypeName { get; set; } = string.Empty;

		protected override void ProcessRecord()
		{
			var decompiler = SimpleDecompiler.Create(Assembly);
			var sw = new StringWriter();
			decompiler.Decompile(sw, TypeName);

			WriteObject(sw.ToString());
		}
	}
}
