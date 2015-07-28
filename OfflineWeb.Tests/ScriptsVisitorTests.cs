using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Moq;
using OfflineWeb.Visitors;
using Xunit;

namespace OfflineWeb.Tests
{
	public class ScriptsVisitorTests
	{
		[Fact]
		public async Task Visit_ScriptWithNoSrc_ReturnsSameNode()
		{
			var visitor = new ScriptsVisitor();
			var node = HtmlNode.CreateNode(@"<script>function some(){}</script>");
			var newNode = await visitor.VisitAsync(null, node);
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_LinkWithAbsoluteHref()
		{
			var visitor = new ScriptsVisitor();
			var node = HtmlNode.CreateNode(@"<script src=""http://www.some2.com/l.js""></script>");
			var client = VisitorsHelper.CreateWebClientMock("function some(){}");
			var context = new VisitingContext()
			{
				RawAddress = "http://www.some.com",
				WebClient = client.Object,
			};
			var newNode = await visitor.VisitAsync(context, node);
			client.Verify(c => c.DownloadStringAsync("http://www.some2.com/l.js"), Times.Once);
			Assert.Equal("<script>function some(){}</script>", newNode.OuterHtml);
		}

		[Fact]
		public async Task Visit_LinkWithRelativeHref()
		{
			var visitor = new ScriptsVisitor();
			var node = HtmlNode.CreateNode(@"<script src=""l.js""></script>");
			var client = VisitorsHelper.CreateWebClientMock("function some(){}");
			var context = new VisitingContext()
			{
				RawAddress = "http://www.some.com",
				WebClient = client.Object,
			};
			var newNode = await visitor.VisitAsync(context, node);
			client.Verify(c => c.DownloadStringAsync("http://www.some.com/l.js"), Times.Once);
			Assert.Equal("<script>function some(){}</script>", newNode.OuterHtml);
		}
	}
}