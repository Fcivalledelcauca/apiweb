using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("/api/temario")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TemarioController : ControllerBase
    {

        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IMapper maper;

        public TemarioController(WebSIDataContext context, IConfiguration configuration, IMapper maper)
        {
            this.context = context;
            this.Configuration = configuration;
            this.maper = maper;
        }


        // GET: TemarioController
        [HttpGet]
        public async Task<ActionResult<List<CpTemarios>>> Index()
        {
            try
            {
                List<CpTemarios> temarios = await this.context.CpTemarios.ToListAsync();
                if (temarios != null)
                {
                    return temarios;
                }
                else {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }


        // GET: TemarioController/Details/5
        [HttpGet("{idtema}")]
        public async Task<ActionResult<List<CpTemarios>>> TemarioId(int idtema)
        {
            try
            {
                List<CpTemarios> temarios = await this.context.CpTemarios.Where(t => t.IdTema == idtema).ToListAsync();
                if (temarios != null)
                {
                    return temarios;
                }
                else {
                    return NotFound("No se encontraron el temarios.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error getId: " + ex.Message);
            }
        }

        // GET: TemarioController/Create
        [HttpPost]
        public async Task<ActionResult> Post(TemariosDisplay temario)
        {
            try
            {
                var existe = await this.context.CpTemarios.AnyAsync(t => t.IdTema == temario.IdTema && t.Temario == temario.Temario);
                if (!existe)
                {
                    var temar = maper.Map<CpTemarios>(temario);
                    this.context.Add(temar);
                    await this.context.SaveChangesAsync();
                    return new JsonResult("El temario se creó con éxito.");
                }
                else {
                    return BadRequest("El temario ya existe.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Post: " + ex.Message);
            }
        }



        // GET: TemarioController/Edit/5
        [HttpPut]
        public async Task<ActionResult> Put(CpTemarios temario)
        {
            try
            {
                CpTemarios tem = await this.context.CpTemarios.Where(t => t.IdTemario == temario.IdTemario).FirstOrDefaultAsync();
                if (tem != null)
                {
                    tem.Temario = temario.Temario;
                    tem.Activo = temario.Activo;
                    this.context.Entry(tem).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    return new JsonResult($"El temario {temario.Temario} se modificó con éxito.");
                }
                else {
                    return NotFound("No se encontró el temario.");
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
                CpTemarios temario = await this.context.CpTemarios.FirstOrDefaultAsync(t => t.IdTemario == id);
                if (temario != null)
                {
                    this.context.Remove(temario);
                    await this.context.SaveChangesAsync();
                    return new JsonResult($"El temario se eliminó con éxito.");
                }
                else {
                    return NotFound("El temario no existe.");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Delete: " + ex.Message);
            }
        }

      
    }
}
