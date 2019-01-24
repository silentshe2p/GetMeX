using System;

namespace GetMeX.ViewModels.Exceptions
{
    public class NoResultException : Exception
    {
        public NoResultException()
            : base("No result found :(") { }

        public NoResultException(string message)
            : base(message) { }
    }
}
