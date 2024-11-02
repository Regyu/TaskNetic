using Microsoft.AspNetCore.Identity;
namespace TaskNetic.Models
{
    public class ApplicationRole : IdentityRole
    {
        // Możesz dodać dodatkowe właściwości, jeśli potrzebujesz
        public required string Description { get; set; }

    }
}