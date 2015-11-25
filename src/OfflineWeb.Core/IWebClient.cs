using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OfflineWeb
{
	public interface IWebClient
	{
		Task<string> DownloadAsync(Uri address);
	}

	public static class WebClientExtensions
	{
		public static Task<string> DownloadAsync(this IWebClient @this, string address)
		{
			return @this.DownloadAsync(new Uri(address));
		}
	}

	public class WebClientAdapter : IWebClient
	{
		private WebClient _webClient = new WebClient();

		public Task<string> DownloadAsync(Uri address)
		{
			return _webClient.DownloadStringTaskAsync(address);
		}
	}
}