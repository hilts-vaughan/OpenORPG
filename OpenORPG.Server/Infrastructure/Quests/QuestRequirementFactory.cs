using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Quests;
using OpenORPG.Database.Models.Quests.Rewards;
using Server.Infrastructure.Quests.Requirements;
using Server.Infrastructure.Quests.Rewards;

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


        public static IQuestReward GetConcreteQuestReward<T>(T reward) 
            where T : QuestReward
        {
            IQuestReward result = null;

            var @switch = new Dictionary<Type, Action>
            {
                {
                    typeof (QuestRewardExperience), () => result = new ExperienceQuestReward(reward as QuestRewardExperience) }               
            };

            @switch[reward.GetType()]();
            return result;

        }



    }
}
