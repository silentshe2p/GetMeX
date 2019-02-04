using System;

namespace GetMeX.ViewModels.Exceptions
{
	public class InsufficientDataException : Exception
	{
		public InsufficientDataException()
            : base("Insufficient input data") { }

		public InsufficientDataException(string message)
			: base(message) { }
	}
}
