using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HealthCheckDotNetFive.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IHealthStatusInfo HealthCheck()
        {
            return new HealthResponse { StatusName = "ОК" };
        }

        [HttpGet("uptime")]
        public IActionResult UpTime()
        {
            var span = (DateTime.Now - Process.GetCurrentProcess().StartTime);
            var info = $"Up time: {Math.Round(span.TotalHours)} hours {span.Minutes:D2} minutes {span.Seconds:D2} seconds";

            return Ok(new { UpTimeInfo = info });
        }

        [HttpGet("Info")]
        public IActionResult Info()
        {
            return BadRequest(new HealthResponse { StatusName = "34" });
        }
    }

    public interface IHealthStatusInfo
    {
        public string StatusName { get; set; }
    }

    public class HealthResponse : IHealthStatusInfo
    {
        [JsonPropertyName("status")]
        public string StatusName { get; set; }
    }
}