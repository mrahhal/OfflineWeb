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
			// Arrange
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""somewhere"">");

			// Act
			var newNode = await visitor.VisitAsync(null, node);

			// Assert
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_LinkWithNonStyleSheetRel_ReturnsSameNode()
		{
			// Arrange
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""somewhere"" rel=""some-rel"">");

			// Act
			var newNode = await visitor.VisitAsync(null, node);

			// Assert
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_LinkWithNoHref_ReturnsSameNode()
		{
			// Arrange
			var visitor = new StylesVisitor();

			// Act
			var node = HtmlNode.CreateNode(@"<link rel=""stylesheet"">");
			var newNode = await visitor.VisitAsync(null, node);

			// Assert
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_LinkWithAbsoluteHref()
		{
			// Arrange
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""http://www.some2.com/l.css"" rel=""stylesheet"">");
			var client = VisitorsHelper.CreateWebClientMock("html{width:0}");
			var context = new VisitingContext()
			{
				Address = new Uri("http://www.some.com"),
				WebClient = client.Object,
			};

			// Act
			var newNode = await visitor.VisitAsync(context, node);

			// Assert
			client.Verify(c => c.DownloadAsync(new Uri("http://www.some2.com/l.css")), Times.Once);
			Assert.Equal("<style>html{width:0}</style>", newNode.OuterHtml);
		}

		[Fact]
		public async Task Visit_LinkWithRelativeHref()
		{
			// Arrange
			var visitor = new StylesVisitor();
			var node = HtmlNode.CreateNode(@"<link href=""l.css"" rel=""stylesheet"">");
			var client = VisitorsHelper.CreateWebClientMock("html{width:0}");
			var context = new VisitingContext()
			{
				Address = new Uri("http://www.some.com"),
				WebClient = client.Object,
			};

			// Act
			var newNode = await visitor.VisitAsync(context, node);

			// Assert
			client.Verify(c => c.DownloadAsync(new Uri("http://www.some.com/l.css")), Times.Once);
			Assert.Equal("<style>html{width:0}</style>", newNode.OuterHtml);
		}
	}
}