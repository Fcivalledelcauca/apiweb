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
    [Route("/api/basemdm")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TcBaseMdmsController : ControllerBase
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;

        public TcBaseMdmsController(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: TcBaseMdms
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return new JsonResult(await _context.TcBaseMdms.ToListAsync());
        }

        // GET: TcBaseMdms/Details/5
        [HttpGet("{lote}")]
        public async Task<ActionResult> Details(string lote)
        {
            if (lote == null)
            {
                return NotFound();
            }

            var tcBaseMdm = await _context.TcBaseMdms
                .Where(m =>m.Lote == lote).ToListAsync();
            if (tcBaseMdm == null)
            {
                return NotFound();
            }

            return new JsonResult(tcBaseMdm);
        }

        // POST: TcBaseMdms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<BaseMdmDisplay> tcBaseMdm)
        {
            try
            {
                var existe = await this._context.TcBaseMdms.AnyAsync(t => t.Lote == tcBaseMdm[0].Lote);
                if (existe)
                {
                    return new JsonResult("Ya existe el Lote.");
                }
                var mapBaseMdm = mapper.Map<List<TcBaseMdm>>(tcBaseMdm);
                _context.AddRange(mapBaseMdm);
                await _context.SaveChangesAsync();
                return new JsonResult(tcBaseMdm);
            }
            catch (Exception ex)
            {
                return BadRequest("Error POS."+ex.InnerException);
            }

        }

        // DELETE: TcBaseMdms/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var tcBaseMdm = await _context.TcBaseMdms.Where(b=>b.IdTablero==id).ToListAsync();
            _context.TcBaseMdms.RemoveRange(tcBaseMdm);
            await _context.SaveChangesAsync();
            return new JsonResult("El Registro se ha borrado con Éxito");
        }
    }
}
