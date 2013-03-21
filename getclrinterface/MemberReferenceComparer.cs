using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace getclrinterface
{
	public class MemberReferenceComparer : IEqualityComparer<MemberReference>
	{
		public MemberReferenceComparer ()
		{
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

				if (mrx.Parameters != mry.Parameters)
					return false;

				// FIXME: Do something less lame
				return mrx.ToString () == mry.ToString ();
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

