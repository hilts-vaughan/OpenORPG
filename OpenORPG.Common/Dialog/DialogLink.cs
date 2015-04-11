using System.Collections.Generic;
using System.Linq;
using OpenORPG.Common.Dialog.Actions;
using OpenORPG.Common.Dialog.Conditions;
using OpenORPG.Common.Entity;
using Server.Infrastructure.Dialog;

namespace OpenORPG.Common.Dialog
{
    public class DialogLink : IDialogNodeElement
    {



        public DialogNode NextNode { get; set; }

        public string Script { get; set; }

        /// <summary>
        /// Represents a list of conditions that must be fulfilled in order for this link to be usable
        /// </summary>
        private List<IDialogCondition> _dialogConditions = new List<IDialogCondition>();

        private List<IDialogAction> _dialogActions = new List<IDialogAction>();

        public DialogLink(string text, DialogNode nextNode)
        {
            Text = text;
            NextNode = nextNode;
            Name = "New Link";
        }

        public DialogLink()
        {
            Name = "New Link";
        }


        public List<IDialogCondition> DialogConditions
        {
            get { return _dialogConditions; }
            set { _dialogConditions = value; }
        }


        /// <summary>
        /// Provides a list of dialog links associated with this particular dialog link
        /// </summary>
        public List<IDialogAction> DialogActions
        {
            get { return _dialogActions; }
            set { _dialogActions = value; }
        }


        /// <summary>
        /// Computes whether or not this dialog link will visible to an interacting player given the conditions imposed
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsAvailable(ICharacterContract player)
        {
            return _dialogConditions.All(x => x.Verify(player));
        }

        public string Name { get; set; }
        public string Text { get; set; }
    }
}
