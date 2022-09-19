using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int CantidaddeRegistroPorPagina
        {
            get => cantidadRegistrosPorPagina;
            set {
                cantidadRegistrosPorPagina = (value > cantidadMaximaRegistrosPorPagina) ? cantidadMaximaRegistrosPorPagina : value;
            }
        }
    }
}
