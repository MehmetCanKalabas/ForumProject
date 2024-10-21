using ForumProjects.Infrastructure.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForumProjects.Infrastructure.Entities
{
    public class Account
    {
        public string Id { get; set; }

        public string UserId { get; set; } // ApplicationUser'ın Id'si

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public UserLevel? UserLevels { get; set; }

        // ApplicationUser ile ilişki
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
