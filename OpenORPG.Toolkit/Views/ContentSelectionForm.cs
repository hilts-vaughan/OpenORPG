using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Toolkit.Content;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Toolkit.Views
{
    public partial class ContentSelectionForm : Form
    {
        private Type _contentType;

        public ContentSelectionForm(Type contentType)
        {
            _contentType = contentType;
            InitializeComponent();

            // Bind
            listContent.DataSource = GetContentTemplates();

        }


        private List<IContentTemplate> GetContentTemplates()
        {
            return ContentTypeResolver.GetContentTemplateFromType(_contentType);
        }

        private void listContent_DoubleClick(object sender, EventArgs e)
        {
            var item = (IContentTemplate) listContent.SelectedItem;
            SelectedTemplate = item;
            Close();
        }

        public IContentTemplate SelectedTemplate { get; private set; }


    }
}
