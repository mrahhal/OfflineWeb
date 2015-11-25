using System;
using System.Collections.Generic;
using System.Linq;

namespace OfflineWeb
{
	/// <summary>
	/// Represents the context of a webpage being processed.
	/// </summary>
	public class VisitingContext
	{
		/// <summary>
		/// Gets the raw address.
		/// </summary>
		public string RawAddress
		{
			get { return Address.ToString(); }
		}

		/// <summary>
		/// Gets or sets the address of the webpage being processed.
		/// </summary>
		public Uri Address { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IWebClient"/> being used.
		/// </summary>
		public IWebClient WebClient { get; set; }
	}
}