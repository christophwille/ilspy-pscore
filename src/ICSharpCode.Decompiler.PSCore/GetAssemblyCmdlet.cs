using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using ICSharpCode.Decompiler.Extensions;
using Mono.Cecil;

namespace ICSharpCode.Decompiler.PSCore
{
	[Cmdlet(VerbsCommon.Get, "Assembly")]
	[OutputType(typeof(ModuleDefinition))]
	public class GetAssemblyCmdlet : PSCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		[Alias("PSPath")]
		[ValidateNotNullOrEmpty]
		public string LiteralPath { get; set; }

		protected override void ProcessRecord()
		{
			string path = GetUnresolvedProviderPathFromPSPath(LiteralPath);
			ModuleDefinition retval = SimpleAssemblyLoader.LoadModule(path);
			WriteObject(retval);
		}
	}
}
