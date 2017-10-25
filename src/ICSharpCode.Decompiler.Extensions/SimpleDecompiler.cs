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
    /// <summary>
    /// SimpleDecompiler only supports CSharpDecompiler
    /// </summary>
    public class SimpleDecompiler
    {
        private readonly ModuleDefinition _module;
        private readonly DecompilerTypeSystem _typeSystem;

        private SimpleDecompiler()
        {
        }
        private SimpleDecompiler(ModuleDefinition module, DecompilerTypeSystem typeSystem)
        {
            _module = module;
            _typeSystem = typeSystem;
        }

        public ModuleDefinition ModuleDefinition => _module;
        public DecompilerTypeSystem TypeSystem => _typeSystem;

        public static SimpleDecompiler Create(ModuleDefinition module)
        {
            var typeSystem = new DecompilerTypeSystem(module);
            return new SimpleDecompiler(module, typeSystem);
        }

        public void ListContent(TextWriter output, ISet<TypeKind> kinds)
        {
            foreach (ITypeDefinition type in ListContent(kinds)) {
                output.WriteLine($"{type.Kind} {type.FullName}");
            }
        }

        public IEnumerable<ITypeDefinition> ListContent(ISet<TypeKind> kinds)
        {
            foreach (ITypeDefinition type in ListContent()) {
                if (!kinds.Contains(type.Kind))
                    continue;
                yield return type;
            }
        }

        public IEnumerable<ITypeDefinition> ListContent()
        {
            return _typeSystem.MainAssembly.GetAllTypeDefinitions();
        }

        public void DecompileAsProject(string outputDirectory)
        {
            WholeProjectDecompiler decompiler = new WholeProjectDecompiler();
            decompiler.DecompileProject(_module, outputDirectory);
        }

        public CSharpDecompiler InitializeDecompiler()
        {
            return new CSharpDecompiler(_typeSystem, new DecompilerSettings());
        }

        public void Decompile(TextWriter output, string typeName = null, bool throwOnTypeNotFound = false)
        {
            CSharpDecompiler decompiler = InitializeDecompiler();

            decompiler.AstTransforms.Add(new EscapeInvalidIdentifiers());
            SyntaxTree syntaxTree;

            if (typeName == null) {
                syntaxTree = decompiler.DecompileWholeModuleAsSingleFile();
            } else {
                var typeToDecompile = _module.GetTypes()
                    .Where(td => string.Equals(td.FullName, typeName, StringComparison.OrdinalIgnoreCase))
                    .ToList(); // we need multiple enumeration (Any() + DecompileTypes)

                if (!typeToDecompile.Any() && throwOnTypeNotFound) {
                    throw new ArgumentException($"Type {typeName} not found, cannot decompile");
                }

                syntaxTree = decompiler.DecompileTypes(typeToDecompile);
            }

            var visitor = new CSharpOutputVisitor(output, FormattingOptionsFactory.CreateSharpDevelop());
            syntaxTree.AcceptVisitor(visitor);
        }
    }
}
