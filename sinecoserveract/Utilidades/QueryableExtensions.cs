using sinecoserveract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Utilidades
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO) {
            return queryable
                    .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.CantidaddeRegistroPorPagina)
                    .Take(paginacionDTO.CantidaddeRegistroPorPagina);
        }
    }
}
