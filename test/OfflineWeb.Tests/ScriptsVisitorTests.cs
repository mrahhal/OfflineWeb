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
			// Arrange
			var visitor = new ScriptsVisitor();
			var node = HtmlNode.CreateNode(@"<script>function some(){}</script>");

			// Act
			var newNode = await visitor.VisitAsync(null, node);

			// Assert
			Assert.Same(node, newNode);
		}

		[Fact]
		public async Task Visit_ScriptWithAbsoluteHref()
		{
			// Arrange
			var visitor = new ScriptsVisitor();
			var node = HtmlNode.CreateNode(@"<script src=""http://www.some2.com/l.js""></script>");
			var client = VisitorsHelper.CreateWebClientMock("function some(){}");
			var context = new VisitingContext()
			{
				Address = new Uri("http://www.some.com"),
				WebClient = client.Object,
			};

			// Act
			var newNode = await visitor.VisitAsync(context, node);

			// Assert
			client.Verify(c => c.DownloadAsync(new Uri("http://www.some2.com/l.js")), Times.Once);
			Assert.Equal("<script>function some(){}</script>", newNode.OuterHtml);
		}

		[Fact]
		public async Task Visit_ScriptWithRelativeHref()
		{
			// Arrange
			var visitor = new ScriptsVisitor();
			var node = HtmlNode.CreateNode(@"<script src=""l.js""></script>");
			var client = VisitorsHelper.CreateWebClientMock("function some(){}");
			var context = new VisitingContext()
			{
				Address = new Uri("http://www.some.com"),
				WebClient = client.Object,
			};

			// Act
			var newNode = await visitor.VisitAsync(context, node);

			// Assert
			client.Verify(c => c.DownloadAsync(new Uri("http://www.some.com/l.js")), Times.Once);
			Assert.Equal("<script>function some(){}</script>", newNode.OuterHtml);
		}

		[Fact]
		public async Task Visit_ScriptWithSrcStartingWithTwoSlashes_ShouldBeHandledCorrectly()
		{
			// Arrange
			var visitor = new ScriptsVisitor();
			var node = HtmlNode.CreateNode(@"<script src=""//www.some2.com/l.js""></script>");
			var client = VisitorsHelper.CreateWebClientMock("function some(){}");
			var context = new VisitingContext()
			{
				Address = new Uri("http://www.some.com"),
				WebClient = client.Object,
			};

			// Act
			var newNode = await visitor.VisitAsync(context, node);

			// Assert
			client.Verify(c => c.DownloadAsync(new Uri("http://www.some2.com/l.js")), Times.Once);
			Assert.Equal("<script>function some(){}</script>", newNode.OuterHtml);
		}
	}
}