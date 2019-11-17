using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using System.Text;

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
            ///Application.EnableVisualStyles();
            ///Application.SetCompatibleTextRenderingDefault(false);
            ///Application.Run(new Form1());
            var dividend = new double[] {0, 0, 0, 0, 0, 0, 0, 129, 115, 99, 98, 73 };
            var divisor = new double[] {23, 68, 144, 134, 240, 92, 254, 1};
            var poly = ReedSolomon.DevideRemainderInField(new Polynomial(dividend), new Polynomial(divisor));
            Console.WriteLine(ReedSolomon.GetFormingPolynomial(7));
            //Console.WriteLine(poly.ToString());
            //Console.WriteLine(ReedSolomon.PolynomialToDouble(new Polynomial(0, 1, 0, 1, 1)));
        }
    }
}
