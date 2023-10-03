using System;

namespace TalesFromTheTable.Utilities.Exceptions
{
	public class AdventurerException : Exception
	{
		public AdventurerException() { }

		public AdventurerException(string message) : base(message) { }

		public AdventurerException(string message, Exception innerException) : base(message, innerException) { }
	}
}
