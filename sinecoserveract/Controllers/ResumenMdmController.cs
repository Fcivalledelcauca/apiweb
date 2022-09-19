using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sinecoserveract.Entidades;
using sinecoserveract.Models;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/resumenmdm")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ResumenMdmController : ControllerBase
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;

        public ResumenMdmController(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: TcBaseIdi
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return new JsonResult(await _context.TcResumenMdms.ToListAsync());
        }

        // GET: TcBaseIdi/Details/5
        [HttpGet("{lote}")]
        public async Task<ActionResult> Details(string lote)
        {
            if (lote == null)
            {
                return NotFound();
            }

            var resumenMdm= await _context.TcResumenMdms.Where(m => m.Lote == lote).ToListAsync();
            if (resumenMdm == null)
            {
                return NotFound();
            }
            return new JsonResult(resumenMdm);

        }

        // POST: TcBaseIdi/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<ResumenMdmDisplay> resumenmdm)
        {
            var existe = await this._context.TcResumenMdms.AnyAsync(t => t.Lote == resumenmdm[0].Lote);
            if (existe)
            {
                return new JsonResult("Ya existe el Lote.");
            }
            else
            {
                var mapResumen = mapper.Map<List<TcResumenMdm>>(resumenmdm);
                _context.AddRange(mapResumen);
                await _context.SaveChangesAsync();
                return new JsonResult(mapResumen);
            }
        }

        // GET: TcBaseIdi/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resumenMdm = await _context.TcResumenMdms.Where(m => m.IdTablero == id).ToListAsync();
            if (resumenMdm == null)
            {
                return NotFound();
            }
            else
            {
                this._context.RemoveRange(resumenMdm);
                await this._context.SaveChangesAsync();
                return new JsonResult("El Registro se ha borrado con Éxito");
            }
        }


    }
}
