using ibDiary_web.Server.Data;
using ibDiary_web.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ibDiary_web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlphaSignupsController(IDbContextFactory<AlphaDbContext> factory) : ControllerBase
    {
        private readonly IDbContextFactory<AlphaDbContext> _dbFactory = factory;

        [HttpPost]
        public ActionResult<AlphaSignupDto> CreateSignup([FromBody] CreateAlphaSignupRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { message = "Email is required." });

            using var context = _dbFactory.CreateDbContext();

            var signup = new AlphaSignupDto
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Notes = request.Notes?.Trim(),
                Source = request.Source ?? "marketing-site"
            };

            context.AlphaSignups.Add(signup);
            context.SaveChanges();

            return Ok(signup);
        }
    }


    public class CreateAlphaSignupRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Notes { get; set; }
        public string? Source { get; set; }
    }
}
