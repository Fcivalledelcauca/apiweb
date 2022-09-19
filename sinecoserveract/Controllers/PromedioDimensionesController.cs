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
    [Route("/api/promediodim")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PromedioDimensionesController : ControllerBase
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;

        public PromedioDimensionesController(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: TcBaseIdi
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return new JsonResult(await _context.TcPromedioDimensiones.ToListAsync());
        }

        // GET: TcBaseIdi/Details/5
        [HttpGet("{lote}")]
        public async Task<ActionResult> Details(string lote)
        {
            if (lote == null)
            {
                return NotFound();
            }

            var promedioDimensiones = await _context.TcPromedioDimensiones.Where(m => m.Lote == lote).ToListAsync();
            if (promedioDimensiones == null)
            {
                return NotFound();
            }
            return new JsonResult(promedioDimensiones);

        }

        // POST: TcBaseIdi/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<PromedioDimDisplay> promedioDims)
        {
            var existe = await this._context.TcPromedioDimensiones.AnyAsync(t => t.Lote == promedioDims[0].Lote);
            if (existe)
            {
                return new JsonResult("Ya existe el Lote.");
            }
            else
            {
                var mapPromedio = mapper.Map<List<TcPromedioDimensione>>(promedioDims);
                _context.AddRange(mapPromedio);
                await _context.SaveChangesAsync();
                return new JsonResult(mapPromedio);
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

            var resumenMdm = await _context.TcPromedioDimensiones.Where(m => m.IdTablero == id).ToListAsync();
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
