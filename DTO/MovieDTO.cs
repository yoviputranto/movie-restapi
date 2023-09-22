using System.ComponentModel.DataAnnotations;
namespace MovieRestfullAPI.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0,10)]
        public float Rating { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
