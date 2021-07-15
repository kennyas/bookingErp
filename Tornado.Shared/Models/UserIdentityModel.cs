using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;

namespace Tornado.Shared.Models
{
    [Table(nameof(GigmUser))]
    public class GigmUser : IdentityUser<Guid>
    {
        public string LastName { get; set; }

        //Code of countries
        [MaxLength(100)]
        public string Nationality { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string MiddleName { get; set; }
        public string Pin { get; set; }
        public string Unit { get; set; }
        public Gender? Gender { get; set; }
        public string PictureUrl { get; set; }
        public bool ChangePasswordOnLogin { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName}";
            }
        }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Activated { get; set; }
        public bool IsDeleted { get; set; }
        public UserType UserType { get; set; }
        [MaxLength(4)]
        public string DialingCode { get; set; }
    }

    public class GigmUserClaim : IdentityUserClaim<Guid>
    {
    }

    public class GigmUserLogin : IdentityUserLogin<Guid>
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }

    public class GigmRole : IdentityRole<Guid>
    {
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public GigmRole()
        {
            CreatedOnUtc = Clock.Now;
            Id = Guid.NewGuid();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
    }

    public class GigmUserRole : IdentityUserRole<Guid>
    {
    }

    public class GigmRoleClaim : IdentityRoleClaim<Guid>
    {
    }

    public class GigmUserToken : IdentityUserToken<Guid>
    {
    }
}