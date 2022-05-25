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
    public partial class PlayerDialog : Form
    {        
        public PlayerDialog()
        {
            InitializeComponent();
        }

        public string Player
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public string Music
        {
            get;
            set;
        }

        private void PlayerDialog_Load(object sender, EventArgs e)
        {
            listBox = new ListBox();
            listBox.Location = new Point(287, 109);
            listBox.Items.Add(Music);
            this.Controls.Add(musiclist);
        }
    }
}
