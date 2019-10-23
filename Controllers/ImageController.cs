using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace BotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        public static List < (byte[] Image, string Link, int AccountId) > Images { get; set; } =
            new List < (byte[] Image, string Link, int AccountId) > ();

        [HttpGet]
        public IActionResult Get([FromQuery] string imageName)
        {
            System.Console.WriteLine(imageName);
            if (imageName == null) return null;
            var res = Images.FirstOrDefault(i => i.Link == imageName).Image;
            if (res == null) return null;

            // processing the stream.            
            Response.Body.WriteAsync(res, 0, res.Length);

            return new EmptyResult();

            // Response.Clear();
            // Response.Headers.Add("Content-Disposition", "attachment; filename=myfile.jpeg");
            // Response.ContentType = "image/jpeg";

            //Write all my data

            //await System.IO.File.WriteAllBytesAsync("file.jpeg", res);
            // var provider = new PhysicalFileProvider("/home/askold/Documents/code/WhoTheFuckBot/");

            //await Response.SendFileAsync(provider.GetFileInfo("file.jpeg"));

            //Not sure what else to do here
            //return Ok();            
            //return new FileContentResult(res, $"image/jpeg");
            //return File(res, "image/jpeg");
        }
    }
}