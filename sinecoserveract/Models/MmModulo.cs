using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class MmModulo
    {
        public int IdModulo { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Descripcion { get; set; }
        public int? Orden { get; set; }
    }
}
