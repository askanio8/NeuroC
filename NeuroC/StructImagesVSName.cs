using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace NeuroC
{
    public struct ImageVSName
    {
        public string name;
        public Bitmap colorBitmap;
        public byte[, ,] colorShape;
        public double[, ,] colorShapeDouble;
        public Bitmap grayBitmap;
        public byte[,] grayShape;
        public double[,] grayShapeDouble;
        public double[] grayShapeDoubleMono;
        public double[] output;
        public double[] grayShapeDoubleProjection;
    }
}
