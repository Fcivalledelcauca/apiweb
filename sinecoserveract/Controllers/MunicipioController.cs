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
    [Route("/api/municipio")]
    public class MunicipioController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public MunicipioController(WebSIDataContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult>  Index([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                var queryable = this.context.MmMunicipios.OrderBy(m => m.Nombre).AsQueryable();
                var cantidPaginas = await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidaddeRegistroPorPagina);
                var municipios = await queryable.Paginar(paginacionDTO).ToListAsync();
                RespuestaPaginacion resP = new RespuestaPaginacion()
                {
                    CantidadPaginas = cantidPaginas,
                    Respuesta = JsonSerializer.Serialize(municipios)
                };
                return new JsonResult(resP);


               
            }
            catch (Exception ex)
            {
                return BadRequest("Get Error " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MmMunicipio>> Municipioxid(int id) 
        {
            try
            {
                MmMunicipio municipio = await this.context.MmMunicipios.FirstOrDefaultAsync(m => m.IdMunicipio == id);
                if (municipio != null)
                {
                    return municipio;
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("municipioxid Error " + ex.Message);
            }
        }

        [HttpGet("{buscar}/{pmunicipio}")]
        public async Task<ActionResult> MunicipioxNombre(string buscar,string pmunicipio)
        {
            try
            {
                var municipio = await this.context.MmMunicipios.FirstOrDefaultAsync(m => m.Nombre == pmunicipio);
                if (municipio != null)
                {
                    return new JsonResult("");
                }
                else
                {
                    return new JsonResult("El Municipio no existe.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("municipioxid Error " + ex.Message);
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<MunicipioDisplay>> Post(MunicipioDisplay municipio)
        {
            try
            {
                var existe = await this.context.MmMunicipios.AnyAsync(m => m.Nombre.ToLower() == municipio.Nombre.ToLower());
                if (!existe)
                {
                    var mcipio=mapper.Map<MmMunicipio>(municipio);
                    this.context.Add(mcipio);
                    await this.context.SaveChangesAsync();
                    return municipio;
                }
                else {
                    return BadRequest("El municipio ya existe.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("POST Error " + ex.Message);
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(MmMunicipio municipio) {
            try
            {
                var munic = await this.context.MmMunicipios.Where(m => m.IdMunicipio == municipio.IdMunicipio).FirstOrDefaultAsync();
                if (munic != null)
                {
                    munic.Nombre = municipio.Nombre;
                    munic.Activo = municipio.Activo;
                    this.context.Entry(munic).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    return new JsonResult("El municipio se actualizó con éxito");
                }
                else {
                    return BadRequest("El usuario ya existe." );
                }

            }
            catch (Exception ex)
            {

                return BadRequest("PUT Error " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                MmMunicipio municipio = await this.context.MmMunicipios.FirstOrDefaultAsync(m => m.IdMunicipio == id);
                if (municipio != null) {
                    if (municipio.Activo == true)
                    {
                        municipio.Activo = false;
                    }
                    else {
                        municipio.Activo = true;
                    }

                    await this.context.SaveChangesAsync();
                    return new JsonResult(municipio);
                }
                else
                {
                    return NotFound("El municipio no se encontró");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Delete Error " + ex.Message);
            }
        }
    }

}
