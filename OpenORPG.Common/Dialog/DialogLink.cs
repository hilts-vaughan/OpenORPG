using System.Collections.Generic;
using System.Linq;
using OpenORPG.Common.Dialog.Conditions;
using OpenORPG.Common.Entity;
using Server.Infrastructure.Dialog;

namespace OpenORPG.Common.Dialog
{
    public class DialogLink : IDialogNodeElement
    {



        public DialogNode NextNode { get; set; }

        /// <summary>
        /// Represents a list of conditions that must be fulfilled in order for this link to be usable
        /// </summary>
        private List<IDialogCondition> _dialogConditions = new List<IDialogCondition>();

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

        public void AddCondition(IDialogCondition condition)
        {
            _dialogConditions.Add(condition);
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
