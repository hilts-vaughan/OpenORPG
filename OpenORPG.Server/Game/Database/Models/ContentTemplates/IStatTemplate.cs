using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Database.Models.ContentTemplates
{
    public interface IStatTemplate
    {
        int Strength { get; set; }
        int Dexterity { get; set; }
        int Vitality { get; set; }
        int Intelligence { get; set; }
        int Hitpoints { get; set; }
    }
}
