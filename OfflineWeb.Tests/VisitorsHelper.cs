using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace OfflineWeb.Tests
{
	public static class VisitorsHelper
	{
		public static Mock<IWebClient> CreateWebClientMock(string returns)
		{
			var client = new Mock<IWebClient>();
			client
				.Setup(c => c.DownloadStringAsync(It.IsAny<string>()))
				.ReturnsAsync(returns);
			return client;
		}
	}
}