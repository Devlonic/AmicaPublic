using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Amica.API.Data.DTO {
    public class SignUpRequest {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        [DefaultValue($"mycoolemail@gmail.com")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        [JsonPropertyName("fullname")]
        [DefaultValue($"John Smith")]
        public string FullName { get; set; } = null!;

        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        [JsonPropertyName("nickname")]
        [DefaultValue($"devloner")]
        public string Nickname { get; set; } = null!;

        [Required]
        [JsonPropertyName("password")]
        [DefaultValue($"KAKAkika123")]
        public string Password { get; set; } = null!;

        [Required]
        [JsonPropertyName("avatar")]
        public IFormFile ProfilePhoto { get; set; } = null!;

        [JsonPropertyName("captchaV2")]
        public string? CaptchaV2 { get; set; }
    }
}
