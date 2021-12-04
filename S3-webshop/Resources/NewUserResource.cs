using System.ComponentModel.DataAnnotations;

namespace S3_webshop.Resources
{
    public class NewUserResource
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
