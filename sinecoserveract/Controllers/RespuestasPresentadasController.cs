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
    [Route("/api/respuestapresentadas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RespuestasPresentadasController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration configuration;
        private readonly IAlmacenadorArchivos almacenador;
        private readonly IMapper mapper;

        public RespuestasPresentadasController(WebSIDataContext context, IConfiguration configuration, IAlmacenadorArchivos almacenador, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.almacenador = almacenador;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> PostRespuestaP(string test, [FromBody] CpRespuestaPresentada resp)
        {
            try
            {
                var revision = await (from r in this.context.CpRespuesta
                                      join p in this.context.CpPregunta on r.IdPregunta equals p.IdPregunta
                                      where r.IdRespuesta == resp.IdRespuesta
                                      select new
                                      {
                                          IdRespuesta = r.IdRespuesta,
                                          Puntos = p.Puntos,
                                          Correcta = r.Correcta
                                      }).FirstOrDefaultAsync();
                if (revision != null)
                {
                    if (revision.Correcta == 1)
                    {
                        resp.Puntos = revision.Puntos;
                    }
                    this.context.Add(resp);
                    var t = mapper.Map<CpRespuestaPresentada>(resp);
                    await this.context.SaveChangesAsync();
                    //--------consultamos las preguntas adicionadas y las devolvemos////
                    List<CpRespuestaPresentada> expr = await this.context.CpRespuestaPresentada.Where(r => r.IdExamenPresentado == resp.IdExamenPresentado).ToListAsync();
                    if (expr != null)
                    {
                        return new JsonResult(expr);
                    }
                    else
                    {
                        return NotFound("No se encontró los registros.");
                    }
                }
                else
                {
                    return NotFound("La respuesta no se encontró");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error pos respPresentada " + ex.InnerException);
            }
        }
    }
}
