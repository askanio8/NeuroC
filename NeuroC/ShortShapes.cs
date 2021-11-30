using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace NeuroC
{
    class ShortShapes
    {
        public ImageVSName[] capches;
        public ImageVSName[][] shapes;
        string[] adressFolders;
        public int quaLines;

        public ShortShapes(int quaLines, string sourseFolder)
        {
            this.quaLines = quaLines;
            adressFolders = Directory.GetDirectories(sourseFolder);
            capches = new ImageVSName[adressFolders.Length];
            shapes = new ImageVSName[adressFolders.Length][];

            for (int i = 0; i < adressFolders.Length; i++)
            {
                capches[i].name = adressFolders[i];
                capches[i] = GetCapcha(capches[i]);

                shapes[i] = new ImageVSName[capches[i].grayBitmap.Width - quaLines];

                shapes[i] = GetShapes(capches[i]);

                try
                {
                    Form1 form = (Form1)Form1.ActiveForm;
                    form.progressBar1.Value = i * 100 / adressFolders.Length;
                }
                catch { }
            }
        }

        public ImageVSName GetCapcha(ImageVSName imgVSname)
        {
            char[] slesh = { '\\' };
            string[] capchaW = imgVSname.name.Split(slesh);
            string nameF = imgVSname.name + "\\" + capchaW[capchaW.Length - 1];

            // загружаем jpg файл
            FileStream stream = new FileStream(nameF + " gray" + ".png", FileMode.Open);
            imgVSname.grayBitmap = new Bitmap(stream);
            stream.Close();

            // загружаем double файл
            string[] ss = File.ReadAllLines(nameF + " grayShapeDouble.txt");
            double[] d = (double[])ss.Select(s => double.Parse(s)).ToArray();

            imgVSname.grayShapeDouble = new double[130, 50];
            for (int i = 0; i < 130; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    imgVSname.grayShapeDouble[i, j] = d[i * imgVSname.grayBitmap.Height + j];
                }
            }

            //имя образа - длиннейшее имя файла 
            string[] adrFiles;
            adrFiles = Directory.GetFiles(imgVSname.name);
            string maxL = "";
            for (int i = 0; i < adrFiles.Length; i++)
            {
                if (adrFiles[i].Length > maxL.Length)
                    maxL = adrFiles[i];
            }
            imgVSname.name = maxL.Replace(imgVSname.name + "\\", "");
            imgVSname.name = imgVSname.name.Replace(".txt", "");

            return imgVSname;
        }

        public ImageVSName[] GetShapes(ImageVSName imgVSname)
        {
            ImageVSName[] shapesFromCapcha = new ImageVSName[imgVSname.grayBitmap.Width - quaLines];

            for (int shapeNumb = 0; shapeNumb < shapesFromCapcha.Length; shapeNumb++)
            {
                // делаем массив
                shapesFromCapcha[shapeNumb].grayShapeDouble = new double[quaLines, imgVSname.grayBitmap.Height];
                shapesFromCapcha[shapeNumb].output = new double[21];
                shapesFromCapcha[shapeNumb].grayShapeDoubleMono = new double[shapesFromCapcha[shapeNumb].grayShapeDouble.Length];
                for (int i = 0; i < quaLines; i++)
                {
                    for (int j = 0; j < imgVSname.grayBitmap.Height; j++)
                    {
                        shapesFromCapcha[shapeNumb].grayShapeDouble[i, j] = imgVSname.grayShapeDouble[i + shapeNumb, j];
                        shapesFromCapcha[shapeNumb].grayShapeDoubleMono[i * j + j] = imgVSname.grayShapeDouble[i + shapeNumb, j];
                    }
                }

                // делаем output
                char[] ch = new char[] { ' ' };
                string[] s = imgVSname.name.Split(ch, 50);
                int[] linesNumbs = new int[s.Length - 1]; //только номера линий

                //делаем name
                shapesFromCapcha[shapeNumb].name = s[0] + " " + quaLines + " " + shapeNumb;


                for (int i = 0; i < linesNumbs.Length; i++)
                {
                    linesNumbs[i] = Convert.ToInt32(s[i + 1]);
                }

                char[] symb = s[0].ToCharArray();
                // делаем output 
                for (int i = 0; i < symb.Length; i++)
                {
                    double proc = 0;
                    if (linesNumbs[i * 2] >= shapeNumb && linesNumbs[i * 2 + 1] >= shapeNumb && linesNumbs[i * 2] <= shapeNumb + quaLines && linesNumbs[i * 2 + 1] <= shapeNumb + quaLines)
                    {
                        proc = 1;
                    }
                    else
                    {
                        if (linesNumbs[i * 2] >= shapeNumb && (linesNumbs[i * 2] <= shapeNumb + quaLines))
                        {
                            proc = (double)(shapeNumb + quaLines - linesNumbs[i * 2] + 1) / (double)(linesNumbs[i * 2 + 1] - linesNumbs[i * 2] + 1);
                        }
                        if (linesNumbs[i * 2 + 1] >= shapeNumb && linesNumbs[i * 2 + 1] <= shapeNumb + quaLines)
                        {
                            proc = (double)(linesNumbs[i * 2 + 1] - shapeNumb + 1) / (double)(linesNumbs[i * 2 + 1] - linesNumbs[i * 2] + 1);
                        }
                    }

                    switch (symb[i])
                    {
                        case 'a':
                            shapesFromCapcha[shapeNumb].output[0] += proc;
                            break;
                        case 'e':
                            shapesFromCapcha[shapeNumb].output[1] += proc;
                            break;
                        case 'u':
                            shapesFromCapcha[shapeNumb].output[2] += proc;
                            break;
                        case 'c':
                            shapesFromCapcha[shapeNumb].output[3] += proc;
                            break;
                        case 'd':
                            shapesFromCapcha[shapeNumb].output[4] += proc;
                            break;
                        case 'h':
                            shapesFromCapcha[shapeNumb].output[5] += proc;
                            break;
                        case 'k':
                            shapesFromCapcha[shapeNumb].output[6] += proc;
                            break;
                        case 'm':
                            shapesFromCapcha[shapeNumb].output[7] += proc;
                            break;
                        case 'n':
                            shapesFromCapcha[shapeNumb].output[8] += proc;
                            break;
                        case 'p':
                            shapesFromCapcha[shapeNumb].output[9] += proc;
                            break;
                        case 'q':
                            shapesFromCapcha[shapeNumb].output[10] += proc;
                            break;
                        case 's':
                            shapesFromCapcha[shapeNumb].output[11] += proc;
                            break;
                        case 'v':
                            shapesFromCapcha[shapeNumb].output[12] += proc;
                            break;
                        case 'x':
                            shapesFromCapcha[shapeNumb].output[13] += proc;
                            break;
                        case 'y':
                            shapesFromCapcha[shapeNumb].output[14] += proc;
                            break;
                        case 'z':
                            shapesFromCapcha[shapeNumb].output[15] += proc;
                            break;
                        case '2':
                            shapesFromCapcha[shapeNumb].output[16] += proc;
                            break;
                        case '4':
                            shapesFromCapcha[shapeNumb].output[17] += proc;
                            break;
                        case '5':
                            shapesFromCapcha[shapeNumb].output[18] += proc;
                            break;
                        case '7':
                            shapesFromCapcha[shapeNumb].output[19] += proc;
                            break;
                        case '8':
                            shapesFromCapcha[shapeNumb].output[20] += proc;
                            break;
                    }
                }
                for (int j = 0; j < shapesFromCapcha[shapeNumb].output.Length; j++)
                {
                    if (shapesFromCapcha[shapeNumb].output[j] > 1)
                        shapesFromCapcha[shapeNumb].output[j] = 1;
                }

                //// переделываем output в диапазон [- 1 , 1 ]

                //for (int j = 0; j < shapesFromCapcha[shapeNumb].output.Length; j++)
                //{
                //    shapesFromCapcha[shapeNumb].output[j] -= 0.5;
                //    shapesFromCapcha[shapeNumb].output[j] *= 2;
                //}

                // делаем проекцию

                shapesFromCapcha[shapeNumb].grayShapeDoubleProjection = new double[imgVSname.grayBitmap.Height + quaLines];

                for (int i = 0; i < imgVSname.grayBitmap.Height; i++)
                {
                    shapesFromCapcha[shapeNumb].grayShapeDoubleProjection[i] = 0;
                    for (int j = 0; j < quaLines; j++ )
                    {
                        shapesFromCapcha[shapeNumb].grayShapeDoubleProjection[i] += 1 - shapesFromCapcha[shapeNumb].grayShapeDouble[j, i];
                    }
                    shapesFromCapcha[shapeNumb].grayShapeDoubleProjection[i] /= quaLines;
                }

                for (int i = 0; i < quaLines; i++)
                {
                    shapesFromCapcha[shapeNumb].grayShapeDoubleProjection[i + imgVSname.grayBitmap.Height] = 0;
                    for (int j = 0; j < imgVSname.grayBitmap.Height; j++)
                    {
                        shapesFromCapcha[shapeNumb].grayShapeDoubleProjection[i + imgVSname.grayBitmap.Height] += 1 - shapesFromCapcha[shapeNumb].grayShapeDouble[i, j];
                    }
                    shapesFromCapcha[shapeNumb].grayShapeDoubleProjection[i + imgVSname.grayBitmap.Height] /= imgVSname.grayBitmap.Height;
                }

                // нормализуем проекцию к диапазону [ 0 , 1 ]
                double mn = 1.0/shapesFromCapcha[shapeNumb].grayShapeDoubleProjection.Max();
                if (!Double.IsInfinity(mn))
                {
                    for (int i = 0; i < shapesFromCapcha[shapeNumb].grayShapeDoubleProjection.Length; i++)
                    {
                        shapesFromCapcha[shapeNumb].grayShapeDoubleProjection[i] *= mn;
                    }
                }
            }
            return shapesFromCapcha;
        }
    }
}
