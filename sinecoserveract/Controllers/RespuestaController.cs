using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/respuesta")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RespuestaController : ControllerBase
    {

        private readonly WebSIDataContext _context;

        public RespuestaController(WebSIDataContext context)
        {
            _context = context;
        }

        // GET: Preguntas
        //[HttpGet]
        //public async Task<ActionResult> Index()
        //{
        //    try
        //    {
        //        return new JsonResult(await _context.CpRespuesta.ToListAsync());
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Error Get " + ex.Message);

        //    }

        //}

        [HttpGet("{idpregunta}")]

        // GET: Preguntas/Details/5
        public async Task<IActionResult> Details(int idpregunta)
        {
            var cpRespuesta = await _context.CpRespuesta.Where(m => m.IdPregunta == idpregunta).ToListAsync();
            if (cpRespuesta == null)
            {
                return NotFound();
            }

            return new JsonResult(cpRespuesta);
        }

        // GET: Preguntas/Create
        [HttpPost]
        public async Task<ActionResult> Create(CpRespuesta respuesta)
        {
            try
            {
                var existe = await this._context.CpRespuesta.AnyAsync(r => r.IdPregunta == respuesta.IdPregunta && r.Respuesta == respuesta.Respuesta);
                if (!existe)
                {
                    this._context.Add(respuesta);
                    await this._context.SaveChangesAsync();
                    var resp = await this._context.CpRespuesta.OrderByDescending(p => p.IdRespuesta).FirstOrDefaultAsync();
                    if (resp != null)
                    {
                        return new JsonResult(resp);
                    }
                    else
                    {
                        return BadRequest("No se creó la respuesta.");
                    }
                }
                else {
                    return BadRequest("El registro ya existe.");
                }
               
            }
            catch (Exception ex)
            {

                return BadRequest("Error post " + ex.Message);
            }
        }

        // GET: Preguntas/Create

        [HttpDelete("{todo}/{idpregunta}")]
        public async Task<ActionResult> BorrarTodos(string todo, int idpregunta)
        {
            try
            {

                var eliminar = await this._context.CpRespuesta.Where(r => r.IdPregunta == idpregunta).ToListAsync();
                if (eliminar != null)
                {
                    this._context.RemoveRange(eliminar);
                    await this._context.SaveChangesAsync();
                    return new JsonResult("Eliminados con éxito.");
                }
                else {
                    return new JsonResult("");
                }
               
            }
            catch (Exception ex)
            {

                return BadRequest("Error post " + ex.Message);
            }
        }

        // GET: Preguntas/Edit/5
        [HttpPut]
        public async Task<IActionResult> Edit(CpRespuesta respuesta)
        {
            var cpRespuesta = await _context.CpRespuesta.FindAsync(respuesta.IdRespuesta);
            if (cpRespuesta != null)
            {
                cpRespuesta.IdPregunta = respuesta.IdPregunta;
                cpRespuesta.Respuesta = respuesta.Respuesta;
                cpRespuesta.Correcta = respuesta.Correcta;
                this._context.Entry(cpRespuesta).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                return new JsonResult(cpRespuesta);
            }
            else
            {
                return NotFound("No se encontró la respuesta.");
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

            var cpRespuesta = await _context.CpRespuesta.FirstOrDefaultAsync(m => m.IdRespuesta == id);
            if (cpRespuesta == null)
            {
                return NotFound();
            }
            this._context.Remove(cpRespuesta);
            await this._context.SaveChangesAsync();
            Respuesta res = new Respuesta()
            {
                Ok = "true",
                Mensaje = "La respuesta se eliminó con éxito."
            };
            return new JsonResult(res);
        }

    }
}
