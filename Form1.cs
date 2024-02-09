using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace form_video
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                label2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var handleM3U8 = new HandleM3U8(textBox1.Text);
            handleM3U8.ProgressoDownload += (progresso) =>
            {
                    progressBar1.Value = (int)progresso;
            };

            var filePath = await handleM3U8.Download(folderBrowserDialog1.SelectedPath);

            await handleM3U8.DownloadTSFiles(filePath, folderBrowserDialog1.SelectedPath);
            
            progressBar1.Value = 0;
            MessageBox.Show("Download Concluido");
        }
    }
}
