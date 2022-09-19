using AutoMapper;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Utilidades
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PerfilUsuarioDisplay, MmPerfilUsuario>()
                    .ForMember(x => x.Imagen, option => option.Ignore());

            CreateMap<EventosDisplay, CpEventos>();

            CreateMap<MunicipioDisplay, MmMunicipio>();

            CreateMap<TemasDisplay, CpTemas>();

            CreateMap<TemariosDisplay, CpTemarios>();

            CreateMap<MunicipioTemaDisplay, CpMunicipioTema>();

            CreateMap<EventoArchivoDisplay, CpEventoArchivo>();

            CreateMap<ExamenDisplay, CpExamen>();

            CreateMap<RespuestaPresentadaDisplay, CpRespuestaPresentada>();

            CreateMap<TaberoDisplay, TcTablero>();

            CreateMap<BaseMdmDisplay, TcBaseMdm>();

            CreateMap<BaseIdfDisplay, TcBaseIdf>();

            CreateMap<BaseIdf2Display, TcBaseIdf2>();

            CreateMap<BaseIdiDisplay, TcBaseIdi>();

            CreateMap<PromedioDimDisplay, TcPromedioDimensione>();

            CreateMap<ResumenMdmDisplay, TcResumenMdm>();


        }
    }
}
