using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    /// <summary>
    /// A collection of all the game messages in the game currently.
    /// </summary>
    public enum GameMessage
    {

        /// <summary>
        /// A message fired off when an item cannot be used
        /// </summary>
        ItemCannotUse,
        PlayerJoinedGame,
        PlayerLeftGame,
        EquipmentChanged,
        NewQuest,
        MonsterDies,
        GainExperience,
        LevelUp,
        QuestCompleted,
        QuestCannotGiveReward,
        ItemCannotBeUsed,


    }
}
