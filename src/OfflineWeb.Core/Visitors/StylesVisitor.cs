using System;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace OfflineWeb.Visitors
{
	/// <summary>
	/// A visitor that puts style references inline.
	/// </summary>
	public class StylesVisitor : IVisitor
	{
		public NodeKind InterestingNodes
		{
			get
			{
				return NodeKind.Link;
			}
		}

		public async Task<HtmlNode> VisitAsync(VisitingContext context, HtmlNode node)
		{
			// We're only interested in stylesheets.
			if (node.GetAttributeValue("rel", null) != "stylesheet")
				return node;
			var href = node.GetAttributeValue("href", null);
			if (href == null)
				return node;
			var hrefUri = new Uri(href, UriKind.RelativeOrAbsolute);
			if (!hrefUri.IsAbsoluteUri)
			{
				hrefUri = new Uri(context.Address, hrefUri);
			}

			// Get the stylesheet and insert it inline.
			var content = default(string);
			try
			{
				content = await context.WebClient.DownloadAsync(hrefUri);
			}
			catch (WebException)
			{
				return node;
			}
			content = "<style>" + content + "</style>";
			return HtmlNode.CreateNode(content);
		}
	}
}