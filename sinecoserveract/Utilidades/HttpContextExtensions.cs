using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task<string> InsertarParametrosPaginacion<T>(this HttpContext httpContext, IQueryable<T> queryable, int cantidadRegistrosporPagina) {
            double cantidad = await queryable.CountAsync();
            double cantidadPaginas = Math.Ceiling(cantidad / cantidadRegistrosporPagina);
            httpContext.Response.Headers.Add("cantidadPaginas", cantidadPaginas.ToString());
            return cantidadPaginas.ToString();
        }
    }
}
