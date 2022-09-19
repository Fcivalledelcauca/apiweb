using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class ItemConsulta
    {
        public int IdItem { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCorta { get; set; }
        public int? IdTipoInventario { get; set; }
        public int? IdUnidadMedida { get; set; }
        public int? IdImptoVenta { get; set; }
        public int? IdImptoCompra { get; set; }
        public bool? ManejaInv { get; set; }
        public bool? PrecioLibre { get; set; }
        public bool? CantidadLibre { get; set; }
        public decimal? PrecioInicial { get; set; }
        public decimal? PrecioFinal { get; set; }
        public decimal? PrecioBase { get; set; }
        public string EspecificacioTecnica { get; set; }
        public byte[] Foto { get; set; }
        public DateTime? Fc { get; set; }
        public bool? Activo { get; set; }
        public int? IdCia { get; set; }
        public string IdCo { get; set; }



        public decimal? Costo { get; set; }


    }
}
