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
    [Route("/api/baseidfdos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TcBaseIdf2Controller : Controller
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;

        public TcBaseIdf2Controller(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: TcBaseIdf2
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return new JsonResult(await _context.TcBaseIdf2s.ToListAsync());
        }

        // GET: TcBaseIdf2/Details/5
        [HttpGet("{lote}")]
        public async Task<ActionResult> Details(string lote)
        {
            if (lote == null)
            {
                return NotFound();
            }

            var tcBaseIdfdos = await _context.TcBaseIdf2s
                .Where(m => m.Lote == lote).ToListAsync();
            if (tcBaseIdfdos == null)
            {
                return NotFound();
            }

            return new JsonResult(tcBaseIdfdos);
        }

        // POST: TcBaseIdf2/Create
        [HttpPost]

        public async Task<ActionResult> Create([FromBody] List<BaseIdf2Display> tcBaseIdf2)
        {
            var existe = await this._context.TcBaseIdf2s.AnyAsync(t => t.Lote == tcBaseIdf2[0].Lote);
            if (existe)
            {
                return new JsonResult("Ya existe el Lote.");
            }
            else { 
                var mapBaseIdfdos = mapper.Map<List<TcBaseIdf2>>(tcBaseIdf2);
                _context.AddRange(mapBaseIdfdos);
                await _context.SaveChangesAsync();
                return new JsonResult(tcBaseIdf2);
            }

        }

        // DELETE: TcBaseIdf2/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcBaseIdf2 = await _context.TcBaseIdf2s
                .Where(m => m.IdTablero == id).ToListAsync();
            if (tcBaseIdf2 == null)
            {
                return NotFound();
            }
            this._context.RemoveRange(tcBaseIdf2);
            await this._context.SaveChangesAsync();

            return new JsonResult("El Registro se ha borrado con Éxito");
        }
    }
}
