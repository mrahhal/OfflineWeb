using System;
using System.Net;
using Konsola.Parser;

namespace OfflineWeb
{
	public class Program
	{
		private static void Main(string[] args)
		{
			var console = new DefaultConsole();
			var parser = new CommandLineParser<CommandLineContext>(console);
			var result = parser.Parse(args);
			if (result.Kind == ParsingResultKind.Success)
			{
				try
				{
					result.Context.Command.ExecuteCommand();
				}
				catch (ArgumentException)
				{
					console.WriteLine(WriteKind.Error, "The url should be absolute");
				}
				catch (WebException)
				{
					console.WriteLine(WriteKind.Error, "Couldn't process the page");
				}
				catch (UnauthorizedAccessException)
				{
					console.WriteLine(WriteKind.Error, "Access denied.");
					console.WriteLine(WriteKind.Info, "Try running as administrator");
				}
			}
		}
	}
}