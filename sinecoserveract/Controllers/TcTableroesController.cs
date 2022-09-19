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
    [Route("/api/tablero")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TcTableroesController : ControllerBase
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;

        public TcTableroesController(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: TcTableroes
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return new JsonResult(await _context.TcTableros.ToListAsync());
        }

        // GET: TcTableroes/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcTablero = await _context.TcTableros
                .FirstOrDefaultAsync(m => m.IdTablero == id);
            if (tcTablero == null)
            {
                return NotFound();
            }

            return new JsonResult(tcTablero);
        }

        // GET: TcTableroes/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaberoDisplay Tablero)
        {
            if (ModelState.IsValid)
            {
                var existe =await this._context.TcTableros.AnyAsync(t => t.Lote == Tablero.Lote);
                if (existe)
                {
                    return new JsonResult("Ya existe el Lote.");
                }
                else { 
                    var mapTablero = mapper.Map<TcTablero>(Tablero);
                    this._context.Add(mapTablero);
                    await this._context.SaveChangesAsync();
                    return new JsonResult(await _context.TcTableros.OrderByDescending(t=>t.IdTablero).FirstOrDefaultAsync());
                }
            }
            else
            {
                return BadRequest("El Modelo no es valido.");
            }
            
        }
      
       // Delete: TcTableroes/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tcTablero = await _context.TcTableros.FindAsync(id);
            var tcbasemdm = await _context.TcBaseMdms.Where(m => m.IdTablero == id).ToListAsync();
            var tcbaseidf = await _context.TcBaseIdfs.Where(m => m.IdTablero == id).ToListAsync();
            var tcbaseidfdos = await _context.TcBaseIdf2s.Where(m => m.IdTablero == id).ToListAsync();
            var tcbaseidi = await _context.TcBaseIdis.Where(m => m.IdTablero == id).ToListAsync();
            var tcResumenMdm = await _context.TcResumenMdms.Where(m => m.IdTablero == id).ToListAsync();
            var tcPromedioDim = await _context.TcPromedioDimensiones.Where(m => m.IdTablero == id).ToListAsync();

            _context.RemoveRange(tcbasemdm);
            _context.RemoveRange(tcbaseidf);
            _context.RemoveRange(tcbaseidfdos);
            _context.RemoveRange(tcbaseidi);
            _context.RemoveRange(tcResumenMdm);
            _context.RemoveRange(tcPromedioDim);

            _context.TcTableros.Remove(tcTablero);
            await _context.SaveChangesAsync();
            return new JsonResult("El Registro se ha borrado con Éxito");
        }
    }
}
