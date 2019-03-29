using System;

namespace Nyxbull.Plugins.XamLoc
{
	/// <summary>
	/// The exception is thrown when an error occurs during package initializing and configuring
	/// </summary>
	internal class LocalizationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Nyxbull.Plugins.XamLoc.LocalizationException"/> class
		/// </summary>
		/// <param name="message">The error message that explains the reason of the exception</param>
		public LocalizationException(string message) : base(message) { }
	}
}