using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
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
            using (var client = new HttpClient())
            {
                try
                {
                    var bytes = await client.GetByteArrayAsync(
                        "https://media-cdn.tripadvisor.com/media/photo-m/1280/17/72/47/aa/utsikten-sommartid.jpg");

                    var file = File(bytes, MediaTypeNames.Image.Jpeg, DateTimeOffset.Now.AddDays(-4),
                        EntityTagHeaderValue.Any, true);
                    file.EnableRangeProcessing = true;

                    return File(bytes, MediaTypeNames.Image.Jpeg, DateTimeOffset.Now.AddDays(-4),
                        EntityTagHeaderValue.Any,
                        true);
                }
                catch (Exception e)
                {
                    return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
                }
            }
        }

        [HttpGet("{i}")]
        public async Task<IActionResult> GetSecondImage(int i)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(
                        "https://media-cdn.tripadvisor.com/media/photo-m/1280/21/7d/4e/7c/terrassen.jpg");

                    var bytes = await response.Content.ReadAsByteArrayAsync();

                    var file = File(bytes, MediaTypeNames.Image.Jpeg, DateTimeOffset.Now.AddDays(-2),
                        EntityTagHeaderValue.Any, true);
                    file.EnableRangeProcessing = true;

                    var eTag = response.Headers.ETag?.Tag != null
                        ? new EntityTagHeaderValue(new StringSegment(response.Headers.ETag?.Tag))
                        : EntityTagHeaderValue.Any;

                    var result = File(bytes, MediaTypeNames.Image.Jpeg, DateTimeOffset.Now.AddDays(-i), eTag, true);
                    //result.FileDownloadName = "image.jpg";

                    return result;
                }
                catch (Exception e)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
                }
            }
        }
    }
}
