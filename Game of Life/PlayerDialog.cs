using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace GOLStartUpTemplate
{
    public partial class PlayerDialog : Form
    {
        SoundPlayer music1 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Diamond no Ace Opening 3 full.wav");
        SoundPlayer music2 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Savage Genius- Watashi wo mitsukete (Pandora Hearts-ED-2).wav");
        SoundPlayer music3 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Baton.wav");
        SoundPlayer music4 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Minami-Ke Okaeri - Merry Christmas S.wav");
        string m1 = "Diamond no Ace Opening 3 full";
        string m2 = "Savage Genius- Watashi wo mitsukete (Pandora Hearts-ED-2)";
        string m3 = "Baton";
        string m4 = "Minami-Ke Okaeri - Merry Christmas";

        public PlayerDialog()
        {
            InitializeComponent();
        }

        public string Player
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public string[] Music
        {
            get;
            set;
        }

        public int Choice
        { get; set; }

        private void PlayerDialog_Load(object sender, EventArgs e)
        {
            //listBox = new ListBox();
            //listBox.Location = new Point(287, 109);
            //listBox.Items.Add(Music);
            listBox.Items.AddRange(Music);
            this.Controls.Add(listBox);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            char t = 'm';
            string m = "music1";
            for (int i = 0; i < 4; i++)
            {
                switch (listBox.SelectedIndex)
                {
                    case 0:
                        music1.PlayLooping();
                        Close();
                        break;
                    case 1:
                        music2.PlayLooping();
                        Close();
                        break;
                    case 2:
                        music3.PlayLooping();
                        Close();
                        break;
                    case 3:
                        music4.PlayLooping();
                        Close();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
