namespace Betacomio.Models.CustomModels
{
    public class UpdateUser
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string? Phone { get; set; }

        public string? UserName { get; set; }
    }
}
