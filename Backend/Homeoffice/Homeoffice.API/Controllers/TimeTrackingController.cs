using Homeoffice.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homeoffice.API.Controllers
{
    [Authorize]
    public class TimeTrackingController : BaseApiController
    {
        private readonly ITimeTrackingService _timeTrackingService;
        public TimeTrackingController(ITimeTrackingService timeTrackingService)
        {
            _timeTrackingService = timeTrackingService;
        }

        // POST /api/TimeTracking/start
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] string? description)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            try
            {
                var resultDto = await _timeTrackingService.StartTrackingAsync(userId, description);
                return Ok(resultDto);
            }
            catch (InvalidOperationException ex)
            {
                // This exception is thrown if the user is already tracking time (Error 409)
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // POST /api/TimeTracking/stop
        [HttpPost("stop")]
        public async Task<IActionResult> Stop()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            var resultDto = await _timeTrackingService.StopTrackingAsync(userId);

            if (resultDto is null)
            {
                return NotFound(new { message = "No active time tracking entry found to stop." });
            }

            return Ok(resultDto);
        }

        // GET /api/TimeTracking/overview?startDate=''&endDate=''
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview([FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new { message = "Start date cannot be after end date." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            var results = await _timeTrackingService.GetOverviewAsync(userId, startDate, endDate);

            return Ok(results);
        }
    }
}
