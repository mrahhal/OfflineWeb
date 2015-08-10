using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Konsola;
using Konsola.Attributes;

namespace OfflineWeb
{
	[ContextOptions(Description = "Downloads complete webpages with resources to a single html file for offline viewing.", ExitOnException = true)]
	[DefaultCommand(typeof(DefaultCommand))]
	public class CommandLineContext : ContextBase
	{
	}

	public class DefaultCommand : CommandBase<CommandLineContext>
	{
		[Parameter("url", IsMandatory = true)]
		public string Url { get; set; }

		[Parameter("local")]
		public string Local { get; set; }

		public override void ExecuteCommand()
		{
			Local = Local ?? Path.GetFileNameWithoutExtension(Url);
			var path = Path.Combine(Environment.CurrentDirectory, Local);
			var worker = new WebWorker();
			var result = worker.ProcessPageAsync(Url).Unwrap();
			File.WriteAllText(path, result);
		}
	}
}