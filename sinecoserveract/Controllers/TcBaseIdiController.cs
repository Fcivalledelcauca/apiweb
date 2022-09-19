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
    [Route("/api/baseidi")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TcBaseIdiController : ControllerBase
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;

        public TcBaseIdiController(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: TcBaseIdi
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return new JsonResult(await _context.TcBaseIdis.ToListAsync());
        }

        // GET: TcBaseIdi/Details/5
        [HttpGet("{lote}")]
        public async Task<ActionResult> Details(string lote)
        {
            if (lote == null)
            {
                return NotFound();
            }

            var tcBaseIdi = await _context.TcBaseIdis.Where(m => m.Lote == lote).ToListAsync();
            if (tcBaseIdi == null)
            {
                return NotFound();
            }
            return new JsonResult(tcBaseIdi);

        }

        // POST: TcBaseIdi/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<BaseIdiDisplay> tcBaseIdi)
        {
            var existe = await this._context.TcBaseIdis.AnyAsync(t => t.Lote == tcBaseIdi[0].Lote);
            if (existe)
            {
                return new JsonResult("Ya existe el Lote.");
            }
            else
            {
                var mapBaseIdi = mapper.Map<List<TcBaseIdi>>(tcBaseIdi);
                _context.AddRange(mapBaseIdi);
                await _context.SaveChangesAsync();
                return new JsonResult(mapBaseIdi);
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

            var tcBaseIdi = await _context.TcBaseIdis.Where(m => m.IdTablero == id).ToListAsync();
            if (tcBaseIdi == null)
            {
                return NotFound();
            }
            else { 
                this._context.RemoveRange(tcBaseIdi);
                await this._context.SaveChangesAsync();
                return new JsonResult("El Registro se ha borrado con Éxito");
            }
        }

        
    }
}
