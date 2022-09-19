using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using sinecoserveract.Models;
using sinecoserveract.Utilidades;
using sinecoserveract.DTOs;
using sinecoserveract.Entidades;
using System.Text.Json;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/grupos")]
   
    public class GruposController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;

        public GruposController(WebSIDataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }

        // GET: GruposController
        [HttpGet]
        public async Task<ActionResult> Index([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {

                var queryable = this.context.DfGrupos.AsQueryable();
                var cantidPaginas = await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidaddeRegistroPorPagina);
                var ctaGrupos = await queryable.Paginar(paginacionDTO).ToListAsync();
                RespuestaPaginacion resP = new RespuestaPaginacion()
                {
                    CantidadPaginas = cantidPaginas,
                    Respuesta = JsonSerializer.Serialize(ctaGrupos)
                };


                return new JsonResult(resP);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }

        // GET: GruposController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var ctaGrupos = await this.context.DfGrupos.FirstOrDefaultAsync(c => c.IdGrupo == id);
                return new JsonResult(ctaGrupos);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }


        // POST: GruposController/Create
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Create([FromBody] DfGrupos grupo)
        {
            try
            {
                var existe = await this.context.DfGrupos.AnyAsync(g => g.IdGrupo == grupo.IdGrupo && g.IdCia == grupo.IdCia);
                if (existe) {
                    return BadRequest("El grupo con nombre " + grupo.Nombre_Grupo + " ya existe");
                }

                this.context.Add(grupo);
                await this.context.SaveChangesAsync();
                return Ok(new JsonResult(grupo));
            }
            catch (Exception ex)
            {
                return BadRequest("Error Post: " + ex.Message);
            }
        }

        // GET: GruposController/Edit/5
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Edit([FromBody] DfGrupos grupo)
        {
            try
            {
                var existe = await this.context.DfGrupos.AnyAsync(g => g.IdGrupo == grupo.IdGrupo);
                if (existe)
                {
                    this.context.Entry(grupo).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    return Ok(new JsonResult(grupo));
                }
                else {
                    return BadRequest("No es posible actualizar el grupo");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Put: " + ex.Message);
            }
        }

        //delete
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var Existe = await this.context.DfGrupos.AnyAsync(g => g.IdGrupo == id);
                if (Existe)
                {
                    var grupo = await this.context.DfGrupos.FindAsync(id);
                    if (grupo != null)
                    {
                        if (grupo.Activo == true) {
                            grupo.Activo = false;
                        }
                        else {
                            grupo.Activo = true;
                        }
                        await this.context.SaveChangesAsync();
                    }
                    return new JsonResult(grupo);
                }
                else {
                    return BadRequest("Grupo no existe " + id );
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Delete " + ex.Message);
            }
        }

    
    }
}
