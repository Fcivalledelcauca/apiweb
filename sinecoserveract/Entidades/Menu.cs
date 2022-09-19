using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class Menu
    {
        public string menu { get; set; }
        public string link { get; set; }
        public string icon { get; set; }
        public List<child> child { get; set; }
       public int order { get; set; }
    }

    public class child
    {
        public string submenu { get; set; }
        public string link { get; set; }
        public int order { get; set; }
    }


}
