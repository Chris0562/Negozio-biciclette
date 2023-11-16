using System.ComponentModel.DataAnnotations;

namespace Betacomio.Models.CustomModels
{
    public class RegisterForm
    {
        public string Email { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string FirstName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = null!;
        public string? UserName { get; set; }
        public bool isAdmin { get; set; }


    }
}
