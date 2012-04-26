using System;
using System.Collections.Generic;
using Mono.Cecil;
namespace getclrinterface
{
	public class TypeReferenceComparer : IEqualityComparer<TypeReference>
	{
		public TypeReferenceComparer ()
		{
		}

		#region IEqualityComparer[TypeReference] implementation
		bool IEqualityComparer<TypeReference>.Equals (TypeReference x, TypeReference y)
		{
			return x.FullName == y.FullName; /* FIXME? */
		}

		int IEqualityComparer<TypeReference>.GetHashCode (TypeReference obj)
		{
			return obj.FullName.GetHashCode();
		}
		#endregion

		public static TypeReferenceComparer comparer = new TypeReferenceComparer();
	}
}

