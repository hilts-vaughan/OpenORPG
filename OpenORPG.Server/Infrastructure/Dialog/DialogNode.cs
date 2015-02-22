using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Dialog
{
    /// <summary>
    /// A single dialog node used in NPC dialog to represent a conversation. Dialog can often offer various services
    /// </summary>
    public class DialogNode
    {
        /// <summary>
        /// A list of internal dialog links that can be chosen from this tree.
        /// </summary>
        private List<DialogLink> _links = new List<DialogLink>();

        /// <summary>
        /// A string representing the text this node will represent
        /// </summary>
        public string Text { get; private set; }

        public DialogNode(string text)
        {
            Text = text;
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
