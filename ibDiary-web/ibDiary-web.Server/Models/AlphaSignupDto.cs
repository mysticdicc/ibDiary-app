using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_web.Server.Models
{
    public class AlphaSignupDto
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Notes { get; set; }
        public string Source { get; set; } = "marketing-site";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
