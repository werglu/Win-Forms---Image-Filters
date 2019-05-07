using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap b;
        string file=" ";
        bool witch = false;
        PictureBox pic = new PictureBox();
       
        public Form1()
        {
            InitializeComponent();

            toolStripStatusLabel1.Text = " ";
            backgroundWorker1.WorkerReportsProgress = true;

            toolStripProgressBar1.Visible = false;
            pictureBox1.Controls.Add(pic);
            pic.Height = 40;
            pic.Width = 40;
            pic.Left = 10;
            pic.Top = 10;
            pic.Visible = false;
            pic.BackColor = Color.White;

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.bmp, *.gif) | *.jpg; *.bmp; *.gif";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
               
                b = new Bitmap(dlg.FileName);
                file = dlg.FileName;
                pictureBox1.Image = b;
                toolStripStatusLabel1.Text = file;
           
                
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                toolStripProgressBar1.Visible = true;
         
                backgroundWorker1.RunWorkerAsync();
             
        }
      

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap bmp = b;

            toolStripProgressBar1.Visible = true;
            pictureBox1.Image = Image.FromFile(file);


            int width = bmp.Width;
            int height = bmp.Height;


            Color p;


            int m = height * width;
            int ile = 0;
            if (!witch)
            {
              
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        p = bmp.GetPixel(x, y);


                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;

                        int avg = (int)(r * 0.299 + g * 0.587 + b * 0.114);

                        bmp.SetPixel(x, y, Color.FromArgb((int)(r * 0.299 + g * 0.587 + b * 0.114), avg, avg));

                        backgroundWorker1.ReportProgress((100 * ile) / m);

                        ile++;

                    }

                }
             
            }
            if (witch)
            {

               
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {

                        p = bmp.GetPixel(x, y);

                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;

                        bmp.SetPixel(x, y, Color.FromArgb((int)(255 - r), 255 - g, 255 - b));
                        backgroundWorker1.ReportProgress((100 * ile) / m);

                        ile++;
                    }
                }
            
            }
            backgroundWorker1.ReportProgress(100);
            toolStripProgressBar1.Visible = false;

            pictureBox1.Image = bmp;
        }
        
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

      
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            pic.Visible = !pic.Visible;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (backgroundWorker1.IsBusy==false && file != " ")
            {
                var x = b.GetPixel(e.X, e.Y);
                pic.BackColor = x;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(backgroundWorker1.IsBusy )
            {
                MessageBox.Show("Cannot close when work in progress", "Error", MessageBoxButtons.OK);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            witch = !witch;
        }
    }
}
