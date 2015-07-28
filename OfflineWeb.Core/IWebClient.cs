using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OfflineWeb
{
	public interface IWebClient
	{
		Task<string> DownloadAsync(string address);
	}

	public static class WebClientExtensions
	{
		public static Task<string> DownloadAsync(this IWebClient @this, Uri address)
		{
			return @this.DownloadAsync(address.ToString());
		}
	}

	public class WebClientAdapter : IWebClient
	{
		private WebClient _webClient = new WebClient();

		public Task<string> DownloadAsync(string address)
		{
			return _webClient.DownloadStringTaskAsync(address);
		}
	}
}