using System;
using System.Collections.Generic;
using System.Linq;

namespace OfflineWeb
{
	public class VisitingContext
	{
		private string _rawAddress;
		private Uri _address;

		public string RawAddress
		{
			get { return _rawAddress; }
			set
			{
				_rawAddress = value;
				_address = new Uri(value, UriKind.Absolute);
			}
		}

		public Uri Address
		{
			get { return _address; }
		}

		public IWebClient WebClient { get; set; }
	}
}