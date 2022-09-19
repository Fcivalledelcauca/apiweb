using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace sinecoserveract.Models
{
    public partial class WebSIDataContext : DbContext
    {

        private readonly IConfiguration Configuration;
        public WebSIDataContext()
        {
        }

        public WebSIDataContext(DbContextOptions<WebSIDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MmRolesUsuario> MmRolesUsuarios { get; set; }
        public virtual DbSet<MmTipoRolesUsuario> MmTipoRolesUsuarios { get; set; }
        public virtual DbSet<MmUsuariosSistema> MmUsuariosSistemas { get; set; }
        public virtual DbSet<MmModulo> MmModulos { get; set; }
        public virtual DbSet<MmOpcionesModulo> MmOpcionesModulos { get; set; }
        public virtual DbSet<MmPermisosRole> MmPermisosRoles { get; set; }
        public virtual DbSet<MmPerfilUsuario> MmPerfilUsuarios { get; set; }
        public virtual DbSet<DfGrupos> DfGrupos { get; set; }
        public virtual DbSet<DfDocumentos> DfDocumentos { get; set; }
        public virtual DbSet<CpEventos> CpEventos { get; set; }
        public virtual DbSet<MmMunicipio> MmMunicipios { get; set; }
        public virtual DbSet<CpTemas> CpTemas { get; set; }
        public virtual DbSet<CpTemarios> CpTemarios { get; set; }
        public virtual DbSet<CpEventoArchivo> CpEventoArchivo  { get; set; }
        public virtual DbSet<CpMunicipioTema> CpMunicipioTema { get; set; }
        public virtual DbSet<CpExamen> CpExamen { get; set; }
        public virtual DbSet<CpPregunta> CpPregunta { get; set; }
        public virtual DbSet<CpRespuesta> CpRespuesta { get; set; }

        public virtual DbSet<CpExamenPresentado> CpExamenPresentado { get; set; }

        public virtual DbSet<CpRespuestaPresentada> CpRespuestaPresentada { get; set; }

        public virtual DbSet<MmMensaje> MmMensaje { get; set; }
        public virtual DbSet<TcBaseIdf> TcBaseIdfs { get; set; }
        public virtual DbSet<TcBaseIdf2> TcBaseIdf2s { get; set; }
        public virtual DbSet<TcBaseIdi> TcBaseIdis { get; set; }
        public virtual DbSet<TcBaseMdm> TcBaseMdms { get; set; }
        public virtual DbSet<TcTablero> TcTableros { get; set; }

        public virtual DbSet<TcPromedioDimensione> TcPromedioDimensiones { get; set; }
        public virtual DbSet<TcResumenMdm> TcResumenMdms { get; set; }
        public virtual DbSet<CpReporteExamenPresentado> CpReporteExamenPresentados { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("sineco"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");


            modelBuilder.Entity<MmRolesUsuario>(entity =>
            {
                entity.HasKey(e => e.IdRolUsuario);

                entity.ToTable("mm_Roles_Usuarios");

                entity.Property(e => e.IdCo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IdCO");

                entity.Property(e => e.IdTpv)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<MmTipoRolesUsuario>(entity =>
            {
                entity.HasKey(e => e.TipoRol);

                entity.ToTable("mm_TipoRoles_Usuario");

                entity.Property(e => e.TipoRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<MmUsuariosSistema>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("mm_UsuariosSistema");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.ClaveUsuario)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdCia).HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTercero).HasDefaultValueSql("((0))");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MmModulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo);

                entity.ToTable("mm_Modulos");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icon)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("icon")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("id")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Orden).HasDefaultValueSql("((0))");

                entity.Property(e => e.Subtitle)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("subtitle")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("type")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MmOpcionesModulo>(entity =>
            {
                entity.HasKey(e => e.IdOpciones);

                entity.ToTable("mm_Opciones_Modulos");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Icon)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("icon")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("id")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Link)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("link")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreControl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreOpcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("type")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MmPermisosRole>(entity =>
            {
                entity.HasKey(e => e.IdPermisoRole);

                entity.ToTable("mm_Permisos_Roles");

                entity.Property(e => e.IdCo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IdCO");

                entity.Property(e => e.IdTpv)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoRol)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MmPerfilUsuario>(entity =>
            {
                entity.HasKey(e => e.IdPerfil);

                entity.ToTable("mm_PerfilUsuario");

                entity.Property(e => e.IdCia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdMunicipio)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.IdCO)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DocumentoIdentidad)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Municipio)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasDefaultValueSql("('')");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Imagen)
                 .HasMaxLength(500)
                 .IsUnicode(false)
                 .HasDefaultValueSql("('')");

            });

            modelBuilder.Entity<DfGrupos>(entity =>
            {
                entity.HasKey(e => e.IdGrupo);

                entity.ToTable("Df_Grupos");

                entity.Property(e => e.Orden)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdCia)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.Nombre_Grupo)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icono)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClaseCSS)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo)
                 .HasMaxLength(50)
                 .IsUnicode(false)
                 .HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<DfDocumentos>(entity =>
            {
                entity.HasKey(e => e.IdDocumento);

                entity.ToTable("Df_Documentos");

                entity.Property(e => e.IdGrupo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdCia)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CodMunicipio)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreArchivo)
                   .HasMaxLength(500)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Archivo)
                   .HasMaxLength(500)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo)
                 .HasMaxLength(50)
                 .IsUnicode(false);

            });

            modelBuilder.Entity<CpEventos>(entity =>
            {
                entity.HasKey(e => e.IdEvento);

                entity.ToTable("cp_Eventos");

                entity.Property(e => e.IdMunicipio)
                 .HasMaxLength(50)
                 .IsUnicode(false);

                entity.Property(e => e.IdCia)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fecha_Ini)
                   .HasColumnType("smalldatetime")
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Fecha_Fin)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TodoElDia)
                   .HasDefaultValueSql("(1)");

                entity.Property(e => e.ColorEvento)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");

                entity.Property(e => e.Imagen)
                 .HasMaxLength(500)
                 .IsUnicode(false)
                 .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreArchivo)
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasDefaultValueSql("('')");

                entity.Property(e => e.Direccion)
                      .HasMaxLength(200)
                      .IsUnicode(false)
                      .HasDefaultValueSql("('')");

                entity.Property(e => e.Link)
                       .HasMaxLength(500)
                       .IsUnicode(false)
                       .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo)
                .HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<CpEventoArchivo>(entity =>
            {
                entity.HasKey(e => e.IdArchivo);

                entity.ToTable("cp_Eventos_Archivos");

                entity.Property(e => e.IdEvento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdTema)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.IdTemario)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NombreArchivo)
               .HasMaxLength(500)
               .IsUnicode(false)
               .HasDefaultValueSql("('')");

                entity.Property(e => e.Archivo)
                 .HasMaxLength(500)
                 .IsUnicode(false)
                 .HasDefaultValueSql("('')");


            });

            modelBuilder.Entity<MmMunicipio>(entity =>
            {
                entity.HasKey(e => e.IdMunicipio);

                entity.ToTable("mm_Municipio");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.CodigoDane)
                   .HasMaxLength(10)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Region)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Categoria_Ruralidad)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasDefaultValueSql("('')");

                entity.Property(e => e.Dotaciones_Iniciales)
                  .HasMaxLength(10)
                  .IsUnicode(false)
                  .HasDefaultValueSql("('')");

                entity.Property(e => e.Grupo_Par)
                 .HasMaxLength(100)
                 .IsUnicode(false)
                 .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<CpTemas>(entity =>
            {
                entity.HasKey(e => e.IdTema);

                entity.ToTable("cp_Temas");

                entity.Property(e => e.IdMunicipio).HasDefaultValueSql("((0))");

                entity.Property(e => e.Tema)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<CpTemarios>(entity =>
            {
                entity.HasKey(e => e.IdTemario);

                entity.ToTable("cp_Temarios");

                entity.Property(e => e.IdTema).HasDefaultValueSql("((0))");

                entity.Property(e => e.Temario)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<CpMunicipioTema>(entity =>
            {
                entity.HasKey(e => e.IdMunicipioTema);

                entity.ToTable("cp_MunicipioTema");

                entity.Property(e => e.IdMunicipio).HasDefaultValueSql("((0))");
                entity.Property(e => e.IdTema).HasDefaultValueSql("((0))");
                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<CpExamen>(entity =>
            {
                entity.HasKey(e => e.IdExamen);

                entity.ToTable("cp_Examenes");

                entity.Property(e => e.IdTema).HasDefaultValueSql("((0))");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Puntuacion)
                   .IsUnicode(false)
                   .HasDefaultValueSql("(0)");

                entity.Property(e => e.Instrucciones)
                   .HasMaxLength(500)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<CpPregunta>(entity =>
            {
                entity.HasKey(e => e.IdPregunta);

                entity.ToTable("cp_Preguntas");

                entity.Property(e => e.IdExamen).HasDefaultValueSql("((0))");

                entity.Property(e => e.Pregunta)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Puntos).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<CpRespuesta>(entity =>
            {
                entity.HasKey(e => e.IdRespuesta);

                entity.ToTable("cp_Respuestas");

                entity.Property(e => e.IdPregunta).HasDefaultValueSql("((0))");

                entity.Property(e => e.Respuesta)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Correcta).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<CpExamenPresentado>(entity =>
            {
                entity.HasKey(e => e.IdExamenPresentado);

                entity.ToTable("cp_ExamenPresentado");
                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.IdExamen).HasDefaultValueSql("((0))");
                entity.Property(e => e.PuntosMin).HasDefaultValueSql("((0))");
                entity.Property(e => e.Puntuacion).HasDefaultValueSql("((0))");
                entity.Property(e => e.Can_Ok).HasDefaultValueSql("((0))");
                entity.Property(e => e.Can_Fail).HasDefaultValueSql("((0))");
                entity.Property(e => e.Aprobo).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<CpRespuestaPresentada>(entity =>
            {
                entity.HasKey(e => e.IdRespuestaPresentada);

                entity.ToTable("cp_RespuestasPresentadas");
                entity.Property(e => e.IdExamenPresentado).HasDefaultValueSql("((0))");
                entity.Property(e => e.IdRespuesta).HasDefaultValueSql("((0))");
                entity.Property(e => e.Puntos).HasDefaultValueSql("((0))");
  

            });

            modelBuilder.Entity<MmMensaje>(entity =>
            {
                entity.HasKey(e => e.IdMensaje);

                entity.ToTable("mm_Mensajes");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdMunicipio).HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Telefono)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Mensaje)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TC).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<CpReporteExamenPresentado>(entity =>
            {
                entity.HasKey(e => e.Reg);

                entity.ToTable("cp_ReporteExamenPresentados");

                entity.Property(e => e.IdExamenPresentado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Municipio)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Guia)
                   .HasMaxLength(300)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.NomExamen)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");


                entity.Property(e => e.Pregunta)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Respuesta)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Puntos).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<TcBaseIdf>(entity =>
            {
                entity.HasKey(e => e.IdBaseIdf);

                entity.ToTable("tc_BaseIDF");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.BonificacionEsfuerzoPropio)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Bonificacion_Esfuerzo_Propio")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaAhorroCorriente)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Ahorro_Corriente")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaBalancePrimario)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Balance_Primario")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaCapacidadEjecucionIngresos)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Capacidad_Ejecucion_Ingresos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.caCapacidadEjecucioInversion)
                   .HasColumnType("decimal(18, 8)")
                   .HasColumnName("Ca_CapacidadEjecucioInversion")
                   .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaDependenciaTransferencia)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Dependencia_Transferencia")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaEndeudamientoLargoPlazo)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Endeudamiento_LargoPlazo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaGestion)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Gestion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaHolgura)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Holgura")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaRelevanciaFbkFijo)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Relevancia_FBK_fijo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaResultado)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Resultado")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CodigoDane)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gestion)
                    .HasColumnType("decimal(18, 8)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GestionBonos)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Gestion_Bonos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTablero).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lote)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Municipio)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NuevoIdf)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Nuevo_IDF")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NuevoIdfSb)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Nuevo_IDF_SB")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rango)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Resultado)
                    .HasColumnType("decimal(18, 8)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TcBaseIdf2>(entity =>
            {
                entity.HasKey(e => e.IdBaseIdf2);

                entity.ToTable("tc_BaseIDF2");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.AhorroCorriente)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ahorro_Corriente")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BalancePrimario)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Balance_Primario")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BonificacionEsfuerzoPropio)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Bonificacion_Esfuerzo_Propio")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaCapacidadEjecucionIngresos)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Capacidad_Ejecucion_Ingresos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaCapacidadEjecucionInversion)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Capacidad_Ejecucion_Inversion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaGestion)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Gestion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CaHolgura)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ca_Holgura")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DependenciaTransferencia)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Dependencia_Transferencia")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EndeudamientoLargoPlazo)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Endeudamiento_LargoPlazo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdTablero).HasDefaultValueSql("((0))");

                entity.Property(e => e.Idf)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("IDF")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lote)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RelevanciaFbkFijo)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Relevancia_FBK_Fijo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Resultados)
                    .HasColumnType("decimal(18, 8)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TcBaseIdi>(entity =>
            {
                entity.HasKey(e => e.IdBaseIdi);

                entity.ToTable("tc_Base_IDI");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.CodigoDane)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.D1TalentoHumano)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("D1_TalentoHumano")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D2DireccionamientoEstrategicoPlaneacion)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("D2_Direccionamiento_Estrategico_Planeacion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D3GestionResultadosValores)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("D3_Gestion_Resultados_Valores")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D4EvaluacionResultados)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("D4_Evaluacion_Resultados")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D5InformacionComunicacion)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("D5_Informacion_Comunicacion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D6GestionConocimiento)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("D6_Gestion_Conocimiento")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D7ControlInterno)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("D7_Control_Interno")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTablero).HasDefaultValueSql("((0))");

                entity.Property(e => e.Idi)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("IDI")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lote)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Municipio)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.P10ServicioCiudadano)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P10_Servicio_Ciudadano")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P11RacionalizacionTramites)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P11_Racionalizacion_Tramites")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P12ParticipacionCiudadanaGestionPublica)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P12_Participacion_Ciudadana_Gestion_Publica")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P13SegumientoEvaluacionDesempeñoInstitucional)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P13_Segumiento_Evaluacion_Desempeño_Institucional")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P14GestionDocumental)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P14_Gestion_Documental")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P15GestionConocimiento)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P15_Gestion_Conocimiento")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P16ControlInterno)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P16_ControlInterno")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P17MejoraNormativa)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P17_MejoraNormativa")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P18GestionInformacionEstadistica)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P18_Gestion_Informacion_Estadistica")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P1GestionEstrategicaTalentoHumano)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P1_Gestion_Estrategica_Talento_Humano")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P2Integridad)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P2_Integridad")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P3PlaneacionInstitucional)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P3_PlaneacionInstitucional")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P4GestionPresupuestalEficienciaGastoPublico)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P4_Gestion_Presupuestal_Eficiencia_Gasto_Publico")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P5FortalecimientoOrganizacionalSimplificacionProcesos)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P5_Fortalecimiento_Organizacional_Simplificacion_Procesos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P6GobiernoDigital)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P6_Gobierno_Digital")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P7SeguridadDigital)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P7_Seguridad_Digital")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P8DefensaJuridica)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P8_Defensa_Juridica")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.P9TransparenciaAccesoInformacionLuchaContraCorrupcion)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("P9_Transparencia_Acceso_Informacion_Lucha_Contra_Corrupcion")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TcBaseMdm>(entity =>
            {
                entity.HasKey(e => e.IdBaseMdm);

                entity.ToTable("tc_BaseMDM");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.AjusteResultados)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ajuste_Resultados")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CodigoDane)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Educacion)
                    .HasColumnType("decimal(18, 8)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EjecucionRecursos)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ejecucion_Recursos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gestion)
                    .HasColumnType("decimal(18, 8)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GobiernoAbierto)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Gobierno_Abierto")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdTablero).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lote)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mdm)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("MDM")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MovilizacionRecursos)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Movilizacion_Recursos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrdenamientoTerritorial)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Ordenamiento_Territorial")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RankingEdu)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Edu")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingEjecu)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Ejecu")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingGestion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Gestion")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingGob)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Gob")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingMdm)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_MDM")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingMov)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Mov")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingOt)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Ot")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingResultados)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Resultados")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingSalud)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Salud")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingSeguridad)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Seguridad")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RankingServPub)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ranking_Serv_Pub")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Resultados)
                    .HasColumnType("decimal(18, 8)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Salud)
                    .HasColumnType("decimal(18, 8)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SeguridadConvivencia)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Seguridad_Convivencia")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ServiciosPublicos)
                    .HasColumnType("decimal(18, 8)")
                    .HasColumnName("Servicios_Publicos")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TcTablero>(entity =>
            {
                entity.HasKey(e => e.IdTablero);

                entity.ToTable("tc_Tablero");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Lote)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TcPromedioDimensione>(entity =>
            {
                entity.HasKey(e => e.IdPromedioDim)
                    .HasName("PK_tc_");

                entity.ToTable("tc_PromedioDimensiones");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.D1TalentoHumano)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("D1_TalentoHumano")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D2DireccionamientoEstrategicoPlaneacion)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("D2_DireccionamientoEstrategicoPlaneacion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D3GestionResultadosValores)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("D3_GestionResultadosValores")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D4EvaluacionResultados)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("D4_EvaluacionResultados")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D5InformacionComunicacion)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("D5_InformacionComunicacion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D6GestionConocimiento)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("D6_GestionConocimiento")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.D7Control)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("D7_Control")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Idi)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("IDI")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdTablero).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lote)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TcResumenMdm>(entity =>
            {
                entity.HasKey(e => e.IdResumen);

                entity.ToTable("tc_ResumenMDM");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Gestion)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mdm)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("MDM")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Resultado)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTablero).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lote)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
