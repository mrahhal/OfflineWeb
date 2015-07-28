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
	public class StylesVisitorTests
	{
		[Fact]
		public async Task Visit_LinkWithNoRel_ReturnsSameNode()
		{
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""somewhere"">");
			var newNode = await visitor.VisitAsync(null, node);
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_LinkWithNonStyleSheetRel_ReturnsSameNode()
		{
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""somewhere"" rel=""some-rel"">");
			var newNode = await visitor.VisitAsync(null, node);
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_LinkWithNoHref_ReturnsSameNode()
		{
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link rel=""stylesheet"">");
			var newNode = await visitor.VisitAsync(null, node);
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_LinkWithAbsoluteHref()
		{
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""http://www.some2.com/l.css"" rel=""stylesheet"">");
			var client = CreateWebClientMock();
            var context = new VisitingContext()
			{
				RawAddress = "http://www.some.com",
				WebClient = client.Object,
			};
			var newNode = await visitor.VisitAsync(context, node);
			client.Verify(c => c.DownloadStringAsync("http://www.some2.com/l.css"), Times.Once);
			Assert.Equal("<style>html{width:0}</style>", newNode.OuterHtml);
		}

		[Fact]
		public async Task Visit_LinkWithRelativeHref()
		{
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""l.css"" rel=""stylesheet"">");
			var client = CreateWebClientMock();
			var context = new VisitingContext()
			{
				RawAddress = "http://www.some.com",
				WebClient = client.Object,
			};
			var newNode = await visitor.VisitAsync(context, node);
			client.Verify(c => c.DownloadStringAsync("http://www.some.com/l.css"), Times.Once);
			Assert.Equal("<style>html{width:0}</style>", newNode.OuterHtml);
		}

		private Mock<IWebClient> CreateWebClientMock(string returns = null)
		{
			var client = new Mock<IWebClient>();
			client
				.Setup(c => c.DownloadStringAsync(It.IsAny<string>()))
				.ReturnsAsync(returns ?? "html{width:0}");
			return client;
		}
	}
}