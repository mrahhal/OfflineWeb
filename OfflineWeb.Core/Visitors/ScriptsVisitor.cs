﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace OfflineWeb.Visitors
{
	public class ScriptsVisitor : IVisitor
	{
		public NodeKind InterestingNodes
		{
			get
			{
				return NodeKind.Script;
			}
		}

		public async Task<HtmlNode> VisitAsync(VisitingContext context, HtmlNode node)
		{
			var src = node.GetAttributeValue("src", null);
            if (src == null)
				return node;
			var srcUri = new Uri(src, UriKind.RelativeOrAbsolute);
			if (!srcUri.IsAbsoluteUri)
			{
				srcUri = new Uri(context.Address, srcUri);
			}
			// Get the script and insert it inline.
			var content = await context.WebClient.DownloadStringAsync(srcUri);
			content = "<script>" + content + "</script>";
			return HtmlNode.CreateNode(content);
		}
	}
}