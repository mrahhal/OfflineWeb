using System;
using System.Collections.Generic;
using System.Linq;

namespace OfflineWeb
{
	[Flags]
	public enum NodeKind
	{
		None   = 0x0,
		Link   = 0x1 << 0,
		Script = 0x1 << 1,
		Other  = 0x1 << 2,

		All    = Link | Script | Other,
	}

	internal static class NodeKindExtensions
	{
		public static bool Has(this NodeKind @this, NodeKind nodeKind)
		{
			return (@this & nodeKind) == nodeKind;
		}
	}
}