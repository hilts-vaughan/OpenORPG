using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;

namespace OpenORPG.Database.Models
{
    [Table("user_flags")]
    public class UserFlag
    {
        public long UserFlagId { get; set; }

        public virtual UserHero User { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

    }
}
