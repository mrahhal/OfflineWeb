using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Konsola;

namespace OfflineWeb
{
	public class Program
	{
		private static void Main(string[] args)
		{
			var console = new Konsola.Console();
            var context = CommandLineParser.Parse<CommandLineContext>(args, console);
			if (context == null)
				return;
			try
			{
				context.Command.ExecuteCommand();
			}
			catch (ArgumentException)
			{
				console.WriteLine(WriteKind.Error, "The url should be absolute");
			}
			catch (WebException)
			{
				console.WriteLine(WriteKind.Error, "Couldn't process the page");
            }
            catch(UnauthorizedAccessException)
            {
                console.WriteLine(WriteKind.Error, "Access Denied! Either the directory is protected or you don't has the permissinon to write to it!");
                console.WriteLine(WriteKind.Info, "Try Running as administrator");
            }
		}
	}
}