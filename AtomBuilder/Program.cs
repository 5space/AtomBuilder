#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using AppKit;
using Foundation;
#endregion

namespace AtomBuilder
{
	static class Program
	{
		private static void Main(string[] args)
		{
			AtomBuilderGame atomBuilderGame = new AtomBuilderGame();
			try
			{
				atomBuilderGame.Window.AllowUserResizing = true;
				atomBuilderGame.Run();
			}
			finally
			{
				atomBuilderGame.Dispose();
			}
		}
	}
}
