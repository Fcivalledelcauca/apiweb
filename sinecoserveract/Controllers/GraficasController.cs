using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/graficas")]
    public class GraficasController : ControllerBase
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;
        public GraficasController(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet("{pbase}/{codigoDane}/{grupo}")]
        public async Task<ActionResult>  bases(string pbase,string codigodane="",string grupo="")
        {
            try
            {
                var lote = await this._context.TcTableros.OrderByDescending(t => t.IdTablero).FirstOrDefaultAsync();
                if (lote != null)
                {

                    if (pbase == "baseidf")
                    {
                        var baseidfMunicipio = await this._context.TcBaseIdfs.Where(i => i.CodigoDane == codigodane && i.IdTablero == lote.IdTablero).FirstOrDefaultAsync();
                        if (baseidfMunicipio != null)
                        {
                            return new JsonResult(baseidfMunicipio);
                        }

                        return new JsonResult("No se encontró municipio");
                    }
                    else if (pbase == "baseidfdos")
                    {
                        var baseidfMunicipio = await this._context.TcBaseIdf2s.Where(i => i.Tipo == grupo && i.IdTablero == lote.IdTablero).ToListAsync();
                        if (baseidfMunicipio != null)
                        {
                            return new JsonResult(baseidfMunicipio);
                        }

                        return new JsonResult("No se encontró municipio");
                    }
                    else if (pbase == "basemdm")
                    {
                        var basemdm = await this._context.TcBaseMdms.Where(i => i.CodigoDane == codigodane && i.IdTablero == lote.IdTablero).ToListAsync();
                        if (basemdm != null)
                        {
                            return new JsonResult(basemdm);
                        }

                        return new JsonResult("No se encontró municipio");

                    }
                    else if (pbase == "resumenmdm")
                    {
                        var resumenmdm = await this._context.TcResumenMdms.Where(i => i.Grupo == grupo && i.IdTablero == lote.IdTablero).ToListAsync();
                        if (resumenmdm != null)
                        {
                            return new JsonResult(resumenmdm);
                        }

                        return new JsonResult("No se encontró municipio");

                    }
                    else if (pbase == "baseidi")
                    {
                        var baseidi = await this._context.TcBaseIdis.Where(i => i.CodigoDane == codigodane && i.IdTablero == lote.IdTablero).ToListAsync();
                        if (baseidi != null)
                        {
                            return new JsonResult(baseidi);
                        }

                        return new JsonResult("No se encontró municipio");

                    }
                    else if (pbase == "promediodimension")
                    {
                        var pd = await this._context.TcPromedioDimensiones.Where(i => i.Tipo == codigodane && i.IdTablero == lote.IdTablero).ToListAsync();
                        if (pd != null)
                        {
                            return new JsonResult(pd);
                        }

                        return new JsonResult("No se encontró municipio");

                    }
                    else
                    {
                        return new JsonResult("No se encontró una base para extraer la información");
                    }
                }
                else {
                    return new JsonResult("No se encontró ningun lote");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error en baseidf: " + ex.InnerException);
            }
        }

       
    }
}
