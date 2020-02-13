using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNoteBook
{
    public partial class Form1 : Form
    {
        //Используется как ключ для кодировки
        string hash = "";

        public void Params()
        {
            richTextBox1.BackColor = Properties.Settings.Default.textBackColor;
            richTextBox1.ForeColor = Properties.Settings.Default.textFontColor;
            richTextBox1.Font = Properties.Settings.Default.textFont;
        }

        async void counter()
        {
            while (true)
            {
                stringCount1.Text = "Strings" + " - " + richTextBox1.Lines.Length.ToString();
                stringCount.Text = "Symbols" + " - " + richTextBox1.Text.Length.ToString();
                await Task.Delay(50);
            }
            
        }
        public Form1()
        {
            InitializeComponent();
            document.PrintPage += new PrintPageEventHandler(document_PrintPage);

        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, new Font("Arial", 20, FontStyle.Regular), Brushes.Black, 20, 20);
        }

        void SaveAs()
        {
            string writePath = "";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                writePath = saveFileDialog1.FileName;
            }
            if (writePath != "")
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))

                {
                    sw.WriteLine(richTextBox1.Text);
                }

                using (StreamReader sr = new StreamReader(writePath, false))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                }

                this.Text = writePath;
            }
        }

        private void sabeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string readPath = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                readPath = openFileDialog1.FileName;

                using (StreamReader sr = new StreamReader(readPath, false))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                }

                this.Text = readPath;
            }

            

        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Text == "MyNoteBook")
            {
                SaveAs();
            }
            else
            {
                string path = this.Text;

                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(richTextBox1.Text);
                }

                using (StreamReader sr = new StreamReader(path, false))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                }
            }
        }

        PrintDocument document = new PrintDocument();
        PrintDialog dialog = new PrintDialog();


        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialog.Document = document;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                document.Print();
            }   
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = "";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;

                using (FileStream fs = File.Create(path, 1024))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("");
                    fs.Write(info, 0, info.Length);
                }

                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                using (StreamReader sr = new StreamReader(path, false))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                }

                this.Text = path;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Params();
            counter();
        }

        private void цветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 showform = new Form2();
            showform.Show();
        }

        private void закодироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hash = hashCode.Text;
            byte[] data = UTF8Encoding.UTF8.GetBytes(richTextBox1.Text);
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    richTextBox1.Text = Convert.ToBase64String(results, 0, results.Length);
                }

            }
        }

        private void раскодироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hash = hashCode.Text;
            byte[] data = Convert.FromBase64String(richTextBox1.Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    richTextBox1.Text = UTF8Encoding.UTF8.GetString(results);
                }

            }
        }
    }
}
