#nullable disable
using System;

namespace MangaDexSharp.Internal.Dto
{
    internal class ErrorDto
    {
        public string Context { get; set; }
        public string Detail { get; set; }
        public string Id { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
    }
}
