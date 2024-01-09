using Microsoft.AspNetCore.Identity;

namespace PersonalBlogPlatform.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? ProfilePic { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
