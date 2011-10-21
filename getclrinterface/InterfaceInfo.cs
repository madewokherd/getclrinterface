using System;
using System.Collections.Generic;
using Mono.Cecil;
namespace getclrinterface
{
	public class InterfaceInfo : Dictionary<AssemblyNameReference, ModuleInterfaceInfo>
	{
		public InterfaceInfo () : base(AssemblyNameReferenceComparer.comparer)
		{
		}
		
		public void AddAssemblyReference(AssemblyNameReference reference)
		{
			Add(reference, new ModuleInterfaceInfo());
		}
		
		public void ReadModuleImports (ModuleDefinition module)
		{
			foreach (AssemblyNameReference reference in module.AssemblyReferences)
			{
				AddAssemblyReference(reference);
			}
		}
		
		public void ReadAssemblyImports (AssemblyDefinition assembly)
		{
			foreach (ModuleDefinition module in assembly.Modules)
			{
				ReadModuleImports(module);
			}
		}
	}
}

