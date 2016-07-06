using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace getclrinterface
{
	public class MemberReferenceComparer : IEqualityComparer<MemberReference>
	{
		private IEqualityComparer<TypeReference> typerefcmp;

		public MemberReferenceComparer ()
		{
			typerefcmp = new TypeReferenceComparer();
		}

		#region IEqualityComparer implementation
		public bool Equals (MemberReference x, MemberReference y)
		{
			if (x.Name != y.Name)
				return false;

			if (x is MemberReference && y is MethodReference)
			{
				MethodReference mrx = (MethodReference)x;
				MethodReference mry = (MethodReference)y;

				if (mrx.HasThis != mry.HasThis)
					return false;

				if (mrx.GenericParameters.Count != mry.GenericParameters.Count)
					return false;

				if (mrx.Parameters.Count != mry.Parameters.Count)
					return false;

				if (!typerefcmp.Equals(mrx.ReturnType, mry.ReturnType))
					return false;

				var arx = mrx.Parameters.ToArray();
				var ary = mry.Parameters.ToArray();
				for (int i = 0; i<mrx.Parameters.Count; i++)
				{
					if (!typerefcmp.Equals(arx[i].ParameterType, ary[i].ParameterType))
						return false;
				}
				return true;
			}

			if (x is EventReference && y is EventReference)
				return true;

			if (x is FieldReference && y is FieldReference)
				return true;

			return false;
		}

		public int GetHashCode (MemberReference obj)
		{
			return obj.Name.GetHashCode ();
		}
		#endregion

		public static readonly MemberReferenceComparer comparer = new MemberReferenceComparer ();
	}
}

