using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace OfflineWeb
{
	public class NodeDescriptor
	{
		public NodeDescriptor(HtmlNode node, NodeKind kind)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));

			Node = node;
			Kind = kind;
		}

		public HtmlNode Node { get; }

		public NodeKind Kind { get; }
	}
}