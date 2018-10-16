using System;

namespace GetMeX.ViewModels.Exceptions
{
	class InsufficientDataException : Exception
	{
		public InsufficientDataException() { }

		public InsufficientDataException(string message)
			: base(message) { }
	}
}
