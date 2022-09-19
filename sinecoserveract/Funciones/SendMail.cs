using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using sinecoserveract.Models;
using sinecoserveract.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sinecoserveract.Funciones
{
    public class SendMail
    {
        private readonly WebSIDataContext context;

         public SendMail(WebSIDataContext context)
        {
            this.context = context;
        }
        public async Task<bool> SednEmail(string email, string subjectcontect, string solicitud)
        {
            
            try
            {
                string usuario = "";
                string rol = "";
                htmlconten send = new htmlconten();
                var query = await (from u in this.context.MmUsuariosSistemas
                                   join p in this.context.MmPerfilUsuarios on u.NombreUsuario equals p.Usuario
                                   join ru in this.context.MmRolesUsuarios on u.IdUsuario equals ru.IdUsuario
                                   where u.NombreUsuario==email
                                   select new {
                                       Usuario = p.NombreCompleto,
                                       Rol = ru.TipoRol,
                                       IdUsuario = u.IdUsuario
                                   }).FirstOrDefaultAsync();
                if (query != null) {
                    usuario = query.Usuario;
                    rol = query.Rol;
                }

                string apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("notificaciones@fcivalledelcauca.com.co", subjectcontect);
                var subject = subjectcontect;
                var to = new EmailAddress(email, "");
                var plainTextContent = "";
                string htmlContent = "";
                switch (solicitud)
                {
                    case "recuperar":
                        string hashclave = this.Hash("12345678");
                        htmlContent = send.HtmlContenpass(email, hashclave, usuario);
                        break;
                    case "bienvenida":
                        htmlContent = send.HtmlContenwelcome(email, usuario);
                        break;
                    case "cambiorol":
                        htmlContent = send.HtmlContenchangerol(email, usuario, rol);
                        break;
                    default:
                        break;
                }

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
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
    }
}
