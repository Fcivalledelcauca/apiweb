namespace sinecoserveract.Entidades
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;

    public class mc_Items
    {
     public int IdItem { get; set; }
     public string Referencia { get; set; }
     public string Descripcion { get; set; }

     public string DescripcionCorta { get; set; }

     public int IdTipoInventario { get; set; }
     public int IdUnidadMedida { get; set; }
     public int IdImptoVenta { get; set; }
     public int IdImptoCompra { get; set; }

     public bool ManejaInv { get; set; }
     public bool PrecioLibre { get; set; }

     public bool CantidadLibre { get; set; }

     public decimal PrecioInicial { get; set; }

     public decimal PrecioFinal { get; set; }
     
     public string EspecificacioTecnica { get;set;}

     public byte[] Foto { get; set; }
     public Nullable<System.DateTime> Fc { get; set; }
     public Nullable<bool> Activo { get; set; }
     public Nullable<decimal> PrecioBase { get; set; }
     public Nullable<int> IdCia { get; set; }
     public string IdCO { get; set; }
    
    }
}