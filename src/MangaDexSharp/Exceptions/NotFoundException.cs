using System;

namespace MangaDexSharp.Exceptions
{
    public class NotFoundException : MangaDexException
    {
        internal NotFoundException(Error error)
            : base(404, error)
        {
        }
    }
}
