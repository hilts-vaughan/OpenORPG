using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Game.Database.Models.ContentTemplates;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenORPG.Toolkit.Views.Auth
{
    public partial class ItemEditorForm : DockContent
    {
        private ItemTemplate _itemTemplate;

        public ItemEditorForm(ItemTemplate itemTemplate)
        {
            _itemTemplate = itemTemplate;
            InitializeComponent();

            propertyGrid1.SelectedObject = _itemTemplate;
        }



    }
}
