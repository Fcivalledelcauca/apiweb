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

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/documentospublicos")]
    public class DocumentosPublicosController : ControllerBase
    {
        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;

        public DocumentosPublicosController(WebSIDataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult> Documentos([FromBody] filtroDocs doc)
        {
            try
            {
                int grupo = Convert.ToInt32(doc.IdGrupo);

                var ctaGrupos = await this.context.DfDocumentos.Where(d=>d.CodMunicipio==doc.NomMunicipio && d.IdGrupo==grupo).ToListAsync();
                return new JsonResult(ctaGrupos);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }

    }
    public class filtroDocs
    {
        public string NomMunicipio { get; set; }
        public string IdGrupo { get; set; }
    }
}

