using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate
{
    public partial class OptionsSettingsDialog : Form
    {
        public OptionsSettingsDialog()
        {
            InitializeComponent();
        }

        public int Width
        {
            get { return (int)widthNumeric.Value; }
            set { widthNumeric.Value = value; }
        }

        public int Height
        {
            get { return (int)heightNumeric.Value; }
            set { heightNumeric.Value = value; }
        }

        public int Timer
        {
            get { return (int)timeNumeric.Value; }
            set { timeNumeric.Value = value; }
        }
    }
}
