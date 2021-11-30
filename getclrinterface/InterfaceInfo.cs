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
			if (!ContainsKey(reference))
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
			foreach (MemberReference member in module.GetMemberReferences())
			{
				TypeReference typeref = member.DeclaringType;
				if (typeref.IsGenericInstance)
					typeref = typeref.GetElementType();
				IMetadataScope scope = typeref.Scope;
				if (scope.MetadataScopeType != MetadataScopeType.AssemblyNameReference)
					continue;
				AssemblyNameReference reference = AssemblyNameReferenceFromToken(module, typeref.Scope.MetadataToken);
				if (!this[reference].ContainsKey (typeref))
					this[reference].AddTypeReference(typeref);
				if (!(member is TypeReference))
				{
					var memberrefs = this[reference][typeref];
					if (!memberrefs.ContainsKey(member))
						memberrefs.Add (member, member);
				}
			}
			foreach (ExportedType t in module.ExportedTypes)
			{
				if (t.IsForwarder)
				{
					AssemblyNameReference reference = AssemblyNameReferenceFromToken(module, t.Scope.MetadataToken);
					var typeref = new TypeReference(t.Namespace, t.Name, module, t.Scope);
					if (!this[reference].ContainsKey (typeref))
						this[reference].AddTypeReference(typeref);
				}
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

