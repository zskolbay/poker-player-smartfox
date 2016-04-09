using System;

namespace Nancy.Simple.Logger
{
	public class LogHelper
	{
		public static void Log (String message, params object[] parameters)
		{
			Console.WriteLine (message, parameters);
		}

		public static void Error (String message, params object[] parameters)
		{
			Console.Error.WriteLine (message, parameters);
		}
	}
}

