using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using Mono.Cecil;

namespace Demo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // https://github.com/icsharpcode/ILSpy/wiki/Getting-Started-With-ICSharpCode.Decompiler
            //
            var decompiler = new CSharpDecompiler("Demo.ConsoleApp.exe", new DecompilerSettings());

            var name = new FullTypeName("Demo.ConsoleApp.Test+NestedClassTest");
            Console.WriteLine(decompiler.DecompileTypeAsString(name));

            Console.WriteLine("-------------");

            ITypeDefinition typeInfo = decompiler.TypeSystem.Compilation.FindType(name).GetDefinition();
            IMemberDefinition cecilProperty = decompiler.TypeSystem.GetCecil(typeInfo.Properties.First()).Resolve();
            Console.WriteLine(decompiler.DecompileAsString(cecilProperty));

            Console.ReadKey();
        }
    }

    public class Test
    {
        public class NestedClassTest
        {

        }
    }
}
