using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Game.Database.Models
{
    /// <summary>
    ///     The user account contains information pertaining to a particular account
    /// </summary>
    [Table("user_account")]
    public class UserAccount
    {
        public UserAccount(string username, string password, string email)
            : this()
        {
            Username = username;
            Password = password;
            Email = email;
        }

        public UserAccount()
        {
            //Heroes = new List<UserHero>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsOnline { get; set; }

        public virtual ICollection<UserHero> Heroes { get; set; }
    }
}