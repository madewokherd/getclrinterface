using System;
using System.Collections.Generic;
using Mono.Cecil;
namespace getclrinterface
{
	public class InterfaceInfo : Dictionary<AssemblyNameReference, AssemblyInterfaceInfo>
	{
		public InterfaceInfo () : base(AssemblyNameReferenceComparer.comparer)
		{
		}
		
		public void AddAssemblyReference(AssemblyNameReference reference)
		{
			Add(reference, new AssemblyInterfaceInfo());
		}
		
		public void PrintInterface ()
		{
			foreach (KeyValuePair<AssemblyNameReference, AssemblyInterfaceInfo> kvp in this)
			{
				System.Console.WriteLine(kvp.Key.ToString());
				kvp.Value.PrintInterface();
			}
		}
		
		internal static AssemblyNameReference AssemblyNameReferenceFromToken (ModuleDefinition module, MetadataToken token)
		{
			foreach (AssemblyNameReference asmref in module.AssemblyReferences)
			{
				if (asmref.MetadataToken == token)
					return asmref;
			}
			return null;
		}
		
		public void ReadModuleImports (ModuleDefinition module)
		{
			foreach (AssemblyNameReference reference in module.AssemblyReferences)
			{
				AddAssemblyReference(reference);
			}
			foreach (TypeReference typeref in module.GetTypeReferences())
			{
				IMetadataScope scope = typeref.Scope;
				if (scope.MetadataScopeType != MetadataScopeType.AssemblyNameReference)
					continue;
				AssemblyNameReference reference = AssemblyNameReferenceFromToken(module, typeref.Scope.MetadataToken);
				this[reference].AddTypeReference(typeref);
			}
		}
		
		public void ReadAssemblyImports (AssemblyDefinition assembly)
		{
			foreach (ModuleDefinition module in assembly.Modules)
			{
				ReadModuleImports(module);
			}
		}
		
		public void ReadAssemblyExports (AssemblyDefinition assembly)
		{
			AddAssemblyReference(assembly.Name);
			foreach (ModuleDefinition module in assembly.Modules)
			{
				this[assembly.Name].ReadModuleExports(module);
			}
		}
	}
}

