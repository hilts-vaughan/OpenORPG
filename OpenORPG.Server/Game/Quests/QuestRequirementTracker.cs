using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Infrastructure.Logging;

namespace Server.Game.Quests
{
    public class MonsterKillInfo
    {
        public MonsterKillInfo(long questId, long monsterId, long monsterAmount)
        {
            QuestId = questId;
            MonsterId = monsterId;
            MonsterAmount = monsterAmount;
        }

        public long QuestId { get; set; }
        public long MonsterId { get; set; }
        public long MonsterAmount { get; set; }

    }


}