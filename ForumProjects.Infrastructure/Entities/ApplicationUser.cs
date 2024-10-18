using ForumProjects.Infrastructure.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForumProjects.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public UserLevel? UserLevels { get; set; }

        public virtual Account Accounts { get; set; }

    }
}
