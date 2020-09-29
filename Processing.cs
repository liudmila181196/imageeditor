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
    public partial class Processing : Form
    {
        public Processing()
        {

        }

        public PointPairList deleteShift(PointPairList list)
        {
            PointPairList listNew = new PointPairList();
            int N = list.Count();
            Analysis analysis = new Analysis();
            double m = analysis.calcMean(list);
            for (int i = 0; i < N; i++)
            {
                listNew.Add(i, list.ElementAt(i).Y - m);
            }
            return listNew;
        }

        public PointPairList antiSpike(PointPairList list, double min, double max)
        {
            PointPairList listNew = new PointPairList();
            int N = list.Count();
            double x = 0;
            for (int i = 0; i < N; i++)
            {
                if (list.ElementAt(i).Y < min || list.ElementAt(i).Y > max)
                {
                    if (i == 0) listNew.Add(i, list.ElementAt(i + 1).Y);
                    else if (i == N - 1) listNew.Add(i, list.ElementAt(i - 1).Y);
                    else
                    {
                        x = (list.ElementAt(i - 1).Y + list.ElementAt(i + 1).Y) / 2;
                        listNew.Add(i, x);
                    }
                }
                else listNew.Add(i, list.ElementAt(i).Y);
            }
            return listNew;
        }

        public PointPairList antiTrend(PointPairList list, int L)
        {
            PointPairList listNew = new PointPairList();
            PointPairList listOfMeans = new PointPairList();
            int N = list.Count();
            double sum = 0, x = 0;
            for (int i = 0; i < L; i++)
            {
                for (int j = 0; j < N / L; j++)
                {
                    sum += list.ElementAt(j + i * N / L).Y;
                }
                sum /= N / L;
                listOfMeans.Add(i, sum);
                sum = 0;
            }

            for (int i = 0; i < L; i++)
            {
                for (int j = 0; j < N / L; j++)
                {
                    x = list.ElementAt(j + i * N / L).Y - listOfMeans.ElementAt(i).Y;
                    listNew.Add((j + i * N / L), x);
                }
            }

            return listNew;
        }

        //Фильтр Низких Частот
        public PointPairList lpf(int m, double dt, double fc)
        {
            PointPairList lpw = new PointPairList();
            PointPairList lpw2 = new PointPairList();
            double arg = 2 * fc * dt;
            lpw.Add(0, arg);
            lpw2.Add(0, arg);
            arg *= Math.PI;
            for (int i = 1; i <= m; i++)
            {
                if (i == m)
                    lpw.Add(i, Math.Sin(arg * i) / 2 * (Math.PI * i));
                else
                    lpw.Add(i, Math.Sin(arg * i) / (Math.PI * i));
            }
            double[] d = { 0.35577019, 0.24369830, 0.07211497, 0.00630165 };
            double sum = lpw.ElementAt(0).Y;
            double sum2 = 0;
            for (int i = 1; i <= m; i++)
            {
                sum2 = d[0];
                arg = Math.PI * i / m;
                for (int k = 1; k <= 3; k++)
                {
                    sum2 += 2 * d[k] * Math.Cos(arg * k);
                }

                lpw2.Add(i, lpw.ElementAt(i).Y * sum2);
                sum += 2 * lpw2.ElementAt(i).Y;
            }
            //нормировка
            lpw2.ForEach(pp => { pp.Y /= sum; });
            PointPairList lpw3 = new PointPairList();
            //отражение
            for (int i = 0; i <= 2 * m; i++)
            {
                if (i <= m) lpw3.Add(i, lpw2.ElementAt(m - i).Y);
                else if (i > m) lpw3.Add(i, lpw2.ElementAt(i - m).Y);
            }
            return lpw3;
        }

        //Фильтр Высоких Частот
        public PointPairList hpf(int m, double dt, double fc)
        {
            PointPairList lpw = lpf(m, dt, fc);
            PointPairList hpw = new PointPairList();

            for (int i = 0; i < 2 * m; i++)
            {
                if (i == m) hpw.Add(i, (1 - lpw.ElementAt(i).Y));
                else hpw.Add(i, -lpw.ElementAt(i).Y);
            }

            return hpw;
        }
        //Полосовой фильтр
        public PointPairList bpf(int m, double dt, double fc1, double fc2)
        {
            PointPairList lpw1 = lpf(m, dt, fc1);
            PointPairList lpw2 = lpf(m, dt, fc2);
            PointPairList bpw = new PointPairList();

            for (int i = 0; i < 2 * m; i++)
            {
                bpw.Add(i, (lpw2.ElementAt(i).Y - lpw1.ElementAt(i).Y));
            }

            return bpw;
        }

        //Режекторный фильтр
        public PointPairList bsf(int m, double dt, double fc1, double fc2)
        {
            if (fc1 > fc2)
            {
                double a = fc1;
                fc1 = fc2;
                fc2 = a;
            }
            PointPairList lpw1 = lpf(m, dt, fc1);
            PointPairList lpw2 = lpf(m, dt, fc2);
            PointPairList bsw = new PointPairList();

            for (int i = 0; i < 2 * m; i++)
            {
                if (i == m) bsw.Add(i, (1 + lpw1.ElementAt(i).Y - lpw2.ElementAt(i).Y));
                else bsw.Add(i, (lpw1.ElementAt(i).Y - lpw2.ElementAt(i).Y));
            }

            return bsw;
        }

        public Bitmap nearestNeighbors(Image image, double kW, double kH)
        {
            int w = (int)(image.Width * kW), h = (int)(image.Height * kH);
            Bitmap im = new Bitmap(image);
            Bitmap imageNew = new Bitmap(w, h);

            for (double x = 0; x < w; x++)
            {
                for (double y = 0; y < h; y++)
                {
                    Color pixelColor = im.GetPixel((int)(x / kW), (int)(y / kH));
                    imageNew.SetPixel((int)x, (int)y, pixelColor);
                }
            }

            return imageNew;
        }
        public Bitmap nearestNeighbors(Image image, int neww, int newh)
        {
           
            Bitmap im = new Bitmap(image);
            Bitmap imageNew = new Bitmap(neww, newh);
            double kW = neww / (double)image.Width, kH = newh / (double)image.Height;
            for (double x = 0; x < neww; x++)
            {
                for (double y = 0; y < newh; y++)
                {
                    Color pixelColor = im.GetPixel((int)(x / kW), (int)(y / kH));
                    imageNew.SetPixel((int)x, (int)y, pixelColor);
                }
            }

            return imageNew;
        }
        public Bitmap bilinearInterp(Image image, double kW, double kH)
        {
            float tmp;
            int red, green, blue, h, w;
            int neww = (int)(image.Width * kW), newh = (int)(image.Height * kH);
            Bitmap im = new Bitmap(image);
            Bitmap imageNew = new Bitmap(neww, newh);

            for (int j = 0; j < newh; j++){
                tmp = (float)j / (float)(newh - 1) * (image.Height - 1);
                h = (int)Math.Floor(tmp);
                if (h < 0) h = 0;
                else if (h >= image.Height - 1) h = image.Height - 2;

                for (int i = 0; i < neww; i++){
                    tmp = (float)(i) / (float)(neww - 1) * (image.Width - 1);
                    w = (int)Math.Floor(tmp);
                    if (w < 0) w = 0;
                    else if (w >= image.Width - 1) w = image.Width - 2;

                    Color pix1 = im.GetPixel(w, h);
                    Color pix2 = im.GetPixel(w, h + 1);
                    Color pix3 = im.GetPixel(w + 1, h + 1);
                    Color pix4 = im.GetPixel(w + 1, h);
                    
                    blue = ((int)pix1.B + (int)pix2.B + (int)pix3.B + (int)pix4.B)/4;
                    green =(((int)pix1.G + (int)pix2.G + (int)pix3.G + (int)pix4.G)/4);
                    red =  (((int)pix1.R + (int)pix2.R + (int)pix3.R + (int)pix4.R)/4);

                    Color newPix = Color.FromArgb(red, green, blue);
                    imageNew.SetPixel(i, j, newPix);
                }
            }
            return imageNew;
        }
        public Bitmap bilinearInterp(Image image, int neww, int newh)
        {
            float tmp;
            int red, green, blue, h, w;
            Bitmap im = new Bitmap(image);
            Bitmap imageNew = new Bitmap(neww, newh);

            for (int j = 0; j < newh; j++)
            {
                tmp = (float)j / (float)(newh - 1) * (image.Height - 1);
                h = (int)Math.Floor(tmp);
                if (h < 0) h = 0;
                else if (h >= image.Height - 1) h = image.Height - 2;

                for (int i = 0; i < neww; i++)
                {
                    tmp = (float)(i) / (float)(neww - 1) * (image.Width - 1);
                    w = (int)Math.Floor(tmp);
                    if (w < 0) w = 0;
                    else if (w >= image.Width - 1) w = image.Width - 2;

                    Color pix1 = im.GetPixel(w, h);
                    Color pix2 = im.GetPixel(w, h + 1);
                    Color pix3 = im.GetPixel(w + 1, h + 1);
                    Color pix4 = im.GetPixel(w + 1, h);

                    blue = ((int)pix1.B + (int)pix2.B + (int)pix3.B + (int)pix4.B) / 4;
                    green = (((int)pix1.G + (int)pix2.G + (int)pix3.G + (int)pix4.G) / 4);
                    red = (((int)pix1.R + (int)pix2.R + (int)pix3.R + (int)pix4.R) / 4);

                    Color newPix = Color.FromArgb(red, green, blue);
                    imageNew.SetPixel(i, j, newPix);
                }
            }
            return imageNew;
        }
        public int calcGrayScale(double f, double min, double max)
        {
            double g = (f - min) / (max - min)*255.0;
            //if (g < 0) g = 0;
            return (int) g;
        }

        public PointPairList[] grayScale(PointPairList[] list, int h, int w)
        {
            PointPairList[] listNew = new PointPairList[h];
            for (int i = 0; i < h; i++)
            {
                listNew[i] = new PointPairList();
            }
            double max = list[0].ElementAt(0).Y;
            double min = list[0].ElementAt(0).Y;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (min > list[i].ElementAt(j).Y) min = list[i].ElementAt(j).Y;
                    if (max < list[i].ElementAt(j).Y) max = list[i].ElementAt(j).Y;
                }
            }
            
            for (int i = 0; i < h; i++)
            {
                for (int j=0; j < w; j++)
                {
                    listNew[i].Add(j, calcGrayScale(list[i].ElementAt(j).Y,  min,  max));
                }
            }
            return listNew;
        }

        private int[] minmaxARGB(Bitmap bmp)
        {
            int XminR = 0;
            int XmaxR = 255;
            int XmaxG = 0;
            int XminG = 255;
            int XmaxB = 0;
            int XminB = 255;
            int XmaxA = 0;
            int XminA = 255;


            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixColor = bmp.GetPixel(i, j);

                    if (XminR > pixColor.R) XminR = pixColor.R;
                    if (XmaxR < pixColor.R) XmaxR = pixColor.R;
                    if (XminG > pixColor.G) XminG = pixColor.G;
                    if (XmaxG < pixColor.G) XmaxG = pixColor.G;
                    if (XminB > pixColor.B) XminB = pixColor.B;
                    if (XmaxB < pixColor.B) XmaxB = pixColor.B;
                    if (XminA > pixColor.A) XminA = pixColor.A;
                    if (XmaxA < pixColor.A) XmaxA = pixColor.A;
                }
            }
            int[] a = { XminR, XmaxR, XminG,XmaxG, XminB, XmaxB, XminA,XmaxA  };
            return a;
        }

        public Bitmap negative(Image image)
        {
            Bitmap im = new Bitmap(image);
            Bitmap imNew = new Bitmap(image.Width, image.Height);
            

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    imNew.SetPixel(i, j, Color.FromArgb(im.GetPixel(i, j).A, 255 - im.GetPixel(i, j).R, 255 - im.GetPixel(i, j).G, 255 - im.GetPixel(i, j).B));
                }
            }

            return imNew;
        }

        public Bitmap logarifm(Image image, double C)
        {
            Bitmap im = new Bitmap(image);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {

                    im.SetPixel(i, j, Color.FromArgb((int)(C*Math.Log(1 + im.GetPixel(i, j).A)), (int)(C * Math.Log(1 + im.GetPixel(i, j).R)), (int)(C * Math.Log(1 + im.GetPixel(i, j).G)), (int)(C * Math.Log(1 + im.GetPixel(i, j).B))));
                }
            }

            return im;
        }

        public Bitmap gamma(Image image, double C, double gamma)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);
            int r,g,b,a;
            int[] rgb = minmaxARGB(im);
            
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = im.GetPixel(i, j);
                    a = (int)(C * Math.Pow(pixel.A, gamma));
                    r = (int)(C * Math.Pow(pixel.R, gamma));
                    g= (int)(C * Math.Pow(pixel.G, gamma));
                    b= (int)(C * Math.Pow(pixel.B, gamma));
                    newIm.SetPixel(i, j, Color.FromArgb(a,r,g,b));
                }
            }
            return newIm;
        }

        public Bitmap corrCDF(Image image, PointPairList list)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color p = im.GetPixel(i, j);
                    int x = (int)p.R;
                    double y = list.ElementAt(x).Y;
                    x = (int) (255.0*y);
                    newIm.SetPixel(i, j, Color.FromArgb(x, x, x));
                }
            }
            return newIm;
        }


        public PointPairList derivative(PointPairList list, double h)
        {
            PointPairList listD = new PointPairList();
            for (int i=1; i < list.Count-1; i++)
            {
                listD.Add(i, (list.ElementAt(i + 1).Y - list.ElementAt(i - 1).Y) / (2 * h));
            }
            return listD;
        }

        public PointPairList[] imageToList (Image image)
        {
            Bitmap img = new Bitmap(image);
            PointPairList[] imgList = new PointPairList[img.Height];
            for (int i=0; i < img.Height; i++)
            {
                imgList[i] = new PointPairList();
            }
            for (int i=0; i<img.Height; i++)
            {
                for (int j=0; j<img.Width; j++)
                {
                    imgList[i].Add(j, img.GetPixel(j, i).R);
                }
            }
            return imgList;
        }

        public Bitmap listToImage (PointPairList[] list, int height, int width)
        {
            Bitmap img =new Bitmap(width, height);
            int x = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    x = (int) list[i].ElementAt(j).Y;
                    img.SetPixel(j, i, Color.FromArgb(x,x,x));
                }
            }
            return img;
        }

        public PointPairList[] normalNoise(PointPairList[] img, int w, int h, double p)
        {
            int N = w * h;
            Random rand = new Random();
            Model m = new Model();
            PointPairList list = m.buildRandom(0, 255, N);
            for (int i=0; i<h; i++)
            {
                for (int j=0; j<w; j++)
                {
                    img[i].ElementAt(j).Y += list.ElementAt(i*j + j).Y*p;
                }
            }
            return img;
        }
        
        public PointPairList[] saltPepperNoise(PointPairList[] img, int w, int h, double p)
        {
            int N = w * h;
            int n = (int)(N * p);
            Random rand = new Random();
            int t = 0, x, y;
            Model m = new Model();
            PointPairList list = m.buildRandom(0, 255, n);
            while (t < n)
            {
                x = rand.Next(0, w);
                y = rand.Next(0, h);
                if (list.ElementAt(t).Y > 127) img[y].ElementAt(x).Y = 0;
                else img[y].ElementAt(x).Y = 255;
                t++;
            }
            return img;
        }

        public PointPairList[] summaryNoise(PointPairList[] img, int w, int h, double p)
        {
            int N = w * h;
            int n = (int)(N * p);
            Random rand = new Random();
            int t = 0, x, y;
            Model m = new Model();
            PointPairList listSP = m.buildRandom(0, 255, n);
            PointPairList listN = m.buildRandom(0, 255, N);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    img[i].ElementAt(j).Y += listN.ElementAt(i * j + j).Y * p;
                }
            }
            while (t < n)
            {
                x = rand.Next(0, w);
                y = rand.Next(0, h);
                if (listSP.ElementAt(t).Y > 127) img[y].ElementAt(x).Y = 0;
                else img[y].ElementAt(x).Y = 255;
                t++;
            }
            return img;
        }

        //усреднящий m=0, медианный m=1
        public Bitmap MMFilter(Image image, int k, int m)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);
            
            int n = (int)(k - 1) / 2;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList a = window(im, n, i, j);
                    int p = 0;
                    if (m == 0) p = (int)a.Average(x => x.X);
                    else if (m==1)
                    {
                        a.Sort();
                        p = (int)a.ElementAt((int)((k * k - 1) / 2)).X;
                    }
                    newIm.SetPixel(i, j, Color.FromArgb(p,p,p));
                }
            }
            return newIm;
        }

        private PointPairList window(Bitmap im, int n, int i, int j)
        {
            PointPairList a = new PointPairList();
            int c = 0;
            for (int x = -n; x <= n; x++)
            {
                for (int y = -n; y <= n; y++)
                {
                    a.Add(im.GetPixel(i + x, j + y).R, c);
                    c++;
                }
            }
            return a;
        }

        private PointPairList[] window2(Bitmap im, int n, int i, int j)
        {
            PointPairList[] a = new PointPairList[2*n];
            int z=0, q=0;
            
            for (int x = -n; x < n; x++)
            {
                a[z] = new PointPairList();
                for (int y = -n; y < n; y++)
                {
                    a[z].Add(im.GetPixel(i + x, j + y).R, q);
                    
                    q++;
                }
                z++;
                q = 0;
            }
            return a;
        }

        public Bitmap globalThresholdConversion(Image image, double T0)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(im.Width, im.Height);
            /*
            Analysis analysis = new Analysis();
            PointPairList histo = analysis.imgHistogram(im);
            double T = ((histo.Min(x => x.X) + histo.Max(x => x.X)) / 2);
            double T1 = 0;
            while (Math.Abs(T - T1)>T0)
            {
                PointPairList G1 = new PointPairList();
                PointPairList G2 = new PointPairList();
                for (int i=0; i<histo.Count(); i++)
                {
                    if (histo.ElementAt(i).X < T) G1.Add(histo.ElementAt(i).X, histo.ElementAt(i).Y);
                    else G2.Add(histo.ElementAt(i).X, histo.ElementAt(i).Y);
                }
                double m1, m2;
                m1 = G1.Average(x => x.Y);
                m2 = G2.Average(x => x.Y);
                T1 = (int)(m1 + m2) / 2;
            }
            */
            for (int i=0; i < im.Width; i++)
            {
                for (int j=0; j < im.Height; j++)
                {
                    if (im.GetPixel(i, j).R <= T0) newIm.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    else newIm.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
            }

            return newIm;
        }

        public Bitmap globalThresholdConversion(Image image, double T0, int min, int max)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(im.Width, im.Height);
            for (int i = 0; i < im.Width; i++)
            {
                for (int j = 0; j < im.Height; j++)
                {
                    if (im.GetPixel(i, j).R <= T0) newIm.SetPixel(i, j, Color.FromArgb(min, min, min));
                    else newIm.SetPixel(i, j, Color.FromArgb(max, max, max));
                }
            }

            return newIm;
        }

        public PointPairList[] rotateList (PointPairList[] list, int h, int w)
        {
            PointPairList[] newList = new PointPairList[w];
            for (int i=0; i<w; i++)
            {
                newList[i] = new PointPairList();
            }
            for (int i=0; i < h;i++)
            {
                for (int j=0; j<w; j++)
                {
                    newList[j].Add(i, list[i].ElementAt(j).Y);
                }
            }
            return newList;
        }

        public PointPairList[] subtractionList (PointPairList[] list1, PointPairList[] list2, int h, int w)
        {
            for (int i=0; i<h; i++)
            {
                for (int j=0; j<w; j++)
                {
                    list1[i].ElementAt(j).Y = Math.Abs(list1[i].ElementAt(j).Y - list2[i].ElementAt(j).Y);
                }
            }

            return list1;
        }
        public PointPairList[] subtractionDiffList(PointPairList[] list1, PointPairList[] list2, int h1, int w1, int h2, int w2)
        {
            for (int i = (h1-h2)/2; i < h2; i++)
            {
                for (int j = (w1 - w2) / 2; j < w2; j++)
                {
                    list1[i].ElementAt(j).Y = Math.Abs(list1[i].ElementAt(j).Y - list2[i].ElementAt(j).Y);
                }
            }

            return list1;
        }
        public int maxPixel(Bitmap im)
        {
            int max = 0;
            for (int i=0; i < im.Width; i++)
            {
                for (int j=0; j < im.Height; j++)
                {
                    if (im.GetPixel(i, j).R > max) max = im.GetPixel(i, j).R;
                }
            }
            return max;
        }

        public Bitmap laplasian(Image image, int k, double t)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);
            int T = (int)(maxPixel(im)*t);
            int n = (int)(k - 1) / 2;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList a = window(im, n, i, j);
                    int sum = (int)(a.Sum(x => x.X)-(k*k)*a.ElementAt((int)((k * k - 1) / 2)).X);
                    if(Math.Abs(sum)>=T) newIm.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else newIm.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return newIm;
        }

        public PointPairList[] gradient(Image image, int k)
        {
            Bitmap im = new Bitmap(image);
            PointPairList[] newIm = imageToList(im);

            int n = (int)(k - 1) / 2;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList a = window(im, n, i, j);
                    int sum = Math.Abs(sobelX(a))+Math.Abs(sobelY(a));
                    newIm[j].ElementAt(i).Y=sum;
                }
            }

            return newIm;
        }

        public int sobelX(PointPairList a)
        {
            int x = (int)(a.ElementAt(6).X + 2 * a.ElementAt(7).X + a.ElementAt(8).X - (a.ElementAt(0).X + 2 * a.ElementAt(1).X + a.ElementAt(2).X));
            return x;
        }
        public int sobelY(PointPairList a)
        {
            int x = (int)(a.ElementAt(2).X + 2 * a.ElementAt(5).X + a.ElementAt(8).X - (a.ElementAt(0).X + 2 * a.ElementAt(3).X + a.ElementAt(6).X));
            return x;
        }

        public Bitmap dilatation(Image image, int[] primitive, int k)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);

            int n = (int)(k - 1) / 2;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList a = window(im, n, i, j);
                    int c = 0;
                    for (int z = 0; z < primitive.Length; z++)
                        if (a.ElementAt(z).X >= primitive[z]) c++;
                    if (c >k-1)
                    {
                        for (int x = -n; x <= n; x++)
                        {
                            for (int y = -n; y <= n; y++)
                            {
                                newIm.SetPixel(i + x, j + y, Color.FromArgb(255, 255, 255));
                            }
                        }
                    }
                    else newIm.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return newIm;
        }

        public Bitmap erosion(Image image, int[] primitive, int k)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);

            int n = (int)(k - 1) / 2;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList a = window(im, n, i, j);
                    int c = 0;
                    for (int z = 0; z < primitive.Length; z++)
                        if (a.ElementAt(z).X >= primitive[z]) c++;
                    if (c == primitive.Length) newIm.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else newIm.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return newIm;
        }

        public Bitmap erosion2(Image image, PointPairList[] primitive, int k, int vol, double percentIn, double percentOut)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);

            int n = k/ 2;
            int maxC = 0;
            for (int z = 0; z < k; z++)
            {
                for (int q = 0; q < k; q++)
                {
                    if (primitive[z].ElementAt(q).Y == vol ) maxC++;
                }
            }
            int maxV = k * k - maxC;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList[] a = window2(im, n, i, j);
                    int c = 0, v=0;
                    for (int z = 0; z < k; z++)
                    {
                        for (int q = 0; q < k; q++)
                        {
                            if (primitive[z].ElementAt(q).Y == vol && a[z].ElementAt(q).X==255) c++;
                            else if (primitive[z].ElementAt(q).Y != vol && a[z].ElementAt(q).X==255) v++; 
                        }
                    }

                    if (c >= maxC * percentIn && c <= maxC && v < maxV * percentOut) newIm.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else newIm.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return newIm;
        }

        public Bitmap erosion3(Image image, PointPairList[] primitive, int k, double vol)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);
            //добавить обработку крайних значений
            int n = k / 2;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList[] a = window2(im, n, i, j);
                    int min = 255;
                    for (int z = 0; z < k; z++)
                    {
                        for (int q=0; q<k; q++)
                        {
                            if (primitive[z].ElementAt(q).Y == vol && min > a[z].ElementAt(q).X) min = (int) a[z].ElementAt(q).X;
                        }
                    }
                    newIm.SetPixel(i, j, Color.FromArgb(min, min, min));
                }
            }
            return newIm;
        }

        public Bitmap dilatation3(Image image, PointPairList[] primitive, int k, double vol)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);
            //добавить обработку крайних значений
            int n = k / 2;
            for (int i = n; i < image.Width - n; i++)
            {
                for (int j = n; j < image.Height - n; j++)
                {
                    PointPairList[] a = window2(im, n, i, j);
                    int max=0;
                    for (int z = 0; z < k; z++)
                    {
                        for (int q = 0; q < k; q++)
                        {
                            if (primitive[z].ElementAt(q).Y == vol && max < a[z].ElementAt(q).X) max = (int)a[z].ElementAt(q).X;
                        }
                    }
                    newIm.SetPixel(i, j, Color.FromArgb(max, max, max));
                }
            }
            return newIm;
        }

        public Bitmap dilatation2 (Image im, int maskSize)
        {
            int[] mask = new int[maskSize * maskSize];
            for (int i = 0; i < maskSize * maskSize; i++)
            {
                mask[i] = 255;
            }
            Image imDil = dilatation(im, mask, maskSize);
            PointPairList[] imDilList = imageToList(imDil);
            Bitmap image = listToImage(imDilList, im.Height, im.Width);
            return image;
        }

        public Bitmap substractImAndMask(Image im, Image mask)
        {
            Bitmap bim = new Bitmap(im);
            Bitmap bmask = new Bitmap(mask);
            for (int i=0; i < im.Width; i++)
            {
                for (int j=0; j<im.Height; j++)
                {
                    if (bmask.GetPixel(i, j).R <50) bim.SetPixel(i, j, Color.FromArgb(0,0,0));
                }
            }
            return bim;
        }

        public Bitmap numberOfStones (Image image, PointPairList[] primitive, int k, int vol)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);
            int n = k / 2;
            int maxC = 0, num=0;
            for (int z = 0; z < k; z++)
            {
                for (int q = 0; q < k; q++)
                {
                    if (primitive[z].ElementAt(q).Y == vol) maxC++;
                }
            }
            
            for (int i = n; i < image.Width-n; i++)
            {
                for (int j = n; j < image.Height-n; j++)
                {
                    PointPairList[] a = window2(im, n, i, j);
                    int c = 0, v=0;
                    for (int z = 0; z < k; z++)
                    {
                        for (int q = 0; q < k; q++)
                        {
                            if (primitive[z].ElementAt(q).Y == vol && a[z].ElementAt(q).X == 255) c++;
                            else if (primitive[z].ElementAt(q).Y != vol && a[z].ElementAt(q).X == 255) v++;
                        }
                    }

                    if (c == maxC && v == 0) { newIm.SetPixel(i, j, Color.FromArgb(255, 0, 0)); num++; }
                    else newIm.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            Console.WriteLine("Количество объектов, у которых размер по каждому из направлений равен 12: " + num);
            return newIm;
        }

        public Bitmap numberOfStones2(Image image, PointPairList[] primitive, int k, int vol)
        {
            Bitmap im = new Bitmap(image);
            Bitmap newIm = new Bitmap(image.Width, image.Height);
            int n = k / 2;
            int num = 0;
            for (int i = n + 1; i < image.Width - n; i += k)
            {
                for (int j = n + 1; j < image.Height - n; j += k)
                {
                    int c = 0;
                    PointPairList[] a = window2(im, n, i, j);
                    for (int z = 0; z < k; z++)
                    {
                        for (int q = 0; q < k; q++)
                        {
                            if (a[z].ElementAt(q).X == 255) c++;
                        }
                    }

                    if (c >= 1)
                    {
                        num++;
                        for (int x = -n + i; x < n + i; x++)
                        {
                            for (int y = -n + j; y < n + j; y++)
                            {
                                newIm.SetPixel(x, y, im.GetPixel(x, y));
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Количество объектов, у которых размер хотя бы по одному направлению равен 12: " + num);
            return newIm;
        }
    }
}
