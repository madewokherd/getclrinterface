using System;
using System.Collections.Generic;
using Mono.Cecil;
using System.Text;


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

		private static string MemberToString(MemberReference member) {
			if (member is MethodReference)
			{
				MethodReference method = member as MethodReference;
				StringBuilder result = new StringBuilder();
				if (!method.HasThis)
					result.Append("static ");
				result.Append(method.ReturnType);
				result.Append(" ");
				result.Append(method.Name);
				if (method.HasGenericParameters)
				{
					result.Append("<!!0");
					for (int i=1; i<method.GenericParameters.Count; i++)
					{
						result.Append(",!!");
						result.Append(i);
					}
					result.Append(">");
				}
				result.Append("(");
				bool comma = false;
				foreach (var param in method.Parameters)
				{
					if (comma)
						result.Append(",");
					result.Append(param.ParameterType);
					if (!string.IsNullOrWhiteSpace(param.Name))
					{
						result.Append(" ");
						result.Append(param.Name);
					}
					comma = true;
				}
				result.Append(")");
				return result.ToString();
			}
			else
				return member.ToString();
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
					Console.Write(MemberToString(mvp.Value));
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

