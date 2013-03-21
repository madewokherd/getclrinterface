using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace getclrinterface
{
	public class TypeInterfaceInfo : Dictionary<MemberReference, MemberReference>
	{
		public TypeInterfaceInfo () : base(MemberReferenceComparer.comparer)
		{
		}
	}
}

