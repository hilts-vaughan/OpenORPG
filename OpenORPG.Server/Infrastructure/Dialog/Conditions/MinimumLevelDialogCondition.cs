using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Infrastructure.Dialog.Conditions
{
    public class MinimumLevelDialogCondition : IDialogCondition
    {
        private int _level;

        public MinimumLevelDialogCondition(int level)
        {
           _level = level;
        }

        public bool Verify(Player player)
        {
            return player.Level >= _level;
        }
    }
}
