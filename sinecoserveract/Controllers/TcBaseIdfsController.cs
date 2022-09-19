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
    [Route("/api/baseidf")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TcBaseIdfsController : ControllerBase
    {
        private readonly WebSIDataContext _context;
        private readonly IMapper mapper;

        public TcBaseIdfsController(WebSIDataContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: TcBaseIdfs
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return new JsonResult(await _context.TcBaseIdfs.ToListAsync());
        }

        // GET: TcBaseIdfs/Details/5
        [HttpGet("{lote}")]
        public async Task<ActionResult> Details(string? lote)
        {
            if (lote == null)
            {
                return NotFound();
            }

            var tcBaseIdf = await _context.TcBaseIdfs
                .Where(m => m.Lote == lote).ToListAsync();
            if (tcBaseIdf == null)
            {
                return NotFound();
            }

            return new JsonResult(tcBaseIdf);
        }

        // POST: TcBaseIdfs/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] List<BaseIdfDisplay> tcBaseIdf)
        {
          
                var mapBaseIdf = mapper.Map<List<TcBaseIdf>>(tcBaseIdf);
                _context.AddRange(mapBaseIdf);
                await _context.SaveChangesAsync();
                return new JsonResult(tcBaseIdf);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcBaseIdf = await _context.TcBaseIdfs
                .FirstOrDefaultAsync(m => m.IdTablero == id);
            if (tcBaseIdf == null)
            {
                return NotFound();
            }

            return new JsonResult("El Registro se ha borrado con Éxito");
        }

    }
}
