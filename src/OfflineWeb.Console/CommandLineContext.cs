﻿using System;
using System.IO;
using System.Text;
using Konsola.Parser;

namespace OfflineWeb
{
	[ContextOptions(Description = "Downloads complete webpages with resources to a single html file for offline viewing.")]
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
			var fileName = Path.GetFileNameWithoutExtension(Url) + ".html";
			string path = null;
			if (Local != null)
			{
				if (IsDirectory(Local))
				{
					path = Path.Combine(Local, fileName);
				}
				else
				{
					path = Local;
				}
			}
			else
			{
				path = Path.Combine(Environment.CurrentDirectory, fileName);
			}

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}

			var worker = new WebWorker();
			var result = worker.ProcessPageAsync(Url).Unwrap();
			File.WriteAllText(path, result, Encoding.Unicode);
		}

		private bool IsDirectory(string path)
		{
			return Path.GetExtension(path) == string.Empty;
		}
	}
}