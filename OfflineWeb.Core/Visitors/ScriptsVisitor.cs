using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace OfflineWeb.Visitors
{
	/// <summary>
	/// A visitor that puts script references inline.
	/// </summary>
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

			// Take care if the src starts with two slashes.
			if (src.StartsWith("//"))
			{
				src = "http:" + src;
			}

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