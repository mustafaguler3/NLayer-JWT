using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniAPI1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        public IActionResult GetStock()
        {
            var username = HttpContext.User.Identity.Name;
            var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier);

            return Ok($"username: {username} - UserId: {userId}");
        }
    }
}
