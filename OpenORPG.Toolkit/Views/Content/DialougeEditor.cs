using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Infrastructure.Dialog;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class DialogEditor : BaseContentForm
    {

        /// <summary>
        /// The root of the tree being rendered for the dialog
        /// </summary>
        private DialogNode _rootDialogNode;

        public DialogEditor()
        {
            InitializeComponent();
        }



    }
}
