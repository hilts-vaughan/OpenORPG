using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Common.Entity;

namespace OpenORPG.Common.Dialog.Conditions
{
    public class QuestProgressDialogCondition : IDialogCondition
    {
        public int QuestId { get; private set; }
        public int StepId { get; private set; }


        public QuestProgressDialogCondition(int questId, int stepId)
        {
            QuestId = questId;
            StepId = stepId;
        }

        public QuestProgressDialogCondition()
        {
            
        }

        public override bool Verify(ICharacterContract player)
        {
            return player.IsOnQuestStep(QuestId, StepId);
        }
    }
}
