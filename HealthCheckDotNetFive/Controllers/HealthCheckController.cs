using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCheckDotNetFive.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public HealthResponse HealthCheck()
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

        [HttpGet("info")]
        public IActionResult Info()
        {
            return BadRequest(new HealthResponse { StatusName = "Bdr" });
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