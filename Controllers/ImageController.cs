using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace BotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        public static List<(byte[] Image, string Link, int AccountId)> Images { get; set; } =
            new List<(byte[] Image, string Link, int AccountId)>();

        [HttpGet]
        public IActionResult Get(string imageName)
        {
            if (imageName == null) return NotFound();
            var res = Images.FirstOrDefault(i => i.Link == imageName).Image;
            if (res == null) return Ok();
            return File(res, "image/jpeg");
        }
    }
}