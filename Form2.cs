using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNoteBook
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.textBackColor = colorDialog1.Color;
                Properties.Settings.Default.Save();
                

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.textFontColor = colorDialog1.Color;
                Properties.Settings.Default.Save();
                

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.textFont = fontDialog1.Font;
                Properties.Settings.Default.Save();
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Restart();
            
        }
    }
}
