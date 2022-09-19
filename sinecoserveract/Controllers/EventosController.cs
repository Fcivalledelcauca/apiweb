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
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/eventos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventosController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration configuration;
        private readonly IAlmacenadorArchivos almacenador;
        private readonly IMapper mapper;

        public EventosController(WebSIDataContext context, IConfiguration configuration, IAlmacenadorArchivos almacenador, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.almacenador = almacenador;
            this.mapper = mapper;
        }

        //consultar los evento por municipio
        [HttpGet("{municipio}/{idmunicipio:int}")]
        public async Task<ActionResult> Index(string municipio,int idmunicipio)
        {
            try
            {
                //int id = Convert.ToInt32(idtema);
                //byte mes = Convert.ToByte(DateTime.Now.Month);
                var eventos = await (from ev in this.context.CpEventos
                                     where ev.IdMunicipio==idmunicipio
                                     select ev).ToListAsync();
                if (eventos != null)
                {
                    return new JsonResult(eventos);
                }
                else {
                    return BadRequest("No se encontró ningun evento");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " Get No se encontró ningun evento");
            }
        }


        //consulta los temas por idEvento
        [HttpGet("{idEvento:int}")]
        public async Task<ActionResult<CpEventos>> GetId(int idEvento)
        {
            try
            {
                EventoMaterial material = new EventoMaterial();
                List<EventoArchivoParticipante> files = new List<EventoArchivoParticipante>();
                List<Temas> listadotemas = new List<Temas>();
               
                
                //byte mes = Convert.ToByte(DateTime.Now.Month);
                var eventos = await (from ev in this.context.CpEventos
                                     where ev.IdEvento == idEvento
                                     select ev).FirstOrDefaultAsync();
                if (eventos != null)
                {
                    material.IdCia = eventos.IdCia;
                    material.IdMunicipio = eventos.IdMunicipio;
                    material.Titulo = eventos.Titulo;
                    material.Descripcion = eventos.Descripcion;
                    material.Fecha_Ini = eventos.Fecha_Ini;
                    material.Fecha_Fin = eventos.Fecha_Fin;
                    material.TodoElDia = eventos.TodoElDia;
                    material.ColorEvento = eventos.ColorEvento;
                    material.Imagen = eventos.Imagen;
                    material.NombreArchivo = eventos.NombreArchivo;
                    material.Direccion = eventos.Direccion;
                    material.Link = eventos.Link;
                    material.Activo = eventos.Activo;
                    var temas = await (
                                           from arc in this.context.CpEventoArchivo
                                           join t in this.context.CpTemas on arc.IdTema equals t.IdTema
                                           where arc.IdEvento==eventos.IdEvento
                                           group arc by new { 
                                               arc.IdTema,
                                               t.Tema
                                           } into g
                                           select new
                                           {
                                               IdTema = g.Key.IdTema,
                                               NombreTema = g.Key.Tema,
                                           }).ToListAsync();
                        
                    if (temas != null) {
                        foreach (var i in temas)
                        {
                            List<Temarios> listadoTemarios = new List<Temarios>();
                            Temas tem = new Temas();
                            tem.IdTema = i.IdTema;
                            tem.NombreTema = i.NombreTema;
                            var temarios = await (from arc in this.context.CpEventoArchivo
                                                join temar in this.context.CpTemarios on arc.IdTemario equals temar.IdTemario
                                                where arc.IdEvento == eventos.IdEvento && arc.IdTema==i.IdTema
                                                group arc by new
                                                {
                                                    arc.IdTemario,
                                                    temar.Temario
                                                } into g
                                                select new
                                                {
                                                    IdTemario = g.Key.IdTemario,
                                                    NombreTemario = g.Key.Temario,
                                                }).ToListAsync();
                            if (temarios != null) {
                                foreach (var t in temarios)
                                {
                                    List<ArchivosGuardados> listadoArchivos = new List<ArchivosGuardados>();
                                    Temarios ti = new Temarios();
                                    ti.IdTemario = t.IdTemario;
                                    ti.NombreTemario = t.NombreTemario;
                                    var archivosg = await this.context.CpEventoArchivo.Where(ea => ea.IdEvento == eventos.IdEvento && ea.IdTemario == t.IdTemario).ToListAsync();
                                    if (archivosg != null) {
                                        foreach (var ar in archivosg)
                                        {
                                            ArchivosGuardados argu = new ArchivosGuardados();
                                            argu.Archivo = ar.Archivo;
                                            argu.NombreArchivo = ar.NombreArchivo;
                                            listadoArchivos.Add(argu);
                                        }
                                        ti.Archivos = listadoArchivos;
                                    }
                                    listadoTemarios.Add(ti);
                                }
                                tem.Temarios = listadoTemarios;
                            }
                            listadotemas.Add(tem);
                        }
                        material.Temas =listadotemas;
                    }
                    return new JsonResult(material);
                }
                else
                {
                    return BadRequest("No se encontró ningun evento");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " Get No se encontró ningun evento");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EventosDisplay eventos)
        {
            try
            {
                var ev = await this.context.CpEventos.AnyAsync(e => e.Titulo == eventos.Titulo && e.IdMunicipio == eventos.IdMunicipio && e.Descripcion == eventos.Descripcion);
                if (!ev)
                {
                    var eve = mapper.Map<CpEventos>(eventos);
                    this.context.Add(eve);
                    await this.context.SaveChangesAsync();

                    var eventoCreado = await this.context.CpEventos.OrderByDescending(e => e.IdEvento).FirstOrDefaultAsync();
                    if (eventoCreado != null)
                    {
                        return new JsonResult(eventoCreado);
                    }
                    else {
                        return NotFound("No se creo ningun evento.");
                    }


                }
                else {
                    return BadRequest("El evento ya existe.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " POST Revise el metodo");
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(CpEventos ev) {
            try
            {
                var evento = await this.context.CpEventos.Where(e=>e.IdEvento==ev.IdEvento).FirstOrDefaultAsync();
                if (evento != null)
                {
                    evento.IdMunicipio = ev.IdMunicipio;
                    evento.Titulo = ev.Titulo;
                    evento.Descripcion = ev.Descripcion;
                    evento.Fecha_Ini = ev.Fecha_Ini;
                    evento.Fecha_Fin = ev.Fecha_Fin;
                    evento.TodoElDia = ev.TodoElDia;
                    evento.Imagen = ev.Imagen;
                    evento.NombreArchivo = ev.NombreArchivo;
                    evento.Direccion = ev.Direccion;
                    evento.Link = ev.Link;
                    evento.Activo = ev.Activo;
                    evento.ColorEvento = ev.ColorEvento;
                    this.context.Entry(evento).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    return new JsonResult(ev);
                }
                else {
                    return BadRequest("No existe el evento.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " POST Revise el metodo");
            }

        }

        [HttpPost("uploadarchivo")]
        public async Task<ActionResult> UploadImagen([FromForm] formImagenEvento archivos)
        {
            ResArchivo resArchivo = new ResArchivo();
            try
            {
                string rutaActual = "";


                var docBD = await this.context.CpEventos.Where(d => d.IdEvento == archivos.IdEvento).FirstOrDefaultAsync();
                if (docBD != null)
                {
                    rutaActual = docBD.Imagen;
                }
                else
                {
                    rutaActual = "";
                }

                //se pasa la informacion que el usuario modifica a la consulta hecha a la base de datos
                //string Carpeta = $"{municipio.ToLower()}{nombregrupo.ToLower()}";
                string Carpeta = "imageneseventos";
                if (archivos.Imagen != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivos.Imagen.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var nombreArchivo = Path.GetFileName(archivos.Imagen.FileName);
                        var extension = Path.GetExtension(archivos.Imagen.FileName);
                        string rutaArchivo = "";
                        if (string.IsNullOrEmpty(rutaActual))
                        {
                            rutaArchivo = await this.almacenador.GuardarArchivo(contenido, extension, Carpeta, archivos.Imagen.ContentType);
                        }
                        else
                        {
                            rutaArchivo = await this.almacenador.EditarArchivo(contenido, extension, Carpeta, archivos.Imagen.ContentType, rutaActual);
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




        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string Carpeta = "imageneseventos";
                string RutaActual = "";

                var ev = await this.context.CpEventos.Where(e => e.IdEvento == id).FirstOrDefaultAsync();
                if (ev != null)
                {
                    this.context.Remove(ev);
                    await this.context.SaveChangesAsync();
                    if (ev.Imagen != "")
                    {
                        RutaActual = ev.Imagen;
                        await this.almacenador.BorrarArchivo(RutaActual, Carpeta);
                    }

                    return new JsonResult("El evento se eliminó con éxito.");
                }
                else {
                    return BadRequest("Algo salió mal al eliminar el evento.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " POST Revise el metodo");
            }
        }
    }
}
