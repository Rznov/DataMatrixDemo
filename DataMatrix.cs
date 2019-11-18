using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using System.Threading.Tasks;
using System.Drawing;

namespace DataMatrixForms
{
    public class DataMatrix
    {

        // refactor for more message length
        private static List<int> GetMatrixShape(int messageLength)
        {
            if (messageLength > 0 && messageLength <= 3)
            {
                return new List<int>() {10, 8};
            }
            else if (messageLength > 3 && messageLength <= 5)
            {
                return new List<int>() { 12, 10};
            }
            else
            {
                return new List<int>() { 14, 12};
            }
        }

        private static List<int[]> GetModules(Polynomial codedMessage)
        {
            var resultStr = new List<string>();
            var resultArr = new List<int[]>();

            foreach (double coefficient in codedMessage.Coefficients)
            {
                int i = 0;
                foreach (char bit in Convert.ToString((int)coefficient, 2))
                {
                    //resultArr.Append<int>((int)Char.GetNumericValue(bit));
                }
            }

            return resultArr;
        }

        // pack matrix with some algorithm
        private static List<int[]> GetMatrix(List<int[]> modules)
        {
            

            return new List<int[]>();
        }

        private static Bitmap ToBitmap(List<int[]> rawImage)
        {
            int width = rawImage[0].Length;
            int height = rawImage.Count;

            Bitmap Image = new Bitmap(width, height);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int color = rawImage[j][i];
                    Color rgb = new Color();
                    if (color == 0)
                    {
                        rgb = Color.White;
                    }
                    else
                    {
                        rgb = Color.Black;
                    }
                    Image.SetPixel(i, j, rgb);
                }
            return Image;
        }

        public static Image Encode(string message)
        {
            var codedMessage = ReedSolomon.Encode(message);
            var modules = GetModules(codedMessage);
            var matrix = GetMatrix(modules);

            return ToBitmap(matrix);
        }
    }
}
