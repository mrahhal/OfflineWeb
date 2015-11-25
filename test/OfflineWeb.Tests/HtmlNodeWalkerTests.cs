using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Xunit;

namespace OfflineWeb.Tests
{
	public class HtmlNodeWalkerTests
	{
		[Fact]
		public void Walk_WithEmptyNode_ReturnsNull()
		{
			// Arrange
			var root = HtmlNode.CreateNode("<div></div>");
			var walker = new HtmlNodeWalker(root);

			// Act
			var nd = walker.Walk();

			// Assert
			Assert.Null(nd);
		}

		[Fact]
		public void Walk_KnowsLinkNodes()
		{
			// Arrange
			var root = HtmlNode.CreateNode(@"<div><link rel=""stylesheet"">some</div>");
			var walker = new HtmlNodeWalker(root);

			// Act
			var nd = walker.Walk();

			// Assert
			Assert.NotNull(nd);
			Assert.Equal(NodeKind.Link, nd.Kind);
		}

		[Fact]
		public void Walk_KnowsScriptNodes()
		{
			// Arrange
			var root = HtmlNode.CreateNode(@"<div><script></script>some</div>");
			var walker = new HtmlNodeWalker(root);

			// Act
			var nd = walker.Walk();

			// Assert
			Assert.NotNull(nd);
			Assert.Equal(NodeKind.Script, nd.Kind);
		}

		[Fact]
		public void Walk_KnowsOtherNodes()
		{
			// Arrange
			var root = HtmlNode.CreateNode(@"<div><sometag></sometag>some</div>");
			var walker = new HtmlNodeWalker(root);

			// Act
			var nd = walker.Walk();

			// Assert
			Assert.NotNull(nd);
			Assert.Equal(NodeKind.Other, nd.Kind);
		}
	}
}