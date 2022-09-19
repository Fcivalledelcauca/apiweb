using sinecoserveract.Entidades;
using sinecoserveract.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using sinecoserveract.Funciones;
using sinecoserveract.Utilidades;
using sinecoserveract.DTOs;
using System.Text.Json;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {

        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;

        public UsuariosController(WebSIDataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }

        [HttpPost("registro")]
        public async Task<ActionResult> registro(User user)
        {
            try
            {
                var SiExiste = await this.context.MmPerfilUsuarios.AnyAsync(p => p.NombreCompleto == user.NombreCompleto || p.Email == user.Email);

                if (SiExiste)
                {
                    return BadRequest("El Usuario ya Existe.");
                }

                //crear el usuario del sistema
                MmUsuariosSistema usersis = new MmUsuariosSistema();
                usersis.NombreUsuario = user.Email;
                usersis.ClaveUsuario = Hash(user.Password);
                usersis.Activo = true;
                this.context.MmUsuariosSistemas.Add(usersis);

                var res = await this.context.SaveChangesAsync();

                //se verifica que el usuario del sistema se haya creado correctamente
                var usuarioCreado = await this.context.MmUsuariosSistemas.Where(u => u.NombreUsuario == user.Email).FirstOrDefaultAsync();
                if (usuarioCreado != null)
                {
                    //crear el rol del usuario
                    MmRolesUsuario rol = new MmRolesUsuario();
                    rol.TipoRol = user.TipoRol==""? user.TipoRol: "PARTICIPANTE";
                    rol.IdUsuario = usuarioCreado.IdUsuario;
                    rol.IdCia = 0;
                    rol.IdTpv = "0";
                    this.context.Add(rol);

                    //creación del perfil del usuario
                    if (user.NombreCompleto != "" && user.DocumentoIdentidad != "" && user.Municipio != "") {
                        MmPerfilUsuario perfil = new MmPerfilUsuario();
                        perfil.IdCia = 0;
                        perfil.IdCO = "";
                        perfil.NombreCompleto = user.NombreCompleto;
                        perfil.DocumentoIdentidad = user.DocumentoIdentidad;
                        perfil.Telefono = user.Telefono;
                        perfil.Direccion = user.Direccion;
                        perfil.Email = user.Email;
                        perfil.IdMunicipio = user.IdMunicipio;
                        perfil.Municipio = user.Municipio;
                        perfil.Usuario = usuarioCreado.NombreUsuario;
                        perfil.Imagen = "";
                        this.context.Add(perfil);
                    }
                    //se guarda la información 
                    await this.context.SaveChangesAsync();
                    SendMail send = new SendMail(this.context);
                    bool respuesta = await send.SednEmail(user.Email, "Bienvenido participante FCI Valle del Cauca", "bienvenida");
                    if (!respuesta) { 
                    
                        return BadRequest("Error con el envió del correo");
                    }

                    return new JsonResult("Usuario Registrado con éxito la confirmación se envió a su correo");

                }
                else {
                    return BadRequest("Algo salio Mal!");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Algo salio Mal!" + ex.Message);
            }
        }

        [HttpPut("actualizar")]
        public async Task<ActionResult> Actualizar(User user)
        {
            try
            {
                string email = "";
                var userSistema = await this.context.MmUsuariosSistemas.FirstOrDefaultAsync(u => u.NombreUsuario == user.NombreUsuario);
                if (userSistema != null)
                {
                    email = userSistema.NombreUsuario;
                    userSistema.NombreUsuario = user.Email;
                    userSistema.Activo = user.activo;

                    this.context.Entry(userSistema).State = EntityState.Modified;
                    //modificar el rol
                    var ctaRol = await this.context.MmRolesUsuarios.FirstOrDefaultAsync(r => r.IdUsuario == userSistema.IdUsuario);
                    if (ctaRol != null)
                    {
                        ctaRol.TipoRol = user.TipoRol;
                        this.context.Entry(ctaRol).State = EntityState.Modified;
                    }

                    //modificar el perfil
                    var ctaPerfil = await this.context.MmPerfilUsuarios.FirstOrDefaultAsync(p => p.Email == email);
                    if (ctaPerfil != null)
                    {
                        ctaPerfil.Email = user.Email;
                        ctaPerfil.Usuario = user.Email;
                        ctaPerfil.IdMunicipio = user.IdMunicipio;
                        ctaPerfil.Municipio = user.Municipio;
                    }
                    await this.context.SaveChangesAsync();
                    SendMail send = new SendMail(this.context);
                    bool res = await send.SednEmail(email, "Actualización usuario FCI Valle del Cauca", "cambiorol");
                    if (res)
                    {
                        return new JsonResult(user);
                    }
                    else
                    {
                        return BadRequest("Error con el envió del correo");
                    }
                    
                }
                else
                {
                    return BadRequest("El Usuario no se encontro");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salio Mal!" + ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(User usuario)
        {
            User userl = new User();
            var resultadoTemp = new RespuestaAutenticacion();
            var resultado = new RespuestaAutenticacion();
            try
            {

                var pass = await this.context.MmUsuariosSistemas.Where(usu => usu.NombreUsuario == usuario.Email && usu.Activo== true).FirstOrDefaultAsync();
                if (pass != null)
                {
                    string clave = Hash(usuario.Password);
                    //var user = await this.context.MmUsuariosSistemas.Where(usu => usu.NombreUsuario == pass.NombreUsuario && usu.ClaveUsuario == clave).FirstOrDefaultAsync();
                    var user =await (from userlog in this.context.MmUsuariosSistemas
                               join roles in this.context.MmRolesUsuarios on userlog.IdUsuario equals roles.IdUsuario
                               where userlog.NombreUsuario == pass.NombreUsuario  &&  userlog.ClaveUsuario== clave
                                     select new {
                                   email=userlog.NombreUsuario,
                                   password=userlog.ClaveUsuario,
                                   rol=roles.IdRolUsuario,
                                   tiporol=roles.TipoRol
                               }).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        userl.Email = user.email;
                        userl.Password = user.password;
                        userl.idRol = user.rol.ToString();
                        userl.TipoRol = user.tiporol;
                        resultado = await ConstruirToken(userl);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Usuario no Existe " + ex.InnerException);
            }
            //return resultado.Ok ? new JsonResult(resultado): BadRequest("error " + new JsonResult(resultado)) ;
            return  new JsonResult(resultado);
        }

        [HttpGet("validate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Validar()
        {

            var resultado = new RespuestaAutenticacion();
            User userl = new User();
            try
            {
                //con el jwt que viene en el autorize sabemos si es un usuario autorizado
                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault();
                //foreach (var claim in emailClaim)
                //{
                userl.Email = emailClaim.Value;
                userl.TipoRol = await this.ConsultarRol(emailClaim.Value);
                resultado = await this.ConstruirToken(userl);

                //}

            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex.Message);
            }

            return new JsonResult(resultado);
        }
       
        [HttpGet("users")]
        public async Task<ActionResult> GetUsuarios([FromQuery] PaginacionDTO paginacionDTO) 
        {
            try
            {
                var queryable = (from usuarios in this.context.MmUsuariosSistemas
                                 join roles in this.context.MmRolesUsuarios on usuarios.IdUsuario equals roles.IdUsuario
                                 join perfil in this.context.MmPerfilUsuarios on usuarios.NombreUsuario equals perfil.Usuario
                                 select new
                                 {
                                     nombreUsuario = usuarios.NombreUsuario,
                                     nombreCompleto = perfil.NombreCompleto,
                                     email = perfil.Email,
                                     idmunicipio = perfil.IdMunicipio,
                                     municipio = perfil.Municipio,
                                     tiporol = roles.TipoRol,
                                     activo = usuarios.Activo

                                 }).AsQueryable();
                var cantidPaginas = await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidaddeRegistroPorPagina);
                var cta = await queryable.Paginar(paginacionDTO).ToListAsync();
                RespuestaPaginacion resP = new RespuestaPaginacion()
                {
                    CantidadPaginas = cantidPaginas,
                    Respuesta = JsonSerializer.Serialize(cta)
                };
                return new JsonResult(resP);

            }
            catch (Exception ex)
            {
                return BadRequest("error" + ex.Message);
            }
        }

        [HttpGet("{email}")]
        public async Task<ActionResult> ValidatorEmail(string email) {
            try
            {
                var existe = await this.context.MmUsuariosSistemas.AnyAsync(u => u.NombreUsuario.ToLower() == email.ToLower());
                if (existe)
                {
                    return new JsonResult("El Email ya Existe en la Base de Datos.");
                }
                else {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {

                return BadRequest("Error en get validator " + ex.Message);
            }
        }   
        
        private async Task<RespuestaAutenticacion> ConstruirToken(User user)
        {
            var usuario = await this.context.MmUsuariosSistemas.AnyAsync(u => u.NombreUsuario == user.Email);
            if (usuario)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("rol", await this.ConsultarRol(user.Email))
                    
                };

                var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["llavejwt"]));
                var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

                var expiracion = DateTime.UtcNow.AddDays(1);
                var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                    expires: expiracion, signingCredentials: creds);

                return new RespuestaAutenticacion()
                {
                    Ok = true,
                    Email = user.Email,
                    Rol= user.TipoRol,
                    Menu =await this.MenuNavegacion(user.Email,user.TipoRol),
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Expiracion = expiracion
                };
            }
            else
            {
                return new RespuestaAutenticacion()
                {
                    Ok = false,
                    Email = null,
                    Rol=null,
                    Menu=null,
                    Token = null,
                    Expiracion = DateTime.Now
                };
            }
        }

        private async Task<string> ConsultarRol(string email)
        {
            string res = "";
            try
            {
                var rol = await (from users in this.context.MmUsuariosSistemas
                           join croles in this.context.MmRolesUsuarios on users.IdUsuario equals croles.IdUsuario
                           where users.NombreUsuario.ToLower()==email.ToLower()
                           select new
                           {
                               rol = croles.TipoRol
                           }).FirstOrDefaultAsync();
                if (rol != null)
                {
                    res = rol.rol;
                }
            }
            catch (Exception ex)
            {
                res = "";
            }
            return res;
        }

        private string Hash(string password)
        {

            UTF8Encoding enc = new UTF8Encoding();

            byte[] data = enc.GetBytes(password);

            byte[] hashresult;

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            hashresult = sha.ComputeHash(data);
            return Convert.ToBase64String(hashresult);
        }

        public async Task<List<Menu>> MenuNavegacion(string email,string roluser)
        {
            List<Menu> res = new List<Menu>();

            int order1 = 1;
            int order2 = 1;

            try
            {
                //string rol = await this.ConsultarRol(email) != "" ? await this.ConsultarRol(email) : "";
                string rol = roluser;
                if (rol != "")
                {
                    var query =await this.context.MmPermisosRoles.Where(p => p.TipoRol == rol)
                                    .GroupBy(mm => mm.IdModulo)
                                    .Select(q => new { idModulo = q.Key }).ToListAsync();
                    foreach (var i in query)
                    {
                        List<child> children = new List<child>();
                        Menu LMenu = new Menu();
                        var menu = (from modulos in this.context.MmModulos
                                    where modulos.IdModulo == i.idModulo
                                    select new
                                    {
                                        idmodulo = modulos.IdModulo,
                                        menu = modulos.Title,
                                        icon = modulos.Icon,
                                    }).FirstOrDefault();

                        LMenu.menu = menu.menu;
                        LMenu.link = "";
                        LMenu.icon = menu.icon;
                        LMenu.order = order1;
                        var opciones = this.context.MmPermisosRoles.Where(op => op.IdModulo == i.idModulo && op.TipoRol==rol).ToList();
                        foreach (var opcion in opciones)
                        {
                            var op = this.context.MmOpcionesModulos.Where(o => o.IdOpciones == opcion.IdOpciones).FirstOrDefault();
                            child opc = new child();
                            opc.submenu = op.Title;
                            opc.link = op.Link;
                            opc.order = order2;
                            children.Add(opc);
                            order2++;
                        }
                        LMenu.child = children;
                        order1++;

                        res.Add(LMenu);
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return res;
        }

    }

  
}
