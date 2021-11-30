using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeuroC
{
    public partial class Form1 : Form
    {
        public static string sourseFolder;
        ImageVSName[][] imgvsn;
        Neuronet newNet;
        Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox1.Image = 
            //    Program.ivsn[0].grayBitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sourseFolder = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int sh = Convert.ToInt32(textBox5.Text);
            ShortShapes shortShapes = new ShortShapes(sh, sourseFolder);
            imgvsn = shortShapes.shapes;
            shortShapes = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int l = Convert.ToInt32(textBox2.Text);
            int n = Convert.ToInt32(textBox3.Text);
            double eta = Convert.ToDouble(textBox4.Text);
            newNet = new Neuronet(l,//слоев
               imgvsn[0][0].grayShapeDoubleProjection.Length,//входов
                imgvsn[0][0].output.Length,//выходов
                n,//нейронов
                0.1, 0.9,// диапазон()
                eta);//скорость обучения eta
            //double[] outp = newNet.ForwardFlow();

            double sqrErr = 0;
            double sumSqrr = 0;
            int randCapcha;
            int randShape;

            textBox1.Text = "";

            for (int j = 0; j < 300; j++)
            {
                sumSqrr = 0;
                for (int i = 0; i < 100; i++)
                {
                    randCapcha = 0;
                    //randCapcha = rand.Next(imgvsn.Length - 1);
                    randShape = rand.Next(imgvsn[0].Length - 1);
                    sqrErr = newNet.TraningNet(imgvsn[randCapcha][randShape].grayShapeDoubleProjection, imgvsn[randCapcha][randShape].output);
                    sumSqrr += sqrErr;
                    //textBox1.AppendText(Convert.ToString(sqrErr) + "  " + Convert.ToString(randCapcha) + "  " + Convert.ToString(randShape) + Environment.NewLine);
                }
                textBox1.AppendText(Convert.ToString(sumSqrr/100) + Environment.NewLine);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int randCapcha;
            int randShape;
            randCapcha = 0;
            //randCapcha = rand.Next(imgvsn.Length - 1);
            randShape = rand.Next(imgvsn[0].Length - 1);
            label1.Text = imgvsn[randCapcha][randShape].name;
            double[] dou = newNet.ForwardFlow(imgvsn[randCapcha][randShape].grayShapeDoubleProjection);

            textBox1.Clear();
            for (int i = 0; i < imgvsn[randCapcha][randShape].output.Length; i++)
            {
                textBox1.AppendText(Convert.ToString(dou[i]) + " ");
                textBox1.AppendText(Convert.ToString(imgvsn[randCapcha][randShape].output[i]) + Environment.NewLine);
            }

            double sqrError = 0;
            for (int i = 0; i < imgvsn[randCapcha][randShape].output.Length; i++)
            {
                sqrError += Math.Pow((dou[i] - imgvsn[randCapcha][randShape].output[i]), 2) / 2;
            }
            textBox1.AppendText("sqrError:" + Environment.NewLine);
            textBox1.AppendText(Convert.ToString(sqrError) + Environment.NewLine);
        }

    }
}
