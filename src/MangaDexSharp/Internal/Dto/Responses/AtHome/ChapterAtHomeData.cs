#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDexSharp.Internal.Dto.Responses.AtHome
{
    internal class ChapterAtHomeData
    {
        public IEnumerable<string> Data { get; set; }
        public IEnumerable<string> DataSaver { get; set; }
        public string Hash { get; set; }
    }
}
