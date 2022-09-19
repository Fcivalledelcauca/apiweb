using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/reportes")]
    public class ReportesController : Controller
    {

        private readonly WebSIDataContext context;

        public ReportesController(WebSIDataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                string query = " ;with examen as (" +
                    " select IdExamenPresentado,EP.Usuario,Ex.IdTema,Ex.Nombre as nomexamen,pg.IdPregunta,pg.Pregunta,count(Can_Ok) as Can_Ok,count(Can_Fail) as Can_Fail " +
                    " from cp_ExamenPresentado as EP " +
                    "  left join cp_Examenes as Ex on Ex.IdExamen=EP.IdExamen" +
                    "  left join cp_Preguntas as pg on pg.IdExamen=EP.IdExamen" +
                    " group by EP.IdExamenPresentado, EP.Usuario,EX.IdTema,EX.Nombre,pg.Pregunta,pg.IdPregunta)" +
                    " select " +
                    "  0 as Reg" +
                    " ,ISNULL((select top 1 municipio from mm_PerfilUsuario as p where p.Usuario=examen.Usuario),'') AS municipio" +
                    " ,ISNULL((select top 1 Tema from cp_Temas as t where t.IdTema=IdTema),'') AS guia" +
                    " ,Usuario" +
                    " ,nomexamen" +
                    " ,Pregunta" +
                    " ,IdExamenPresentado" +
                    " ,ISNULL((select top 1 respuesta " +
                    "   from cp_RespuestasPresentadas_tmp as rpt" +
                    "   left join cp_Respuestas as res on res.IdRespuesta=rpt.IdRespuesta" +
                    "   where rpt.IdPregunta=examen.IdPregunta and rpt.IdExamenPresentado=examen.IdExamenPresentado),'') as respuesta" +
                    " ,ISNULL((select top 1 puntos" +
                    "   from cp_RespuestasPresentadas_tmp as rpt " +
                    "   left join cp_Respuestas as res on res.IdRespuesta=rpt.IdRespuesta" +
                    "   where rpt.IdPregunta=examen.IdPregunta and rpt.IdExamenPresentado=examen.IdExamenPresentado),'') as Puntos" +
                    " from examen" +
                    " order by nomexamen,IdExamenPresentado";

                //var userSuppliedSearchTerm = ".NET";
                var consulta =this.context.CpReporteExamenPresentados.FromSqlRaw("EXECUTE dbo.sp_ConsultaReporteCuestionario");


                return new JsonResult(consulta);
            }
            catch (Exception ex)
            {
                return BadRequest("Inconsitencias en el index" + ex.Message);
            }

        }
    }
}
