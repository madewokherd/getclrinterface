using System;
using System.Collections.Generic;
using Mono.Cecil;
namespace getclrinterface
{
	public class AssemblyNameReferenceComparer : IEqualityComparer<AssemblyNameReference>
	{
		public AssemblyNameReferenceComparer ()
		{
		}

		#region IEqualityComparer[AssemblyNameReference] implementation
		bool IEqualityComparer<AssemblyNameReference>.Equals (AssemblyNameReference x, AssemblyNameReference y)
		{
			return x.FullName == y.FullName; /* FIXME? */
		}

		int IEqualityComparer<AssemblyNameReference>.GetHashCode (AssemblyNameReference obj)
		{
			return obj.FullName.GetHashCode();
		}
		#endregion

		public static AssemblyNameReferenceComparer comparer = new AssemblyNameReferenceComparer();
	}
}

