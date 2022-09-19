using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace sinecoserveract.Funciones
{
    public class Datos
    {
        private readonly WebSIDataContext contex;
        public Datos(WebSIDataContext context)
        {

            this.contex = context;

        }

        public async Task<decimal> CostoPromedioUnitario(int iditemrow)
        {
            decimal _costoPromUni = 0;
            try
            {
               
            }
            catch (Exception)
            {
            }

            return _costoPromUni;
        }
    }
}
