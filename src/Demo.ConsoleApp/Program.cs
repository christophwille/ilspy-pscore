using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

            Console.WriteLine(tds.Count());
        }
    }
}
