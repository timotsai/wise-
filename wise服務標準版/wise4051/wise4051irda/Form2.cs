using System;
using System.Media;
using System.Windows.Forms;

namespace wise4051irda
{
    public partial class Form2 : Form
    {
        private int voicetype = 0;
        public Form2(int voice)
        {
            InitializeComponent();
            voicetype = voice;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            SoundPlayer snd = new SoundPlayer("alarm"+ voicetype.ToString()+".wav");
            snd.PlaySync();
            this.Close();
        }

    }
}
