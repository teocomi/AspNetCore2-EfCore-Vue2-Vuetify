using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using AspNetCore.Sample.DesktopUtils;

namespace AspNetCore.Sample.Controllers
{

    //[Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var first = User.FindFirst(ClaimTypes.GivenName).Value;
                var last = User.FindFirst(ClaimTypes.Surname).Value;
                var username = User.FindFirst(ClaimTypes.Name).Value;

                var env = "";

                if (Configuration.IsLocal)
                    env = "LOCAL - ";
                else if (Configuration.IsDevelopment)
                    env = "DEV - ";

                ViewData["user"] = new { first, last, username };
                ViewData["env"] = env;

                if (Configuration.IsProduction)
                {
                    GoogleAnalyticsApi.TrackEvent("sitevisit", "user", username);
                }

            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
