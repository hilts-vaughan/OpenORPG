using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenORPG.Toolkit.Controls
{
    class Field : FlowLayoutPanel
    {
        private Label label;

        private TextBox _textBox;

        public TextBox TextBox
        {
            get { return _textBox; }            
        }

        public string LabelText
        {
            get { return label.Text ??  ""; }
            set { label.Text = value; }
        }
 
        public Field()
        {
            AutoSize = true;

            label = new Label();
            label.AutoSize = true;
            label.Anchor = AnchorStyles.Left;
            label.TextAlign = ContentAlignment.MiddleLeft;

            Controls.Add(label);

            _textBox = new TextBox();

            Controls.Add(_textBox);
        }


    }

}
