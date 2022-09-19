using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using sinecoserveract.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/archivoevento")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventosArchivosController : ControllerBase
    {

        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenador;

        public EventosArchivosController(WebSIDataContext context,
                                         IConfiguration configuration,
                                         IMapper mapper,
                                         IAlmacenadorArchivos almacenador)
        {
            this.context = context;
            this.Configuration = configuration;
            this.mapper = mapper;
            this.almacenador = almacenador;
        }



        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var temas = await (from t in this.context.CpEventoArchivo
                                   select t).ToListAsync();
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
                return BadRequest("Index Error " + ex.Message);
            }
        }



        [HttpGet("{idevento}")]
        public async Task<ActionResult> Temaxid(int idevento)
        {
            try
            {
                var eventoarchivos = await (from ev in this.context.CpEventoArchivo
                                            join tema in this.context.CpTemas on ev.IdTema equals tema.IdTema
                                            join temario in this.context.CpTemarios on ev.IdTemario equals temario.IdTemario
                                            where ev.IdEvento== idevento
                                            select new
                                            {
                                                IdArchivo = ev.IdArchivo,
                                                IdTema = ev.IdTema,
                                                IdTemario = ev.IdTemario,
                                                Tema = tema.Tema,
                                                Temario = temario.Temario,
                                                NombreArchvio = ev.NombreArchivo
                                            }).ToListAsync();

                if (eventoarchivos != null)
                {
                    return new JsonResult(eventoarchivos);
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

        // GET: TemasController/Details/5
        [HttpGet("{idevento}/{idtema}/{idtemario}")]
        public async Task<ActionResult> Temaxid(int idevento, int idtema, int idtemario)
        {
            try
            {
                var eventoarchivos = await (from ev in this.context.CpEventoArchivo
                                            join tema in this.context.CpTemas on ev.IdTema equals tema.IdTema
                                            join temario in this.context.CpTemarios on ev.IdTemario equals temario.IdTemario
                                            where ev.IdTema==idtema && ev.IdTemario==idtemario && ev.IdEvento==idevento
                                            select new
                                            {
                                                IdArchivo = ev.IdArchivo,
                                                IdTema = ev.IdTema,
                                                IdTemario = ev.IdTemario,
                                                Tema = tema.Tema,
                                                Temario = temario.Temario,
                                                NombreArchvio = ev.NombreArchivo
                                            }).ToListAsync();

                if (eventoarchivos != null)
                {
                    return new JsonResult(eventoarchivos);
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
        public async Task<ActionResult> Post(EventoArchivoDisplay eventoarchivo)
        {
            try
            {
                var existe = await this.context.CpEventoArchivo.AnyAsync(t => t.IdEvento == eventoarchivo.IdEvento && t.IdTema == eventoarchivo.IdTema && t.IdTemario == eventoarchivo.IdTemario && t.NombreArchivo == eventoarchivo.NombreArchivo);
                if (!existe)
                {
                    var t = mapper.Map<CpEventoArchivo>(eventoarchivo);
                    this.context.Add(t);
                    await this.context.SaveChangesAsync();

                    var temaCreado = await this.context.CpEventoArchivo.OrderByDescending(t => t.IdArchivo).FirstOrDefaultAsync();
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
        //[HttpPut]
        //public async Task<ActionResult> Put(CpTemas tema)
        //{
        //    try
        //    {
        //        CpTemas t = await this.context.CpTemas.FirstOrDefaultAsync(te => te.IdTema == tema.IdTema);
        //        if (t != null)
        //        {
        //            t.IdMunicipio = tema.IdMunicipio;
        //            t.Tema = tema.Tema;
        //            t.Activo = tema.Activo;
        //            this.context.Entry(t).State = EntityState.Modified;
        //            await this.context.SaveChangesAsync();
        //            return new JsonResult($"El tema {tema.Tema} se modificó con éxito.");
        //        }
        //        else
        //        {
        //            return NoContent();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest("Put Error " + ex.Message);
        //    }

        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string Carpeta = "materialcapacitacion";
                string RutaActual = "";

                CpEventoArchivo te = await this.context.CpEventoArchivo.FirstOrDefaultAsync(t => t.IdArchivo == id);
                this.context.Remove(te);
                await this.context.SaveChangesAsync();
                //se borra del localstorage de azure
                if (te.Archivo != "") {
                    RutaActual = te.Archivo;
                    await this.almacenador.BorrarArchivo(RutaActual, Carpeta);
                }

                return new JsonResult($"El Archivo se eliminó con éxito.");

            }
            catch (Exception ex)
            {

                return BadRequest("Delete Error " + ex.Message);
            }
        }

        [HttpPost("uploadarchivo")]
        public async Task<ActionResult> UploadImagen([FromForm] EventoArchivoDisplay archivos)
        {
            ResArchivo resArchivo = new ResArchivo();
            try
            {
                string rutaActual = "";

                if (archivos.IdTema == 0 || archivos.IdTemario == 0) { return BadRequest("No se puede procesar el archivo"); }
                var docBD = await this.context.CpEventoArchivo.Where(d => d.IdTema == archivos.IdTema && d.IdTemario == archivos.IdTemario && d.NombreArchivo == archivos.NombreArchivo).FirstOrDefaultAsync();
                if (docBD != null)
                {
                    rutaActual = docBD.Archivo;
                }
                else
                {
                    rutaActual = "";
                }

                //se pasa la informacion que el usuario modifica a la consulta hecha a la base de datos
                //string Carpeta = $"{municipio.ToLower()}{nombregrupo.ToLower()}";
                string Carpeta = "materialcapacitacion";
                if (archivos.File != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivos.File.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var nombreArchivo = Path.GetFileName(archivos.File.FileName);
                        var extension = Path.GetExtension(archivos.File.FileName);
                        string rutaArchivo = "";
                        if (string.IsNullOrEmpty(rutaActual))
                        {
                            rutaArchivo = await this.almacenador.GuardarArchivo(contenido, extension, Carpeta, archivos.File.ContentType);
                        }
                        else
                        {
                            rutaArchivo = await this.almacenador.EditarArchivo(contenido, extension, Carpeta, archivos.File.ContentType, rutaActual);
                        }
                        resArchivo.NombreArchivo = nombreArchivo;
                        resArchivo.RutaArchivo = rutaArchivo;

                    }
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error " + ex.Message);
            }
            return new JsonResult(resArchivo);
        }


    }
}
