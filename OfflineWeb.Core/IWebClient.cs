using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OfflineWeb
{
	public interface IWebClient
	{
		Task<string> DownloadStringAsync(string address);
	}

	public static class WebClientExtensions
	{
		public static Task<string> DownloadStringAsync(this IWebClient @this, Uri address)
		{
			return @this.DownloadStringAsync(address.ToString());
		}
	}

	public class WebClientAdapter : IWebClient
	{
		private WebClient _webClient = new WebClient();

		public Task<string> DownloadStringAsync(string address)
		{
			return _webClient.DownloadStringTaskAsync(address);
		}
	}
}