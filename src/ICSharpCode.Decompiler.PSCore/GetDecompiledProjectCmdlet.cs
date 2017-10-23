using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using ICSharpCode.Decompiler.Extensions;
using Mono.Cecil;

namespace ICSharpCode.Decompiler.PSCore
{
    [Cmdlet(VerbsCommon.Get, "DecompiledProject")]
    [OutputType(typeof(string))]
    public class GetDecompiledProjectCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public ModuleDefinition Assembly { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        [Alias("PSPath", "OutputPath")]
        [ValidateNotNullOrEmpty]
        public string LiteralPath { get; set; }

        protected override void ProcessRecord()
        {
            string path = GetUnresolvedProviderPathFromPSPath(LiteralPath);
            if (!Directory.Exists(path))
            {
                WriteObject("Destination directory must exist prior to decompilation");
                return;
            }

            var decompiler = new SimpleDecompiler(Assembly);
            decompiler.DecompileAsProject(path);

            WriteObject("Decompilation finished");
        }
    }
}
