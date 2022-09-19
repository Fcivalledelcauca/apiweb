using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sinecoserveract.DTOs;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using sinecoserveract.Servicios;
using sinecoserveract.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/mensaje")]
    public class MensajeController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IAlmacenadorArchivos almacenador;
        private readonly IMapper mapper;

        public MensajeController(WebSIDataContext context, IConfiguration configuration, IAlmacenadorArchivos almacenador, IMapper mapper)
        {
            this.context = context;
            this.Configuration = configuration;
            this.almacenador = almacenador;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Get([FromQuery] PaginacionDTO paginacionDTO) {
            try
            {

                var queryable = (from me in this.context.MmMensaje
                                 join muni in this.context.MmMunicipios on me.IdMunicipio equals muni.IdMunicipio
                                 select new
                                 {
                                     IdMensaje = me.IdMensaje,
                                     Nombre = me.Nombre,
                                     IdMunicipio = me.IdMunicipio,
                                     Municipio = muni.Nombre,
                                     Email = me.Email,
                                     Telefono = me.Telefono,
                                     Mensaje = me.Mensaje
                                 }).AsQueryable();
                var cantidPaginas = await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidaddeRegistroPorPagina);
                var mensajes = await queryable.Paginar(paginacionDTO).ToListAsync();
                RespuestaPaginacion resP = new RespuestaPaginacion()
                {
                    CantidadPaginas = cantidPaginas,
                    Respuesta = JsonSerializer.Serialize(mensajes)
                };
                return new JsonResult(resP);
               
            }
            catch (Exception ex)
            {

                return BadRequest("Error get Mensaje " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostMensaje([FromBody] MmMensaje mensaje)
        {
            try
            {
                this.context.Add(mensaje);
                await this.context.SaveChangesAsync();
                MmMensaje men = await this.context.MmMensaje.OrderByDescending(e => e.IdMensaje).FirstOrDefaultAsync();
                if (men != null)
                {
                    return new JsonResult(men);
                }
                else
                {
                    return NotFound("No se creó el registro.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error pos Mensaje " + ex.Message);
            }
        }


        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put([FromBody] MmMensaje mensaje)
        {
            try
            {
                var sql = await this.context.MmMensaje.Where(e => e.IdMensaje== mensaje.IdMensaje).FirstOrDefaultAsync();
                if (sql != null)
                {
                    sql.Nombre = mensaje.Nombre;
                    sql.IdMunicipio = mensaje.IdMunicipio;
                    sql.Email = mensaje.Email;
                    sql.Telefono = mensaje.Telefono;
                    sql.Mensaje = mensaje.Mensaje;
                    sql.TC = mensaje.TC;
                    this.context.Entry(sql).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    var ctaSql = await this.context.MmMensaje.FirstOrDefaultAsync(m=>m.IdMensaje==mensaje.IdMensaje);
                    if (ctaSql != null)
                    {
                        return new JsonResult(ctaSql);
                    }
                    else
                    {
                        return BadRequest("El registro no se Editó");
                    }
                }
                else
                {
                    return NotFound("El registro no se encontró.");
                }





            }
            catch (Exception ex)
            {
                return BadRequest("Error put Mensaje " + ex.Message);
            }
        }

        [HttpDelete("{idmensaje}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> delete(int idmensaje) {
            try
            {
                var sql = await this.context.MmMensaje.Where(m => m.IdMensaje == idmensaje).FirstOrDefaultAsync();
                if (sql != null) {
                    this.context.Remove(sql);
                    await this.context.SaveChangesAsync();
                    return new JsonResult("El registro se eliminó con éxito.");
                }
                else {
                    return NotFound("Registro no encontrado.");
                }
                
            }
            catch (Exception ex)
            {

                return BadRequest("Error delete Mensaje " + ex.Message);
            }
        }
    }
}
