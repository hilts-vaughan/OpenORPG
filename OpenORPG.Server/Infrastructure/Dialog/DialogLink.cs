using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Infrastructure.Dialog.Conditions;

namespace Server.Infrastructure.Dialog
{
    public class DialogLink
    {

        /// <summary>
        /// Represents the dialog node that is immediately attached to this, going forward
        /// </summary>
        private DialogNode _attachedDialogNode;

        /// <summary>
        /// Represents a list of conditions that must be fulfilled in order for this link to be usable
        /// </summary>
        private List<IDialogCondition> _dialogConditions = new List<IDialogCondition>();

        /// <summary>
        /// Computes whether or not this dialog link will visible to an interacting player given the conditions imposed
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsAvailable(Player player)
        {
            return _dialogConditions.All(x => x.Verify(player));
        }

        public string Text { get; private set; }


    }
}
