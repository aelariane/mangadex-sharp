using System;

namespace MangaDexSharp.Exceptions
{
    public class UnauthorizedException : Exception
    {
        internal UnauthorizedException() : base("Requested action requires to be logged in.")
        {
        }
    }
}
