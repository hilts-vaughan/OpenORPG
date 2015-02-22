using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Common.Dialog;

namespace Server.Infrastructure.Dialog
{
    /// <summary>
    /// A single dialog node used in NPC dialog to represent a conversation. Dialog can often offer various services
    /// </summary>
    public class DialogNode : IDialogNodeElement
    {
        /// <summary>
        /// A list of internal dialog links that can be chosen from this tree.
        /// </summary>
        private List<DialogLink> _links = new List<DialogLink>();

        public string Name { get; set; }

        public string Text { get; set; }



        public DialogNode(string text)
        {
            Text = text;
            Name = "New Node";
        }

        public DialogNode()
        {
            Name = "New Node";
        }

        /// <summary>
        /// A collection of links that are maintained by this dialog node
        /// </summary>
        public List<DialogLink> Links
        {
            get { return _links; }
        }

       
        private void AddDialogLink(DialogLink link)
        {
            
        }

        private void RemoveDialogLink(DialogLink link)
        {
            
        }


    }
}
