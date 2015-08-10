using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineWeb
{
	public static class AsyncExtensions
	{
		public static void Unwrap(this Task @this)
		{
			@this.GetAwaiter().GetResult();
		}

		public static T Unwrap<T>(this Task<T> @this)
		{
			return @this.GetAwaiter().GetResult();
		}
	}
}