using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace DataMatrixForms
{

    public class ReedSolomon
    {
        private static readonly Polynomial FieldPolynomial = new Polynomial(1, 0, 1, 1, 0, 1, 0, 0, 1);

        private static List<Polynomial> GetField(int r)
        {

            // a up to n-k = r
            var a0 = new Polynomial(1, 0, 0, 0, 0, 0, 0, 0);
            var a1 = new Polynomial(0, 1, 0, 0, 0, 0, 0, 0);
            var a2 = new Polynomial(0, 0, 1, 0, 0, 0, 0, 0);
            var a3 = new Polynomial(0, 0, 0, 1, 0, 0, 0, 0);
            var a4 = new Polynomial(0, 0, 0, 0, 1, 0, 0, 0);
            var a5 = new Polynomial(0, 0, 0, 0, 0, 1, 0, 0);
            var a6 = new Polynomial(0, 0, 0, 0, 0, 0, 1, 0);
            var a7 = new Polynomial(0, 0, 0, 0, 0, 0, 0, 1);
            var a8 = new Polynomial(1, 0, 1, 1, 0, 1, 0, 0);

            List<Polynomial> resultField = new List<Polynomial> { a0, a1, a2, a3, a4, a5, a6, a7, a8 };

            for (int i = 9; i <= r; i++)
            {
                resultField.Add(MultiplyInField(resultField[i-1], a1));
            }

            return resultField;
        }

     
        private static List<Polynomial> GetG(int r)
        {
            List<Polynomial> resultG = new List<Polynomial>();
            // g up to n-k = r

            resultG.Add(new Polynomial(1, 0, 0, 0, 0, 0, 0, 0));

            for (int i = 1; i <= r; i++)
            {
                resultG.Add(new Polynomial(0, 0, 0, 0, 0, 0, 0, 0));
            }
            return resultG;
        }


        private static Polynomial NormalizePolynomial(Polynomial poly)
        {
            var normalizedPoly = new double[8];
            int i = 0;

            foreach (double value in poly.Coefficients)
            {

                if ( Math.Abs(value) % 2 == 0)
                {
                    normalizedPoly[i] = 0;
                }
                else if (Math.Abs(value) % 2 == 1)
                {
                    normalizedPoly[i] = 1;
                }
                else
                {
                    normalizedPoly[i] = value;
                }

                i++;
            }

            return new Polynomial(normalizedPoly);
        }

        private static Polynomial ModField(Polynomial poly) 
            => Polynomial.DivideRemainder(poly, FieldPolynomial).Item2;

        private static Polynomial MultiplyInField(Polynomial first, Polynomial second)
            => NormalizePolynomial(ModField(Polynomial.Multiply(first, second)));

        private static Polynomial PolynomialXOR(Polynomial first, Polynomial second)
        {
            var resultPoly = new double[8];

            for (int i = 0; i <= 7; i++)
            {
                if (( i > first.Coefficients.Length - 1 ) && (i > second.Coefficients.Length - 1)) {
                    resultPoly[i] = 0;
                }
                else if ((i > first.Coefficients.Length - 1) && !(i > second.Coefficients.Length - 1))
                {
                    resultPoly[i] = Convert.ToDouble(Convert.ToBoolean(second.Coefficients[i]));
                }
                else if (!(i > first.Coefficients.Length - 1) && (i > second.Coefficients.Length - 1))
                {
                    resultPoly[i] = Convert.ToDouble(Convert.ToBoolean(first.Coefficients[i]));
                }
                else {
                    var bit = (Convert.ToDouble(Convert.ToBoolean(first.Coefficients[i]) ^ Convert.ToBoolean(second.Coefficients[i])));
                    resultPoly[i] = bit;
                }
            }

            return new Polynomial(resultPoly);
        }

        private static Polynomial GetPolynoimalOfDegree(int degree)
        {
            var coefficients = new double[degree + 1];
            coefficients[degree] = 1;
            return new Polynomial(coefficients);
        }

        // n-k = r
        private static Polynomial GetFormingPolynomial(int r)
        {
            List<Polynomial> g = GetG(r);
            List<Polynomial> a = GetField(r);
            List<double> result = new List<double>();

            // i up to n-k
            for (int i = 1; i <=r; i++)
            {
                for (int j = r; j >= 1; j--)
                {
                    g[j] = PolynomialXOR(g[j-1], MultiplyInField(g[j], a[i]));
                }
                g[0] = MultiplyInField(g[0], a[i]);
            }

            foreach (Polynomial poly in g)
            {
                result.Add(PolynomialToDouble(poly));
            }

            return new Polynomial(result.ToArray());
        }

        private static Double[] StringEncode(string message)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(message);
            List<double> encodedMessage = new List<double>();
            int count = 0;
            byte previousNumberByte = 0;
            string previousNumberString = "";
            string presentNumberString = "";

            foreach (byte symbol in asciiBytes)
            {
                if (symbol >= 48 && symbol <= 57)
                {
                    if (count == 0)
                    {
                        count++;
                        previousNumberByte = symbol;
                        previousNumberString = Encoding.ASCII.GetString(new byte[] { symbol });
                    }
                    else
                    {
                        presentNumberString = Encoding.ASCII.GetString(new byte[] { symbol });
                        encodedMessage.Add(Convert.ToDouble(previousNumberString + presentNumberString) + 130);
                        count = 0;
                    };
                }
                else
                {
                    if (count == 1)
                    {
                        encodedMessage.Add((Double)(previousNumberByte + 1));
                        encodedMessage.Add((Double)(symbol + 1));
                        count = 0;
                    }
                    else
                    {
                        encodedMessage.Add((Double)(symbol + 1));
                        count = 0;
                    }
                }
            }

            return encodedMessage.ToArray();
        }

        // redo this method
        private static int[] GetSize(int messageLegth)
        {
            // n - word length, k - information
            int n = 12;
            int k = 5;

            return new int[] { n, k};
        }

        private static Double[] AddFiller(Double[] arrayToAdd, int number)
        {
            List<double> resultArray = new List<double>(arrayToAdd);

            if (number == 1)
            {
                resultArray.Add(129);
                return resultArray.ToArray();
            }

            for (int i = 2; i <= number; i++)
            {
                double R = ((149 * (Double)i) % 253) + 1;
                double C = (R + 129) % 254;
                resultArray.Add(C);
            }

            return resultArray.ToArray();
        }

        private static Polynomial DoubleToPolinomial(double value)
        {
            List<double> result = new List<double>();
            string binary = Convert.ToString((int)value, 2);

            foreach (char bit in binary)
            {
                result.Add(Char.GetNumericValue(bit));
            }
            result.Reverse();

            return new Polynomial(result.ToArray());
        }

        private static double PolynomialToDouble(Polynomial value)
        {
            var coefficients = new List<double>(value.Coefficients) ;
            var result = new StringBuilder();
            coefficients.Reverse();

            foreach (double bit in coefficients)
            {
                result.Append(bit);
            }
           
            return (Double)Convert.ToInt32(result.ToString(), 2);
        }

        private static Polynomial DevideRemainderInField(Polynomial dividend, Polynomial divisor) 
        {
            var dividendCoef = dividend.Coefficients;
            var divisorCoef = divisor.Coefficients;

            // остаток 
            var remainder = (double[])dividendCoef.Clone();
            // частное 
            var quotient = new double[remainder.Length - divisorCoef.Length + 1];

            for (int i = 0; i < quotient.Length; i++)
            {
                double coeff = remainder[remainder.Length - i - 1];

                for (int j = 0; j < divisorCoef.Length; j++)
                {
                    var coeffMultiplying = MultiplyInField(DoubleToPolinomial(coeff), DoubleToPolinomial(divisorCoef[divisorCoef.Length - j - 1]));
                    var first = coeffMultiplying;
                    var second = DoubleToPolinomial(remainder[remainder.Length - i - j - 1]);
                    var coeffXOR = PolynomialXOR(first, second);
                    var coeffDouble = PolynomialToDouble(coeffXOR);
                    remainder[remainder.Length - i - j - 1] = coeffDouble;  
                }
            }


            return new Polynomial(remainder);
        }


        public static Polynomial Encode(string message)
        {
            var n = GetSize(message.Length)[0];
            var k = GetSize(message.Length)[1];
            var formingPolynomial = GetFormingPolynomial(n - k);
            var codedAsciiMessage = StringEncode(message);

            if (k - codedAsciiMessage.Length != 0)
            {
                codedAsciiMessage = AddFiller(codedAsciiMessage, k - codedAsciiMessage.Length);
            }
            Array.Reverse(codedAsciiMessage);

            var polynomialToEncode = Polynomial.Multiply(new Polynomial(codedAsciiMessage), GetPolynoimalOfDegree(n - k));

            var codedMessage = DevideRemainderInField(polynomialToEncode, formingPolynomial);
            
            



            return new Polynomial(codedAsciiMessage);
        }


    }
}
