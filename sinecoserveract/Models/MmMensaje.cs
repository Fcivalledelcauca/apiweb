using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class MmMensaje
    {
        public int IdMensaje    {get;set;}
        public string Nombre    {get;set;}
        public int IdMunicipio  {get;set;}
        public string Email     {get;set;}
        public string Telefono  {get;set;}
        public string Mensaje   { get; set; }
        public bool TC          { get; set; }
    }
}
