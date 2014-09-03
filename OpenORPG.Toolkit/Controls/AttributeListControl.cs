using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Database.Enums;

namespace OpenORPG.Toolkit.Controls
{
    public partial class AttributeListControl : UserControl
    {
        private List<NumericUpDown> _numericUpDowns = new List<NumericUpDown>(); 

        public AttributeListControl()
        {
            InitializeComponent();

            for (int index = 0; index < Enum.GetNames(typeof (StatTypes)).Length; index++)
            {
                var type = Enum.GetNames(typeof (StatTypes))[index];
                var label = new Label() {Text = type};
                var numericBox = new NumericUpDown() {Name = "numeric" + type};


                flowLayoutPanel1.Controls.Add(label);
                flowLayoutPanel1.Controls.Add(numericBox);

                // Append to the list box
                _numericUpDowns.Add(numericBox);
            }
        }

        public long[] StatValues
        {
            get { return _numericUpDowns.Select(x => (long) x.Value).ToArray(); }
        }


    }
}
