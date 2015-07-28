using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace OfflineWeb
{
	public class HtmlNodeWalker
	{
		private HtmlNode _root;
		private HtmlNodeWalkerMode _mode;
		private IEnumerator<HtmlNode> _nodes;
		private Dictionary<string, NodeKind> _map = new Dictionary<string, NodeKind>
		{
			{ "link", NodeKind.Link },
			{ "script", NodeKind.Script },
		};

		public HtmlNodeWalker(HtmlNode root, HtmlNodeWalkerMode mode = HtmlNodeWalkerMode.Descendants)
		{
			if (root == null)
				throw new ArgumentNullException(nameof(root));

			_root = root;
			_mode = mode;
			Initialize();
		}

		public NodeDescriptor Walk()
		{
			if (!_nodes.MoveNext())
			{
				return null;
			}
			var node = _nodes.Current;
			return new NodeDescriptor(node, GetKindFromNodeName(node.Name));
		}

		private NodeKind GetKindFromNodeName(string name)
		{
			name = name.ToLowerInvariant();
			var kind = default(NodeKind);
			if (_map.TryGetValue(name, out kind))
			{
				return kind;
			}
			return NodeKind.Other;
		}

		private void Initialize()
		{
			switch (_mode)
			{
				case HtmlNodeWalkerMode.Descendants:
					_nodes = _root.Descendants().GetEnumerator();
					break;
				case HtmlNodeWalkerMode.Children:
					_nodes = _root.ChildNodes.Nodes().GetEnumerator();
					break;
			}
		}
	}

	public enum HtmlNodeWalkerMode
	{
		Descendants,
		Children,
	}
}