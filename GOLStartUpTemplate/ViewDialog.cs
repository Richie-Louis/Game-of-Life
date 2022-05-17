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
    public partial class ViewDialog : Form
    {
        public ViewDialog()
        {
            InitializeComponent();
        }
        // Encapsulation

        // C++ Traditional Way (Get/Set Mutators)
        //public int GetNumber()
        //{
        //    return (int)numericUpDownNumber.Value;
        //}

        //public void SetNumber(int number)
        //{
        //    numericUpDownNumber.Value = number;
        //}

        // Preferred way in C# Property
        public int Number
        {
            get { return (int)numericUpDownNumber.Value; }
            set { numericUpDownNumber.Value = value; }
        }
    }
}
