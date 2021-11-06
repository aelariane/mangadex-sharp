using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDexSharp.Exceptions
{
    public class ForbiddenException : MangaDexException
    {
        internal ForbiddenException(Error error)
            : base(403, error)
        {

        }
    }
}
