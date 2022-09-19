using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.IO;
using sinecoserveract.Servicios;
using Microsoft.AspNetCore.Http;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/perfiles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PerfilController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenador;
        private readonly string Carpeta = "fotosperfiles";
        public PerfilController(WebSIDataContext context,
                            IConfiguration configuration,
                            IMapper mapper,
                            IAlmacenadorArchivos almacenador)
        {
            this.context = context;
            this.Configuration = configuration;
            this.mapper = mapper;
            this.almacenador = almacenador;
        }

        [HttpGet("{usuario}")]
        public async Task<ActionResult<MmPerfilUsuario>> Get(string usuario)
        {
            MmPerfilUsuario perfiles = new MmPerfilUsuario();
            try
            {

                var ctaperfiles = await this.context.MmPerfilUsuarios.FirstOrDefaultAsync(p => p.Usuario == usuario);
                if (ctaperfiles != null)
                {
                    perfiles = ctaperfiles;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return new JsonResult(perfiles);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PerfilUsuarioDisplay perfil)
        {
            try
            {
                var SiExiste = await this.context.MmPerfilUsuarios.AnyAsync(p => p.NombreCompleto == perfil.NombreCompleto || p.Email == perfil.Email);

                if (SiExiste)
                {
                    return BadRequest("El Usuario ya Existe.");
                }

                var mapPerfil = mapper.Map<MmPerfilUsuario>(perfil);

                if (perfil.Imagen != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await perfil.Imagen.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(perfil.Imagen.FileName);
                        mapPerfil.Imagen = await this.almacenador.GuardarArchivo(contenido, extension, Carpeta, perfil.Imagen.ContentType);
                    }
                }

                this.context.Add(mapPerfil);
                await this.context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok("usuario creado");
        }

        [HttpPut]
        public async Task<ActionResult> Put(PerfilUsuarioDisplay perfil)
        {
            string rutaFoto = "";
            try
            {
                var perfilDB = await this.context.MmPerfilUsuarios.FirstOrDefaultAsync(p => p.Usuario == perfil.Usuario);
                if (perfilDB == null) { return BadRequest("No se Encontro el Usuario " + perfil.Usuario); ; }

                //se pasa la informacion que el usuario modifica a la consulta hecha a la base de datos
                perfilDB = mapper.Map(perfil, perfilDB);

                if (perfil.Imagen != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await perfil.Imagen.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(perfil.Imagen.FileName);
                        perfilDB.Imagen = await this.almacenador.EditarArchivo(contenido, extension, Carpeta, perfil.Imagen.ContentType, perfilDB.Imagen);
                        rutaFoto = perfilDB.Imagen;
                    }
                }

                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                return BadRequest("Error " + ex.Message);
            }

            return Ok(rutaFoto);
        }

        [HttpPut("uploadimagenperfil")]
        public async Task<ActionResult> UploadImagen([FromForm] formArchivos archivos)
        {
            string rutaFoto = "";
            try
            {
                var perfilDB = await this.context.MmPerfilUsuarios.FirstOrDefaultAsync(p => p.Usuario == archivos.Usuario);
                if (perfilDB == null) { return BadRequest("No se Encontro el Usuario " + archivos.Usuario); ; }

                //se pasa la informacion que el usuario modifica a la consulta hecha a la base de datos

                if (archivos.Imagen != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivos.Imagen.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(archivos.Imagen.FileName);
                        perfilDB.Imagen = await this.almacenador.EditarArchivo(contenido, extension, Carpeta, archivos.Imagen.ContentType, perfilDB.Imagen);
                        rutaFoto = perfilDB.Imagen;
                    }
                }

                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                return BadRequest("Error " + ex.Message);
            }
            return new JsonResult(rutaFoto);
        }
    }
}
