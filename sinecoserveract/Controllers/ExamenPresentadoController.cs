using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sinecoserveract.Models;
using sinecoserveract.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/test")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExamenPresentadoController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IAlmacenadorArchivos almacenador;
        private readonly IMapper mapper;

        public ExamenPresentadoController(WebSIDataContext context, IConfiguration configuration, IAlmacenadorArchivos almacenador, IMapper mapper)
        {
            this.context = context;
            this.Configuration = configuration;
            this.almacenador = almacenador;
            this.mapper = mapper;
        }

        [HttpGet("{usuario}/{idtema}")]
        public async Task<ActionResult> Get(string usuario, int idtema) {
            try
            {
                var cta = await (from ep in this.context.CpExamenPresentado
                                 join ex in this.context.CpExamen on ep.IdExamen equals ex.IdExamen
                                 join tema in this.context.CpTemas on ex.IdTema equals tema.IdTema
                                 where ep.Usuario==usuario && ex.IdTema==idtema
                                 select new
                                 {
                                     Tema = tema.Tema,
                                     Examen = ex.Nombre,
                                     PuntuacionMinima = ep.PuntosMin,
                                     Puntuacion = ep.Puntuacion,
                                     RespuestasCorrectas = ep.Can_Ok,
                                     RespuestasIncorrectas = ep.Can_Fail,
                                     Aprobo=ep.Aprobo
                                 }).ToListAsync();
                if (cta != null)
                {
                    return new JsonResult(cta);
                }
                else {
                    return new JsonResult("No se encontró ningun resultado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error en GET: " + ex.InnerException);
            }
        }


        [HttpPost]
        public async Task<ActionResult> PostExamenP([FromBody] CpExamenPresentado exp) {
            try
            {
                this.context.Add(exp);
                await this.context.SaveChangesAsync();
                CpExamenPresentado expr = await this.context.CpExamenPresentado.OrderByDescending(e => e.IdExamenPresentado).FirstOrDefaultAsync();
                if (expr != null)
                {
                    return new JsonResult(expr);
                }
                else
                {
                    return NotFound("No se creó el registro.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error pos examen " + ex.Message);   
            }
        }


        [HttpPut]
        public async Task<ActionResult> Put([FromBody] CpExamenPresentado ep) {
            try
            {
                var sql = await this.context.CpExamenPresentado.Where(e => e.IdExamenPresentado == ep.IdExamenPresentado).FirstOrDefaultAsync();
                if (sql != null)
                {
                    sql.Puntuacion = ep.Puntuacion;
                    sql.Can_Ok = ep.Can_Ok;
                    sql.Can_Fail = ep.Can_Fail;
                    sql.Aprobo = ep.Puntuacion >= sql.PuntosMin ? true : false;
                    this.context.Entry(sql).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    var ctaSql = await (from e in this.context.CpExamenPresentado
                                        join p in this.context.MmPerfilUsuarios on e.Usuario equals p.Usuario
                                        where e.IdExamenPresentado == ep.IdExamenPresentado
                                        select new
                                        {
                                            IdExamenPresentado = e.IdExamenPresentado,
                                            IdExamen = e.IdExamen,
                                            NombreUsuario = p.NombreCompleto,
                                            Can_Ok = e.Can_Ok,
                                            Can_Fail = e.Can_Fail,
                                            PuntuacionMinima = e.PuntosMin,
                                            Puntuacion = e.Puntuacion,
                                            Aprobo=e.Aprobo
                                        }
                                    ).FirstOrDefaultAsync();
                    if (ctaSql != null)
                    {
                        return new JsonResult(ctaSql);
                    }
                    else
                    {
                        return BadRequest("El registro no se Editó");
                    }
                }
                else {
                    return NotFound("El registro no se encontró.");
                }



                    

            }
            catch (Exception ex)
            {
                return BadRequest("Error put examenPresentada " + ex.Message);
            }
        }
    }
}
