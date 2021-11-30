using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace NeuroCОтключено
{
    public struct ImageVSName
    {
        //public Image image;
        public string name;
        public Bitmap colorBitmap;
        public byte[, ,] colorShape;
        public Bitmap grayBitmap;
        public byte[,] grayShape;
    }

    

    class ImageProcessing
    {
        public string adressFolder;
        public string[] filenames;
        ImageVSName[] capches;
        ImageVSName[] symbols;
        public double[] outp = new double[21];

        public ImageProcessing(string adressFolder)
        {
            this.adressFolder = adressFolder;
            filenames = Directory.GetFiles(adressFolder, "*.jpg");
        }

        public ImageVSName[] CreateIVSNCapches()
        {
            capches = new ImageVSName[filenames.Length];
            for (int i = 0; i < filenames.Length; i++)
            {
                string namepic = filenames[i].Replace(adressFolder, "");
                namepic = namepic.Replace(".jpg", "");
                capches[i].name = namepic.Substring(1);

                FileStream stream = new FileStream(filenames[i], FileMode.Open);
                capches[i].colorBitmap = new Bitmap(stream);
                stream.Close();

                capches[i] = MakeGray(capches[i]);
            }
            return capches;
        }

        private ImageVSName MakeGray(ImageVSName imgVSname)
        {
            imgVSname.colorShape = new byte[imgVSname.colorBitmap.Width, imgVSname.colorBitmap.Height, 3];
            imgVSname.grayShape = new byte[imgVSname.colorBitmap.Width, imgVSname.colorBitmap.Height];

            Bitmap bitm = new Bitmap(130, 50);

            Color clr1 = new Color();
            Color clr2;
            for (int i = 0; i < imgVSname.colorBitmap.Width; i++)
            {
                for (int j = 0; j < imgVSname.colorBitmap.Height; j++)
                {
                    clr1 = imgVSname.colorBitmap.GetPixel(i, j);
                    imgVSname.colorShape[i, j, 0] = clr1.R;
                    imgVSname.colorShape[i, j, 1] = clr1.G;
                    imgVSname.colorShape[i, j, 2] = clr1.B;

                    int clr = (clr1.R+clr1.G+clr1.B)/3;
                    clr2 = Color.FromArgb(clr,clr,clr);
                    bitm.SetPixel(i, j, clr2);
                    imgVSname.grayShape[i, j] = (byte)clr;
                    imgVSname.grayBitmap = bitm;
                }
                if (i == 25)
                {
                    break;
                }
            }

            return imgVSname;
        }

        public double[,] GetShape()
        {
            Random rand = new Random();
            int numbCapcha = rand.Next(capches.Length - 1);
            int numbFirstLine = rand.Next(70);
            double[,] shape = new double[50,50];

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    shape[i, j] = ((double)capches[numbCapcha].grayShape[i + numbFirstLine, j])/(double)255;
                }
            }

            char[] ch =  new char[]{' '};
            string[] s = capches[numbCapcha].name.Split(ch,50);
            int[] iss = new int[s.Length-1];

            for (int i = 0; i<iss.Length;i++ )
            {
                iss[i]=Convert.ToInt32(s[i+1]);
            }

            char[] symb = s[0].ToCharArray();

            for (int i = 0; i < iss.Length/2; i++)
            {
                double proc = 0;
                if(iss[i*2]>=numbFirstLine && iss[i*2+1]>=numbFirstLine && iss[i*2]<=numbFirstLine+50 && iss[i*2+1]<=numbFirstLine+50)
                {
                    proc=1;
                }
                else
                {
                    if(iss[i*2]>=numbFirstLine && (iss[i*2]<=numbFirstLine + 50))
                    {
                        proc = (double)(numbFirstLine+50-iss[i*2]+1)/(double)(iss[i*2+1]-iss[i*2] + 1);
                    }
                    if(iss[i*2+1]>=numbFirstLine && iss[i*2+1]<=numbFirstLine + 50)
                    {
                        proc = (double)(iss[i*2+1] - numbFirstLine+1)/(double)(iss[i*2+1]-iss[i*2] + 1);
                    }
                }

                    switch (symb[i])
                    { 
                        case 'a':
                            outp[0] += proc;
                            break;
                        case 'e':
                            outp[1] += proc;
                            break;
                        case 'u':
                            outp[2] += proc;
                            break;
                        case 'c':
                            outp[3] += proc;
                            break;
                        case 'd':
                            outp[4] += proc;
                            break;
                        case 'h':
                            outp[5] += proc;
                            break;
                        case 'k':
                            outp[6] += proc;
                            break;
                        case 'm':
                            outp[7] += proc;
                            break;
                        case 'n':
                            outp[8] += proc;
                            break;
                        case 'p':
                            outp[9] += proc;
                            break;
                        case 'q':
                            outp[10] += proc;
                            break;
                        case 's':
                            outp[11] += proc;
                            break;
                        case 'v':
                            outp[12] += proc;
                            break;
                        case 'x':
                            outp[13] += proc;
                            break;
                        case 'y':
                            outp[14] += proc;
                            break;
                        case 'z':
                            outp[15] += proc;
                            break;
                        case '2':
                            outp[16] += proc;
                            break;
                        case '4':
                            outp[17] += proc;
                            break;
                        case '5':
                            outp[18] += proc;
                            break;
                        case '7':
                            outp[19] += proc;
                            break;
                        case '8':
                            outp[20] += proc;
                            break;
                    }
            }
                for (int j =0;j<outp.Length;j++)
                {
                    if(outp[j]>1)
                        outp[j]=1;
                }

                return shape;
        }
    }
}
