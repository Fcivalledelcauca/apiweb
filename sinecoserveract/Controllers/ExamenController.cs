using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sinecoserveract.DTOs;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using sinecoserveract.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/examen")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExamenController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration configuration;
        private readonly IMapper maper;

        public ExamenController(WebSIDataContext context, IConfiguration configuration, IMapper maper)
        {
            this.context = context;
            this.configuration = configuration;
            this.maper = maper;
        }


        [HttpGet]
        public async Task<ActionResult> Index([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {

                var queryable = (from ex in this.context.CpExamen
                                 join tema in this.context.CpTemas on ex.IdTema equals tema.IdTema
                                 select new
                                 {
                                     IdExamen = ex.IdExamen,
                                     IdTema = ex.IdTema,
                                     Tema = tema.Tema,
                                     Nombre = ex.Nombre,
                                     Puntuacion = ex.Puntuacion,
                                     Instrucciones = ex.Instrucciones,
                                     Activo = ex.Activo
                                 }).AsQueryable();
                var cantidPaginas = await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidaddeRegistroPorPagina);
                var examenes = await queryable.Paginar(paginacionDTO).ToListAsync();
                RespuestaPaginacion resP = new RespuestaPaginacion()
                {
                    CantidadPaginas = cantidPaginas,
                    Respuesta = JsonSerializer.Serialize(examenes)
                };
                return new JsonResult(resP);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }


        // GET: TemarioController/Details/5
        [HttpGet("{idexamen}")]
        public async Task<ActionResult> examenxid(int idexamen)
        {
            try
            {
                var examen = await (from ex in this.context.CpExamen
                                    join tema in this.context.CpTemas on ex.IdTema equals tema.IdTema
                                    where ex.IdExamen==idexamen
                                    select new
                                    {
                                          IdExamen = ex.IdExamen,
                                          IdTema = ex.IdTema,
                                          Tema = tema.Tema,
                                          Nombre = ex.Nombre,
                                          Puntuacion = ex.Puntuacion,
                                          Instrucciones = ex.Instrucciones,
                                          Activo = ex.Activo
                                    }).ToListAsync();
                if (examen != null)
                {
                    return new JsonResult(examen);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error getId: " + ex.Message);
            }
        }

        // GET: TemarioController/Details/5
        [HttpGet("{temas}/{idTema}")]
        public async Task<ActionResult> examenxtema(string temas,int idTema)
        {
            try
            {
                PresentarExamenDisplay exd = new PresentarExamenDisplay();
                
                var examen = await (from ex in this.context.CpExamen
                                    join tema in this.context.CpTemas on ex.IdTema equals tema.IdTema
                                    where ex.IdTema ==idTema
                                    select new
                                    {
                                        IdExamen = ex.IdExamen,
                                        IdTema = ex.IdTema,
                                        Tema = tema.Tema,
                                        Nombre = ex.Nombre,
                                        Puntuacion = ex.Puntuacion,
                                        Instrucciones = ex.Instrucciones,
                                        Activo = ex.Activo
                                    }).ToListAsync();
                if (examen != null)
                {

                    foreach (var i in examen)
                    {

                        exd.IdExamen = i.IdExamen;
                        exd.IdTema = i.IdTema;
                        exd.Nombre = i.Nombre;
                        exd.Puntuacion = i.Puntuacion;
                        exd.Instrucciones = i.Instrucciones;
                        exd.Activo = i.Activo;
                        List<Preguntas> listaP = new List<Preguntas>();
                        var preguntasr = await this.context.CpPregunta.Where(p => p.IdExamen == i.IdExamen).ToListAsync();
                        if (preguntasr != null) {
                            foreach (var j in preguntasr)
                            {
                                Preguntas p = new Preguntas();
                                p.IdPregunta = j.IdPregunta;
                                p.IdExamen = j.IdExamen;
                                p.Pregunta = j.Pregunta;
                                p.Puntos = j.Puntos;
                                List<Respuestas> lres = new List<Respuestas>();
                                var respuestas = await this.context.CpRespuesta.Where(r => r.IdPregunta == j.IdPregunta).ToListAsync();
                                foreach (var r in respuestas)
                                {
                                    Respuestas res = new Respuestas();
                                    res.IdRespuesta = r.IdRespuesta;
                                    res.IdPregunta = r.IdPregunta;
                                    res.Respuesta = r.Respuesta;
                                    res.Correcta = r.Correcta;
                                    lres.Add(res);
                                }
                                p.Respuestas = lres;
                                listaP.Add(p);
                            }
                            exd.Preguntas = listaP;
                        }
                    }
                    return new JsonResult(exd);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error getId: " + ex.Message);
            }
        }

        // GET: TemarioController/Create
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ExamenDisplay examen)
        {
            try
            {
                Respuesta res = new Respuesta();
                var existe = await this.context.CpExamen.AnyAsync(t => t.IdTema == examen.IdTema && t.Nombre==examen.Nombre);
                if (!existe)
                {
                    var exam = maper.Map<CpExamen>(examen);
                    this.context.Add(exam);
                    await this.context.SaveChangesAsync();
                    var examenCreado = await this.context.CpExamen.OrderByDescending(e => e.IdExamen).FirstOrDefaultAsync();
                    return new JsonResult(examenCreado);
                }
                else
                {
                    return BadRequest("El registro ya existe.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Post: " + ex.Message);
            }
        }


        // GET: TemarioController/Edit/5
        [HttpPut]
        public async Task<ActionResult> Put(CpExamen examen)
        {
            try
            {
                CpExamen tem = await this.context.CpExamen.Where(t => t.IdExamen==examen.IdExamen).FirstOrDefaultAsync();
                if (tem != null)
                {
                    tem.IdTema = examen.IdTema;
                    tem.Nombre = examen.Nombre;
                    tem.Puntuacion = examen.Puntuacion;
                    tem.Instrucciones = examen.Instrucciones;
                    tem.Activo = examen.Activo;
                    this.context.Entry(tem).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    return new JsonResult($"El registro se modificó con éxito.");
                }
                else
                {
                    return NotFound("No se encontró el registro.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Put: " + ex.Message);
            }
        }



        // GET: TemarioController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //revisar que este examen no se haya realizado por ningun participante
                var examenPresentado = await this.context.CpExamenPresentado.Where(e => e.IdExamen == id).ToListAsync();
                if (examenPresentado!=null) {
                    if (examenPresentado.Count > 0) {
                        return BadRequest("No se puede eliminar el cuestionario, ya esta elaborado por uno ó varios participantes ");
                    }
                }
                //***************************************************************

                CpExamen mt = await this.context.CpExamen.FirstOrDefaultAsync(t => t.IdExamen == id);
                if (mt != null)
                {
                    CpPregunta pregunta = await this.context.CpPregunta.FirstOrDefaultAsync(p => p.IdExamen == mt.IdExamen);
                    if (pregunta != null) {
                        List<CpRespuesta> res = await this.context.CpRespuesta.Where(r => r.IdPregunta == pregunta.IdPregunta).ToListAsync();
                        if (res != null) {
                            this.context.RemoveRange(res);
                        }
                        this.context.Remove(pregunta);
                    }
                    this.context.Remove(mt);
                    await this.context.SaveChangesAsync();
                    return new JsonResult($"El registro se eliminó con éxito.");
                }
                else
                {
                    return NotFound("El registro no existe.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Delete: " + ex.Message);
            }
        }
    }
}
