using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sinecoserveract.Entidades;
using sinecoserveract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("api/navigation")]
    public class NavigationController : Controller
    {
        private readonly WebSIDataContext context;

        public NavigationController(WebSIDataContext contex)
        {
            this.context = contex;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Index()
        {

            List<Modulos> menu = new List<Modulos>();
            Navigation _navigation = new Navigation();


            try
            {

        
            }
            catch (Exception ex)
            {

            }
            return new JsonResult(_navigation);
        }
    }
}
