using System;

namespace MangaDexSharp.Exceptions
{
    public class BadRequestException : MangaDexException
    {
        internal BadRequestException(Error error)
            : base(400, error)
        {

        }
    }
}
