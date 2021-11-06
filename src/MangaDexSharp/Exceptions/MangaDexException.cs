using System;

namespace MangaDexSharp.Exceptions
{
    public class MangaDexException : Exception
    {
        public int Code { get; }
        public Error Information { get; }

        internal MangaDexException(int code, Error error)
            : base(error.Detail)
        {
            Code = code;
            Information = error;
        }

        internal MangaDexException(int code, Error error, string message)
            : base(message)
        {
            Code = code;
            Information = error;
        }
    }
}
