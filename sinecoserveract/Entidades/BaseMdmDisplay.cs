using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class BaseMdmDisplay
    {
        public int? IdTablero { get; set; }
        public string Lote { get; set; }
        public string CodigoDane { get; set; }
        public string Grupo { get; set; }
        public decimal? MovilizacionRecursos { get; set; }
        public decimal? EjecucionRecursos { get; set; }
        public decimal? GobiernoAbierto { get; set; }
        public decimal? OrdenamientoTerritorial { get; set; }
        public decimal? Gestion { get; set; }
        public decimal? Educacion { get; set; }
        public decimal? Salud { get; set; }
        public decimal? ServiciosPublicos { get; set; }
        public decimal? SeguridadConvivencia { get; set; }
        public decimal? Resultados { get; set; }
        public decimal? AjusteResultados { get; set; }
        public decimal? Mdm { get; set; }
        public string RankingMdm { get; set; }
        public string RankingMov { get; set; }
        public string RankingEjecu { get; set; }
        public string RankingGob { get; set; }
        public string RankingOt { get; set; }
        public string RankingGestion { get; set; }
        public string RankingEdu { get; set; }
        public string RankingSalud { get; set; }
        public string RankingServPub { get; set; }
        public string RankingSeguridad { get; set; }
        public string RankingResultados { get; set; }
        public bool? Activo { get; set; }
    }
}
