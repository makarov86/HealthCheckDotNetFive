using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace HealthCheckDotNetFive.Controllers
{
    [Route("img")]
    [ApiController]
    public class ImgController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetImage()
        {
            var client = new HttpClient();

            try
            {
                Thread.Sleep(70000);
                var bytes = await client.GetStreamAsync(
                    "https://icdn.lenta.ru/images/2023/03/05/19/20230305193707882/owl_detail_240_a2bdb05ed87262b135e472b7165d0777.jpeg");

                var file = File(bytes, MediaTypeNames.Image.Jpeg, DateTimeOffset.Now.AddDays(-4), EntityTagHeaderValue.Any, true);
                file.EnableRangeProcessing = true;

                return File(bytes, MediaTypeNames.Image.Jpeg, DateTimeOffset.Now.AddDays(-4), EntityTagHeaderValue.Any,
                    true);
            }
            catch (Exception e)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
