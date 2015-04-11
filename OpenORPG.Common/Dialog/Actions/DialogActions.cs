using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Common.Dialog.Actions
{

    /*
     *  This file contains a lot a dialog action commands that are to be implemented by a receiver.
     *  They are mostly data to be set and edited through an interface, then tossed away.
     * 
     */

    public class AwardExperienceDialogAction : IDialogAction
    {

        public int EXP { get; set; }

        public AwardExperienceDialogAction(int exp)
        {
            EXP = exp;
        }

        public AwardExperienceDialogAction()
        {
            
        }

        public void Execute(IDialogActionReceiver receiver)
        {
            receiver.GiveExperience(EXP);
        }
    }

    public class AwardItemDialogAction : IDialogAction
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public AwardItemDialogAction()
        {
            
        }

        public AwardItemDialogAction(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        public void Execute(IDialogActionReceiver receiver)
        {
            receiver.GiveItem(ItemId, Quantity);
        }

    }

    public class SetFlagDialogAction : IDialogAction
    {
        public string FlagKey { get; set; }
        public string FlagValue { get; set; }

        public SetFlagDialogAction()
        {
            
        }

        public SetFlagDialogAction(string flagKey, string flagValue)
        {
            FlagKey = flagKey;
            FlagValue = flagValue;
        }

        public void Execute(IDialogActionReceiver receiver)
        {
            receiver.SetFlag(FlagKey, FlagValue);
        }
    }

    public class PerformHealingDialogAction : IDialogAction
    {
        public PerformHealingDialogAction()
        {
            
        }

        public void Execute(IDialogActionReceiver receiver)
        {
            receiver.PerformHealing();
        }
    }


}
