using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

using ZedGraph;
using System.Linq;

namespace Nazarova
{
    public partial class Analysis : Form
    {
        public Analysis()
        {
            
        }
        //Нахождение М средних значений в массиве list 
        public PointPairList meansOfIntervals(PointPairList list, int M)
        {
            PointPairList listOfMeans = new PointPairList();
            int N = list.Count();
            int n = N / M;
            double sum = 0;

            for (int i = 0; i < M; i += 1)
            {
                for (int j = 0; j < n; j++)
                {
                    sum += list.ElementAt(i * n + j).Y;
                }
                listOfMeans.Add(i, sum / n);
                sum = 0;
            }
            return listOfMeans;
        }
        //Нахождение отклонений в массиве значений на b %
        public String deviations(PointPairList list, double b)
        {
            double max = list.Max(p => p.Y);
            double min = list.Min(p => p.Y);
            int M = list.Count();
            double xmax = (max - min) / 2;
            double[] diff = new double[M];
            BigInteger k = 0;
            double maxDiff = b * xmax;

            for (int i = 0; i < M; i += 1)
            {
                for (int j = i + 1; j < M; j++)
                    if (Math.Abs(list.ElementAt(i).Y - list.ElementAt(j).Y) <= Math.Abs(maxDiff)) k++;
            }

            string diffStr = "";
            for (int i = 1; i < M; i++)
            {
                diff[i - 1] = Math.Round(list.ElementAt(i - 1).Y - list.ElementAt(i).Y, 3);
                diffStr += diff[i - 1].ToString() + " ";
            }

            BigInteger c = combinations(M, 2);
            double perc = (double)k / (double)c * 100;
            maxDiff = Math.Round(maxDiff, 3);
            String result = "Cовпадений: " + k + " из " + c + " = " + perc + " %\nДопустимое отклонение: " + maxDiff + "; \n Отклонения интеравалов: [" + diffStr + "]";
            return result;
        }
        //Комбинации (из теор. вероятности)
        private BigInteger combinations(int n, int k)
        {
            BigInteger factorialN = 1, factorialK = 1, factorialNK = 1;
            for (int i = 2; i <= n; i++) factorialN = factorialN * i;
            for (int i = 2; i <= k; i++) factorialK = factorialK * i;
            for (int i = 2; i <= n - k; i++) factorialNK = factorialNK * i;
            BigInteger f = factorialN / (factorialNK * factorialK);
            return f;
        }
        //дисперсия на М интервалах
        public PointPairList dispOfIntervals(PointPairList list, int M)
        {
            PointPairList listOfDisp = new PointPairList();
            int N = list.Count();
            int n = N / M;
            double sum = 0;
            PointPairList listOfMeans = meansOfIntervals(list, M);

            for (int i = 0; i < M; i += 1)
            {
                for (int j = 0; j < n; j++)
                    sum += Math.Pow((list.ElementAt(i * n + j).Y - listOfMeans.ElementAt(i).Y), 2);
                listOfDisp.Add(i, sum / n);
                sum = 0;
            }
            return listOfDisp;
        }
        //вычисление среднего значения
        public double calcMean(PointPairList list)
        {
            double mean = 0;
            int N = list.Count();
            for (int i = 0; i < N; i++)
                mean += list.ElementAt(i).Y;
            mean /= N;
            return mean;
        }
        //вычисление дисперсии
        public double calcDisp(PointPairList list)
        {
            double mean = 0, disp = 0;
            int N = list.Count();
            mean = calcMean( list);
            for (int i = 0; i < N; i++)
                disp += Math.Pow((list.ElementAt(i).Y - mean), 2);
            disp /= N;
            disp = Math.Sqrt(disp);
            return disp;
        }
        //гистограмма М значений
        public PointPairList buildHistogram(PointPairList list, int M)
        {
            int N = list.Count();
            double max = list.Max(p => p.Y);
            double min = list.Min(p => p.Y);
            double k = (max - min) / M;
            PointPairList HList = new PointPairList();
            int[] hist = new int [M];
            for (int i = 0; i < M; i++) hist[i] = 0;

            for (int i=0; i<N; i++)
            {
                for (int j=1; j <= M; j++)
                {
                    if (list.ElementAt(i).Y <= (min+j * k)&& list.ElementAt(i).Y > (min + (j-1) * k)) hist[j-1]++;
                }
            }

            for (int j = 1; j <= M; j++)
            {
                HList.Add((min + j * k), hist[j - 1]);
            }

            return HList;
        }
        //корреляция функции
        public PointPairList buildCorrelation(PointPairList list)
        {
            int N = list.Count();
            PointPairList CList = new PointPairList();
            double mean = calcMean(list), sum1 = 0, sum2 = 0;
            double[] Rxx = new double[N];

            for (int L = 0; L < N; L++)
            {
                for (int j = 0; j < N - L - 1; j++)
                {
                    sum1 += (list.ElementAt(j).Y - mean) * (list.ElementAt(j + L).Y - mean);
                }
                Rxx[L] = sum1;
                sum1 = 0;
                sum2 += Math.Pow((list.ElementAt(L).Y - mean), 2);
            }

            for (int L = 0; L < N - 1; L++)
            {
                CList.Add(L, Rxx[L] / sum2);
            }
            return CList;
        }
        //Взаимнокорреляционная функция
        public PointPairList buildCorrelation(PointPairList list1, PointPairList list2)
        {
            int N = list1.Count();
            PointPairList CList = new PointPairList();
            double mean1 = calcMean(list1), mean2=calcMean(list2), sum=0, sum1 = 0, sum2 = 0;
            double[] Rxx = new double[N];

            for (int L = 0; L < N; L++)
            {
                for (int j = 0; j < N - L - 1; j++)
                {
                    sum += (list1.ElementAt(j+L).Y - mean1) * (list2.ElementAt(j + L).Y - mean2);
                }
                Rxx[L] = sum;
                sum = 0; 
            }
            for (int i=0; i < N; i++)
            {
                sum1 += Math.Pow((list1.ElementAt(i).Y - mean1), 2);
                sum2 += Math.Pow((list2.ElementAt(i).Y - mean2), 2);
            }
            for (int L = 0; L < N; L++)
            {
                CList.Add(L, Rxx[L] / Math.Pow(sum1*sum2,0.5));
            }
            return CList;
        }

        private double calcRe(PointPairList list, int m, double N)
        {
            double re = 0;
            for (int k = 0; k < N; k++)
            {
                re += list.ElementAt(k).Y * Math.Cos((2 * Math.PI * k * m) / N);
            }
            return re / N;
        }

        private double calcIm(PointPairList list, int m, double N)
        {
            double im = 0;
            for (int k = 0; k < N; k++)
            {
                im += list.ElementAt(k).Y * Math.Sin((2 * Math.PI * k * m) / N);
            }
            return im / N;
        }
        //построение спектра Фурье, scale - шкала (при значении 2 - показывется только половина всего спекра т.к. он зеркально отражается относит Оу)
        public PointPairList buildSpecter(PointPairList list, double scale)
        {
            int N = list.Count();
            PointPairList listSpec = new PointPairList();
            PointPairList listSpec2 = new PointPairList();
            double x = 0, re = 0, im = 0;
            for (int m = 0; m < N; m++)
            {
                re = calcRe(list, m, N);
                im = calcIm(list, m, N);
                x = Math.Sqrt(Math.Pow(re, 2) + Math.Pow(im, 2));
                listSpec.Add(m, x);
            }

            for (int i = 0; i < N / scale; i++)
            {
                listSpec2.Add(i, listSpec.ElementAt(i).Y);
            }
            
            return listSpec2;
        }
        //Построение спектра фурье, df - домножение координат
        ///scale - шкала (при значении 2 - показывется только половина всего спекра т.к. он зеркально отражается относит Оу)
        public PointPairList buildSpecter(PointPairList list, double scale, double df)
        {
            int N = list.Count();
            PointPairList listSpec = new PointPairList();
            PointPairList listSpec2 = new PointPairList();
            double x = 0, re = 0, im = 0;
            for (int m = 0; m < N; m++)
            {
                re = calcRe(list, m, N);
                im = calcIm(list, m, N);
                x = Math.Sqrt(Math.Pow(re, 2) + Math.Pow(im, 2));
                listSpec.Add(m, x);
            }

            for (int i = 0; i < N / scale; i++)
            {
                listSpec2.Add((int)(((double)i)*df), listSpec.ElementAt(i).Y);
            }

            return listSpec2;
        }

        //Спектр Фурье без извлечения корня (для обратного преобразования нужен весь спектр)
        public PointPairList buildSpecter(PointPairList list)
        {
            int N = list.Count();
            PointPairList listSpec = new PointPairList();
            double x = 0, re = 0, im = 0;
            for (int m = 0; m < N; m++)
            {
                re = calcRe(list, m, N);
                im = calcIm(list, m, N);
                x = re+im;
                listSpec.Add(m, x);
            }

            return listSpec;
        }

        public PointPairList getComplex(PointPairList list)
        {
            int N = list.Count();
            double x = 0, re = 0, im = 0;
            PointPairList listX = new PointPairList();
            for (int m = 0; m < N; m++)
            {
                re = calcRe(list, m, N);
                im = calcIm(list, m, N);
                x = re+im;
                listX.Add(m, x);
            }
            return listX;
        }

        public double[] getComplex(PointPairList list, int m)
        {
            int N = list.Count();
            double re = 0, im = 0;
            re =  N*calcRe(list, m, N);
            im =  N*calcIm(list, m, N);
            double[] z = { re, im };
            return z;
        }


        public double divComplex(double[] z1, double[] z2)
        {
            double re=0, im=0;
            re = (z1[0]*z2[0]+z1[1]*z2[1]) / (Math.Pow(z2[0], 2) + Math.Pow(z2[1], 2));
            im = (z2[0]*z1[1]-z1[0]*z2[1]) / (Math.Pow(z2[0], 2) + Math.Pow(z2[1], 2));
            
            return re+im;
        }

        public double[] divComplex2(double[] z1, double[] z2)
        {
            double[] x= { 0, 0 };
            x[0] = (z1[0] * z2[0] + z1[1] * z2[1]) / (Math.Pow(z2[0], 2) + Math.Pow(z2[1], 2));
            x[1] = (z2[0] * z1[1] - z1[0] * z2[1]) / (Math.Pow(z2[0], 2) + Math.Pow(z2[1], 2));
            
            return x;
        }

        public double[] multiplyComplex(double[] z1, double[] z2)
        {
            double[] x = { 0, 0 };
            x[0] = z1[0]*z2[0]-z1[1]*z2[1];
            x[1] = z1[0]*z2[1]+z1[1]*z2[0];

            return x;
        }

        public PointPairList divComplexList(PointPairList list1, PointPairList list2)
        {
            int N = list1.Count();
            
            double[] a, b;
            double x;
            PointPairList listX = new PointPairList();
            for (int i=0; i < N; i++)
            {
                a = getComplex(list1, i);
                b = getComplex(list2, i);
                x = divComplex(a, b);
                listX.Add(i, x);
            }
            return listX;
        }

        public PointPairList divOptimalComplexList(PointPairList list1, PointPairList list2, double k)
        {
            int N = list1.Count();

            double[] a, b, c;
            double d;
            PointPairList listX = new PointPairList();
            for (int i = 0; i < N; i++)
            {
                a = getComplex(list1, i);
                b = getComplex(list2, i);
                b[1] = -b[1];
                c = multiplyComplex(a, b);
                d = Math.Pow(b[0], 2) + Math.Pow(b[1], 2) + k;
                c[0] /= d;
                c[1] /= d;
                listX.Add(i, c[0]+c[1]);
            }
            return listX;
        }
        //Построение гистограммы изображения по составляющей R
        public PointPairList imgHistogram (Image img)
        {
            PointPairList list = new PointPairList();
            Bitmap bit = new Bitmap(img);
            double size = bit.Width * bit.Height;
            for (int i=0; i<256; i++)
            {
                list.Add(i, 0);
            }
            for (int i=0; i<bit.Width; i++)
            {
                for (int j=0; j<bit.Height; j++)
                {
                    list.ElementAt(bit.GetPixel(i, j).R).Y++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                list.ElementAt(i).Y = list.ElementAt(i).Y / size;
            }
            return list;
        }
        //Построение гистограммы изображения, color=0 - Red, color=1 - Green, color=2 - Blue
        public PointPairList imgHistogram(Image img, int color)
        {
            PointPairList list = new PointPairList();
            Bitmap bit = new Bitmap(img);
            double size = bit.Width * bit.Height;
            for (int i = 0; i < 256; i++)
            {
                list.Add(i, 0);
            }

            if (color==0)
                for (int i = 0; i < bit.Width; i++)
                {
                    for (int j = 0; j < bit.Height; j++)
                    {
                        list.ElementAt(bit.GetPixel(i, j).R).Y++;
                    }
                }
            else if (color == 1)
                for (int i = 0; i < bit.Width; i++)
                {
                    for (int j = 0; j < bit.Height; j++)
                    {
                        list.ElementAt(bit.GetPixel(i, j).G).Y++;
                    }
                }
            
            else if (color == 2)
                for (int i = 0; i < bit.Width; i++)
                {
                    for (int j = 0; j < bit.Height; j++)
                    {
                        list.ElementAt(bit.GetPixel(i, j).B).Y++;
                    }
                }
            for (int i = 0; i < 256; i++)
            {
                list.ElementAt(i).Y = list.ElementAt(i).Y / size;
            }
            return list;
        }
        //Вычисление функции CDF изображения
        public PointPairList imgCDF(Image img)
        {
            PointPairList list = new PointPairList();
            Bitmap bit = new Bitmap(img);
            double cdf = 0;
            double size = bit.Width * bit.Height;
            for (int i = 0; i < 256; i++)
            {
                list.Add(i, 0);
            }
            for (int i = 0; i < bit.Width; i++)
            {
                for (int j = 0; j < bit.Height; j++)
                {
                    list.ElementAt(bit.GetPixel(i, j).R).Y++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                cdf += list.ElementAt(i).Y;
                list.ElementAt(i).Y = cdf / size;
            }

            return list;
        }
    }
}
