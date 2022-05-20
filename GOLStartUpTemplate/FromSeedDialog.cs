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
    public partial class FromSeedDialog : Form
    {
        public FromSeedDialog()
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
        public int Seed
        {
            get { return (int)seed.Value; }
            set { seed.Value = value; }
        }

        private void randomizeButton_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            Seed = rng.Next(Int32.MinValue, Int32.MaxValue);
        }
    }
}
