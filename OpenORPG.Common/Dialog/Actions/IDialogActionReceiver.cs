using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Common.Dialog.Actions
{
    public interface IDialogActionReceiver
    {

        void GiveExperience(int experience);
    
        void GiveItem(int itemId, int quantity);

        void SetFlag(string key, string value);

        void PerformHealing();


    }
}
