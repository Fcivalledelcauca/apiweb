using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class PresentarExamenDisplay
    {
        public int IdExamen { get; set; }
        public int IdTema { get; set; }
        public string Nombre { get; set; }
        public decimal Puntuacion { get; set; }
        public string Instrucciones { get; set; }
        public bool Activo { get; set; }
        public List<Preguntas> Preguntas { get; set; }
    }

    public class Preguntas
    {
        public int IdPregunta { get; set; }
        public int IdExamen { get; set; }
        public string Pregunta { get; set; }
        public decimal Puntos { get; set; }
        public List<Respuestas> Respuestas { get; set; }
    }

    public class Respuestas
    {
        public int IdRespuesta { get; set; }
        public int IdPregunta { get; set; }
        public string Respuesta { get; set; }
        public int Correcta { get; set; }
    }
}
