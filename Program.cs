using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using System.Text;
using System.Drawing;
using System.IO;

namespace DataMatrixForms
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Image image = Image.FromFile(@"C:\Users\Artem Rozanov\Desktop\bitmap.png");
            var ms = new MemoryStream();

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            var bytes = ms.ToArray();

            var imageMemoryStream = new MemoryStream(bytes);

            Image imgFromStream = Image.FromStream(imageMemoryStream);

        }
    }
}
