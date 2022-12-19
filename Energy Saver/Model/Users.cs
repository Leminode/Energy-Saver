using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Energy_Saver.Model
{
    [Table("users")]
    public class Users
    {
        [Key]
        [Column("id")]
        public int UserId { get; set; }

        [Column("nickname")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters long")]
        public string? UserName { get; set; }

        [Column("email")]
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        public string? UserEmailAddress { get; set; }

        //[Column("password")]
        //public string Password { get; set; }

        //[Column("email_verified")]
        //public bool EmailVerified { get; set; }

        [Column("email_password")]
        public string? UserEmailPassword { get; set; }

        [NotMapped]
        public string? UserProfileImage { get; set; }
    }
}
