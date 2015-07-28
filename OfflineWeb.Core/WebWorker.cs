using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OfflineWeb.Visitors;

namespace OfflineWeb
{
	/// <summary>
	/// Downloads a webpage and performs a predefined set of operations using <see cref="IVisitor"/>s on it.
	/// </summary>
	public class WebWorker
	{
		private IWebClient _webClient;
		private IList<IVisitor> _visitors;

		//--------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the <see cref="IWebClient"/> used.
		/// </summary>
		public IWebClient WebClient
		{
			get
			{
				return _webClient ?? (_webClient = new WebClientAdapter());
			}
			set { _webClient = value; }
		}

		/// <summary>
		/// Gets the visitors that will be used when processing the webpages.
		/// </summary>
		public IList<IVisitor> Visitors
		{
			get
			{
				return _visitors ?? (_visitors = CreateDefaultVisitors());
			}
		}

		public Task<string> ProcessPageAsync(string address)
		{
			if (address == null)
				throw new ArgumentNullException(nameof(address));

			return ProcessPageAsync(new Uri(address, UriKind.RelativeOrAbsolute));
		}

		public Task<string> ProcessPageAsync(Uri address)
		{
			if (address == null)
				throw new ArgumentNullException(nameof(address));
			if (!address.IsAbsoluteUri)
				throw new ArgumentException("The address should be absolute.", nameof(address));

			return InternalProcessPageAsync(address);
		}

		private async Task<string> InternalProcessPageAsync(Uri address)
		{
			// Download the webpage.
			var content = await WebClient.DownloadStringAsync(address);

			// Create the document.
			var document = new HtmlDocument();
			document.LoadHtml(content);

			// Create the context.
			var context = new VisitingContext()
			{
				RawAddress = address.ToString(),
				WebClient = WebClient,
			};

			// Walk the tree.
			var walker = new HtmlNodeWalker(document.DocumentNode);
			var nd = default(NodeDescriptor);
			while ((nd = walker.Walk()) != null)
			{
				foreach (var visitor in Visitors)
				{
					if (!visitor.InterestingNodes.Has(nd.Kind))
						continue;
					var newNode = await visitor.VisitAsync(context, nd.Node);
					if (newNode != nd.Node)
					{
						if (newNode == null)
						{
							nd.Node.Remove();
						}
						else
						{
							nd.Node.ParentNode.ReplaceChild(newNode, nd.Node);
						}
					}
				}
			}

			// Save and return the final transformed webpage.
			var sb = new StringBuilder();
			document.Save(new StringWriter(sb));
			return sb.ToString();
		}

		//--------------------------------------------------------------------

		private IList<IVisitor> CreateDefaultVisitors()
		{
			return new List<IVisitor>
			{
				new StylesVisitor(),
				new ScriptsVisitor(),
			};
		}
	}
}