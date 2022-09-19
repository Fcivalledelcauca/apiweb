using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/municipiotema")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MTController : ControllerBase
    {

        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IMapper maper;

        public MTController(WebSIDataContext context, IConfiguration configuration, IMapper maper)
        {
            this.context = context;
            this.Configuration = configuration;
            this.maper = maper;
        }

        // GET: TemarioController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var temasxm = await (from mt in this.context.CpMunicipioTema
                                     join m in this.context.MmMunicipios on mt.IdMunicipio equals m.IdMunicipio
                                     join t in this.context.CpTemas on mt.IdTema equals t.IdTema
                                     select new {
                                         IdMunicipioTema = mt.IdMunicipioTema,
                                         IdMunicipio = mt.IdMunicipio,
                                         IdTema = mt.IdTema,
                                         Municipio = m.Nombre,
                                         Tema = t.Tema,
                                         Activo = mt.Activo
                                     }).ToListAsync();
                if (temasxm != null)
                {
                    return new JsonResult(temasxm);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }


        // GET: TemarioController/Details/5
        [HttpGet("{idmunicipio}")]
        public async Task<ActionResult> TemasIdmunicipio(int idmunicipio)
        {
            try
            {
                var temasxm = await (from mt in this.context.CpMunicipioTema
                                     join m in this.context.MmMunicipios on mt.IdMunicipio equals m.IdMunicipio
                                     join t in this.context.CpTemas on mt.IdTema equals t.IdTema
                                     where mt.IdMunicipio==idmunicipio
                                     select new
                                     {
                                         IdMunicipioTema = mt.IdMunicipioTema,
                                         IdMunicipio = mt.IdMunicipio,
                                         IdTema = mt.IdTema,
                                         Municipio = m.Nombre,
                                         Tema = t.Tema,
                                         Activo = mt.Activo
                                     }).ToListAsync();
                if (temasxm != null)
                {
                    return new JsonResult(temasxm);
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
        public async Task<ActionResult> Post(MunicipioTemaDisplay municpiotemas)
        {
            try
            {
                var existe = await this.context.CpMunicipioTema.AnyAsync(t => t.IdTema == municpiotemas.IdTema && t.IdMunicipio == municpiotemas.IdMunicipio);
                if (!existe)
                {
                    var temar = maper.Map<CpMunicipioTema>(municpiotemas);
                    this.context.Add(temar);
                    await this.context.SaveChangesAsync();
                    return new JsonResult("El registro se creó con éxito.");
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
        public async Task<ActionResult> Put(CpMunicipioTema municipioTema)
        {
            try
            {
                CpMunicipioTema tem = await this.context.CpMunicipioTema.Where(t => t.IdMunicipioTema == municipioTema.IdMunicipioTema).FirstOrDefaultAsync();
                if (tem != null)
                {
                    tem.IdMunicipio = municipioTema.IdMunicipio;
                    tem.IdTema = municipioTema.IdTema;
                    tem.Activo = municipioTema.Activo;
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
                //revisar que este tema del muncipio no tenga material asociado
                var idtema = await this.context.CpMunicipioTema.Where(m => m.IdMunicipioTema == id).FirstOrDefaultAsync();
                if (idtema != null) { 
                    var ctaArchivos = await this.context.CpEventoArchivo.Where(e => e.IdTema == idtema.IdTema).ToListAsync();
                    if (ctaArchivos != null) {
                        if (ctaArchivos.Count > 0) {
                            return BadRequest("No se puede eliminar el tema. Existe material adjunto en los eventos");
                        }
                    }
                }



                //***************************************************************

                CpMunicipioTema mt = await this.context.CpMunicipioTema.FirstOrDefaultAsync(t => t.IdMunicipioTema == id);
                if (mt != null)
                {
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
