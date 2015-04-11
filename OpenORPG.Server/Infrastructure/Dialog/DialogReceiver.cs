using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Common.Dialog.Actions;
using OpenORPG.Database.Enums;
using Server.Game.Entities;

namespace Server.Infrastructure.Dialog
{
    public class DialogReceiver : IDialogActionReceiver
    {

        private Player _player;

        public void BeginSession(Player player)
        {
            _player = player;
        }

        public void EndSession()
        {
            _player = null;
        }

        public void GiveExperience(int experience)
        {
            _player.Experience += experience;
        }

        public void GiveItem(int itemId, int quantity)
        {
            //TODO: Need's more advanced logic
            throw new NotImplementedException();
        }

        public void SetFlag(string key, string value)
        {
            _player.SetFlagKey(key, value);
        }

        public void PerformHealing()
        {
            _player.CharacterStats[StatTypes.Hitpoints].CurrentValue =
                _player.CharacterStats[StatTypes.Hitpoints].MaximumValue;
        }


        

    }
}
