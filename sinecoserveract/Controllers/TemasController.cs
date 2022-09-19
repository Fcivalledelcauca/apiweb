using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("/api/temas")]
    public class TemasController : ControllerBase
    {

        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IMapper mapper;

        public TemasController(WebSIDataContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.Configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {

                var queryable = (from t in this.context.CpTemas
                                 select t).AsQueryable();
                var cantidPaginas = await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidaddeRegistroPorPagina);
                var temas = await queryable.Paginar(paginacionDTO).ToListAsync();
                RespuestaPaginacion resP = new RespuestaPaginacion()
                {
                    CantidadPaginas = cantidPaginas,
                    Respuesta = JsonSerializer.Serialize(temas)
                };
                return new JsonResult(resP);
            }
            catch (Exception ex)
            {
                return BadRequest("Index Error " + ex.Message);
            }
        }

        // GET: TemasController/Details/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Temaxid(int id)
        {
            try
            {

                var temas = await (from t in this.context.CpMunicipioTema
                                   join m in this.context.MmMunicipios on t.IdMunicipio equals m.IdMunicipio
                                   join tem in this.context.CpTemas on t.IdTema equals tem.IdTema
                                   where t.IdMunicipio == id
                                   select new
                                   {
                                       IdTema = t.IdTema,
                                       IdMunicipio = t.IdMunicipio,
                                       Municipio = m.Nombre,
                                       Tema = tem.Tema,
                                       Activo = t.Activo
                                   }).ToListAsync();
                if (temas != null)
                {
                    return new JsonResult(temas);
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {

                return BadRequest("temaxid Error " + ex.Message);
            }
        }

        // GET: TemasController/Create
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(TemasDisplay tema)
        {
            try
            {
                var existe = await this.context.CpTemas.AnyAsync(t => t.Tema == tema.Tema);
                if (!existe)
                {
                    var t = mapper.Map<CpTemas>(tema);
                    this.context.Add(t);
                    await this.context.SaveChangesAsync();

                    var temaCreado = await this.context.CpTemas.OrderByDescending(t => t.IdTema).FirstOrDefaultAsync();
                    if (temaCreado != null)
                    {
                        return new JsonResult(temaCreado);
                    }
                    else
                    {
                        return NotFound();
                    }


                }
                else
                {
                    return new JsonResult("El tema ya está creado.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Post Error " + ex.Message);
            }
        }

        // GET: TemasController/Edit/5
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(CpTemas tema)
        {
            try
            {
                CpTemas t = await this.context.CpTemas.FirstOrDefaultAsync(te => te.IdTema == tema.IdTema);
                if (t != null)
                {
                    t.IdMunicipio = tema.IdMunicipio;
                    t.Tema = tema.Tema;
                    t.Activo = tema.Activo;
                    this.context.Entry(t).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    return new JsonResult($"El tema {tema.Tema} se modificó con éxito.");
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {

                return BadRequest("Put Error " + ex.Message);
            }

        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //Validamos que el tema no tenga ningun temario
                var existe = await this.context.CpTemarios.AnyAsync(t => t.IdTema == id);
                if (existe)
                {
                    return BadRequest("El tema tiene temarios y no se puede eliminar.");
                }
                //Validamos que el tema no este relacionado con un municipio
                var municipiotema = await this.context.CpMunicipioTema.Where(m => m.IdTema == id).ToListAsync();
                if (municipiotema.Count != 0) {
                    return BadRequest($"El tema ha sido seleccionado por algun municipio");
                }

                CpTemas te = await this.context.CpTemas.FirstOrDefaultAsync(t => t.IdTema == id);
                this.context.Remove(te);
                await this.context.SaveChangesAsync();
                return new JsonResult($"El tema se eliminó con éxito.");

            }
            catch (Exception ex)
            {

                return BadRequest("Delete Error " + ex.Message);
            }
        }



    }
}
