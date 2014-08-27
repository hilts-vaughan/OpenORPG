using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Quests;
using Server.Infrastructure.Quests.Requirements;

namespace Server.Infrastructure.Quests
{
    public static class QuestRequirementFactory
    {

        public static IQuestRequirement GetConcreteQuestRequirement<TQuestRequirement>(TQuestRequirement requirement)
            where TQuestRequirement : QuestRequirement
        {

            IQuestRequirement result = null;

            var @switch = new Dictionary<Type, Action>
            {
                {
                    typeof (QuestMonsterRequirementTable), () => result = new QuestMonstersKilledRequirement(requirement as QuestMonsterRequirementTable) }               
            };

            @switch[requirement.GetType()]();
            return result;
        }


    }
}
