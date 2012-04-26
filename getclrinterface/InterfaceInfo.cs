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

