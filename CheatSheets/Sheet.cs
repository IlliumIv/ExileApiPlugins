using CheatSheets.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets
{
    public class Sheet
    {
        public string Name { get; protected set; }
        public List<string> Preloads { get; protected set; }
        public bool AllowIconDrawing { get; set; } = false;
    }
}
