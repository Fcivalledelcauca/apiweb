using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("/api/pregunta")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PreguntasController : ControllerBase
    {
        private readonly WebSIDataContext _context;

        public PreguntasController(WebSIDataContext context)
        {
            _context = context;
        }

        // GET: Preguntas
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                return new JsonResult(await _context.CpPregunta.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Error Get " + ex.Message);

            }

        }

        [HttpGet("{idexamen}")]

        // GET: Preguntas/Details/5
        public async Task<IActionResult> Details(int? idexamen)
        {
            var cpPregunta = await _context.CpPregunta.Where(m => m.IdExamen == idexamen).ToListAsync();
            if (cpPregunta == null)
            {
                return NotFound();
            }

            return new JsonResult(cpPregunta);
        }

        // GET: Preguntas/Create
        [HttpPost]
        public async Task<ActionResult> Create(CpPregunta pregunta)
        {
            try
            {
                var existe = await this._context.CpPregunta.AnyAsync(p => p.IdExamen == pregunta.IdExamen && p.Pregunta == pregunta.Pregunta);
                if (!existe)
                {

                    this._context.Add(pregunta);
                    await this._context.SaveChangesAsync();
                    var preg = await this._context.CpPregunta.OrderByDescending(p => p.IdPregunta).FirstOrDefaultAsync();
                    if (preg != null)
                    {
                        return new JsonResult(preg);
                    }
                    else
                    {
                        return BadRequest("No se creó la pregunta.");
                    }

                }
                else {
                    return BadRequest("La pregunta ya existe");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Get " + ex.Message);
            }
        }


        // GET: Preguntas/Edit/5
        [HttpPut]
        public async Task<IActionResult> Edit(CpPregunta pregunta)
        {
            var cpPregunta = await _context.CpPregunta.FindAsync(pregunta.IdPregunta);
            if (cpPregunta != null)
            {
                cpPregunta.IdExamen = pregunta.IdExamen;
                cpPregunta.Pregunta = pregunta.Pregunta;
                cpPregunta.Puntos = pregunta.Puntos;
                this._context.Entry(cpPregunta).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                return new JsonResult(cpPregunta);
            }
            else {
                return NotFound("No se encontró la pregunta.");
            }

        }


        // GET: Preguntas/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cpPregunta = await _context.CpPregunta
                .FirstOrDefaultAsync(m => m.IdPregunta == id);
            if (cpPregunta == null)
            {
                return NotFound();
            }
            this._context.Remove(cpPregunta);
            await this._context.SaveChangesAsync();
            Respuesta res = new Respuesta()
            {
                Ok = "true",
                Mensaje = "La pregunta se eliminó con éxito."
            };
            return new JsonResult(res);
        }
       
    }
}
