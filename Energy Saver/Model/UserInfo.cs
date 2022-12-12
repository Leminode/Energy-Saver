using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Energy_Saver.Model
{
    [Table("users")]
    public class UserInfo
    {
        [Key]
        [Column("id")]
        public int UserId { get; set; }

        //[Column("nickname")]
        //public string UserName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        //[Column("password")]
        //public string Password { get; set; }

        //[Column("email_verified")]
        //public bool EmailVerified { get; set; }
    }
}
