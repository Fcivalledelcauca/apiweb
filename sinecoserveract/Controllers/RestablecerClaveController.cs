using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sinecoserveract.Funciones;
using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("api/restablecer")]
    public class RestablecerClaveController : ControllerBase
    {
        private readonly WebSIDataContext context;

        public RestablecerClaveController(WebSIDataContext context)
        {
            this.context = context;
        }

        //RECUPERAR CONTRASEÑAS-----------------
        [HttpPatch("{email}")]
        public async Task<ActionResult> RecuperarClave(string email)
        {
            try
            {
                var usuario = await this.context.MmUsuariosSistemas.FirstOrDefaultAsync(u => u.NombreUsuario == email);
                if (usuario != null)
                {
                    usuario.ClaveUsuario = this.Hash("12345678");
                    this.context.Entry(usuario).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    SendMail send = new SendMail(this.context);
                    bool res = await send.SednEmail(email, "Solicitud cambio de clave FCI Valle del Cauca", "recuperar");
                    if (res)
                    {
                        return new JsonResult("Revise la bandeja de entrada de su correo electrónico para la recuperación de la clave");
                    }
                    else
                    {
                        return BadRequest("Error con el envió del correo");
                    }

                }
                else
                {
                    return NotFound("No se Encontró el Email");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error en Patch " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarClaveC([FromBody] ActualizarClave user)
        {
            var usuario = await this.context.MmUsuariosSistemas.FirstOrDefaultAsync(u => u.NombreUsuario == user.Email && u.ClaveUsuario.Contains(user.Hash));
            if (usuario != null)
            {
                usuario.ClaveUsuario = Hash(user.Clave);
                usuario.Activo = true;
                this.context.Entry(usuario).State = EntityState.Modified;
                await this.context.SaveChangesAsync();
                return new JsonResult("La clave ha sido actualizada con éxito.");
            }
            else
            {
                return NotFound("El Email no es valido.");
            }
        }
        //----------------------------------------
        private string Hash(string password)
        {

            UTF8Encoding enc = new UTF8Encoding();

            byte[] data = enc.GetBytes(password);

            byte[] hashresult;

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            hashresult = sha.ComputeHash(data);
            return Convert.ToBase64String(hashresult);
        }
    }

    

    public class ActualizarClave
    {
        public string Email { get; set; }
        public string Clave { get; set; }
        public string Hash { get; set; }
    }
}
