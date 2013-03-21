using System;
using System.Collections.Generic;
using Mono.Cecil;
namespace getclrinterface
{
	public class AssemblyInterfaceInfo : Dictionary<TypeReference, TypeInterfaceInfo>
	{
		public AssemblyInterfaceInfo () : base(TypeReferenceComparer.comparer)
		{
		}

		public void AddTypeReference (TypeReference typeref)
		{
			Add(typeref, new TypeInterfaceInfo());
		}

		public void PrintInterface ()
		{
			foreach (KeyValuePair<TypeReference, TypeInterfaceInfo> kvp in this)
			{
				Console.Write("    ");
				Console.Write(kvp.Key.FullName);
				Console.WriteLine();
				foreach (KeyValuePair<MemberReference, MemberReference> mvp in kvp.Value)
				{
					Console.Write("        ");
					Console.Write(mvp.Value.ToString());
					Console.WriteLine();
				}
			}
		}
		
		private static bool IsPublicTypeDefinition (TypeDefinition typedef)
		{
			if (typedef.IsNested && !IsPublicTypeDefinition(typedef.DeclaringType))
				return false;
			if (typedef.IsPublic || typedef.IsNestedFamily || typedef.IsNestedFamilyOrAssembly ||
			    typedef.IsNestedPublic)
				return true;
			return false;
		}
		
		public void ReadModuleExports (ModuleDefinition module)
		{
			foreach (TypeDefinition typedef in module.Types)
			{
				if (!IsPublicTypeDefinition(typedef))
					continue;
				AddTypeReference(typedef);
			}
		}
	}
}

