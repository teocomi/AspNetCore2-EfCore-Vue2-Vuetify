using AspNetCore.Sample.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore.Sample.Controllers
{
  // AUTHORIZATION
  //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
  [Route("[controller]")]
  public class ApiController : Controller
  {
    private readonly IHostingEnvironment _hostingEnvironment;
    private DataContext _context;

    public ApiController(DataContext context, IHostingEnvironment hostingEnvironment)
    {
      _hostingEnvironment = hostingEnvironment;
      _context = context;
    }

    // GET USER INFO, IF AUTHENTICATED
    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
      var first ="";
      var last = "";
      var username = "";

      try
      {
        first = User.FindFirst(ClaimTypes.GivenName).Value;
        last = User.FindFirst(ClaimTypes.Surname).Value;
        username = User.FindFirst(ClaimTypes.Name).Value;
      }
      catch
      {

      }
    

      return Json(
             new { first, last, username }
          );
    }


  }
}
