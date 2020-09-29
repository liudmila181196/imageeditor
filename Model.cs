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
	public partial class Model : Form
	{
		public Model ()
		{
			
		}
       
        private double f1 (double t, double a, double b)
		{
			return (a*t+b);
		}
        private double f2 (double t, double a, double b)
        {
            return b * Math.Pow(Math.E, a * t);
        }

		public PointPairList trend (double a, double b, bool f, int N)
		{
			PointPairList list = new PointPairList ();

			for (int t = 0; t <= N; t ++)
			{
                if (f)
                    list.Add (t, f1 (t, a, b));
                else list.Add(t, f2(t, a, b));
            }
            return list;
		}

        public PointPairList trendComp(double a, double alpha, double b, double beta, int min, int max)
        {
            PointPairList list = new PointPairList();

            for (int t = min; t <= max; t++)
            {
                if (t<=333)
                    list.Add(t, f2(t, alpha, beta));
                else if (t>333 && t<=666) list.Add(t, f1(t, a, b));
                else list.Add(t, f1(t, -a, b));
            }
            return list;
        }

        private double fr2(double pi1, double pi2, int i, int j)
        {
            double a = ((pi1)%(double)((i+1)%(j-0.78)*10)+ (pi2) % (double)((i+1) % (j + 0.11) * 10)-7.5)/7.5;
            return a;
        }

        public PointPairList buildSelfRandom(double max, int N)
        {
            PointPairList list = new PointPairList();
            int[] pi = new int[11];
            pi = Math.PI.ToString().ToCharArray(2,11).Select(x => x - '0').ToArray();

            for (int i = 0; i < 10; i++) pi[i]++;

            for (int i = 0; i < N/10; i ++)
            {
                for (int j = 0; j < 10; j++) {
                    list.Add(i*10+j, fr2(pi[j], pi[j+1],i, j)*max);
                }
            }
            return list;
        }

        public PointPairList addSpikes(PointPairList list)
        {
            Random rnd = new Random();
            double max = list.Max(point => point.Y), min = list.Min(point => point.Y);
            int N = list.Count();
            PointPairList listNew = new PointPairList();
            int n = N/100;
            int m = rnd.Next(1, n);
            int[] j=new int[10];
            for (int i=0; i < m; i++)
            {
                j[i] = rnd.Next(0, N);
            }
            double a = 0;
            bool b=true;
            for (int i = 0; i < N; i ++)
            {
                for (int k = 0; k <= m; k++)
                {
                    if (j[k] == i && j[k]!=0)
                    {
                        a = (rnd.NextDouble() * (max - min ) + min) * 100;
                        listNew.Add(i, list.ElementAt(i).Y + a);
                        b = false;
                    }
                }
                if (b) listNew.Add(i, list.ElementAt(i).Y);
                else b = true;
            }
            return listNew;
        }

        public PointPairList addSpikes(PointPairList list, double max, double min)
        {
            Random rnd = new Random();
            int N = list.Count();
            PointPairList listNew = new PointPairList();
            int n = N / 100;
            int m = rnd.Next(1, n);
            int[] j = new int[10];
            for (int i = 0; i < m; i++)
            {
                j[i] = rnd.Next(0, N);
            }
            double a = 0;
            bool b = true;
            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k <= m; k++)
                {
                    if (j[k] == i && j[k] != 0)
                    {
                        a = (rnd.NextDouble() * (max - min) + min) * 100;
                        listNew.Add(i, list.ElementAt(i).Y + a);
                        b = false;
                    }
                }
                if (b) listNew.Add(i, list.ElementAt(i).Y);
                else b = true;
            }
            return listNew;
        }

        public PointPairList shift(PointPairList list, int m, int n, double C)
        {
            Random rnd = new Random();
            int N = list.Count();
            PointPairList listNew = new PointPairList();
            double a = 0;
            for (int i = 0; i < N; i ++)
            {
                if (i >= m && i <= n)
                {
                    a = list.ElementAt(i).Y + C;
                    listNew.Add(i, a);
                }
                else listNew.Add(i, list.ElementAt(i).Y);
            }

            return listNew;
        }


        public PointPairList buildRandom(double min, double max, int N)
        {
            Random rnd = new Random();
            PointPairList list = new PointPairList();
            double a = 0;
            for (int i = 0; i < N; i ++)
            {
                a = rnd.NextDouble() * (max - min) + min;
                list.Add(i, a);
            }
            return list;
        }

        public PointPairList addLists(PointPairList list1, PointPairList list2)
        {
            PointPairList list = new PointPairList();
            int N = list1.Count();
            if (list1.Count != list2.Count) return list;
            for (int i=0; i<N; i++) list.Add(i, list1.ElementAt(i).Y+list2.ElementAt(i).Y);
            return list;
        }

        public PointPairList multiLists(PointPairList list1, PointPairList list2)
        {
            PointPairList list = new PointPairList();
            int N = list1.Count();
            if (list1.Count != list2.Count) return list;
            for (int i = 0; i < N; i++) list.Add(i, list1.ElementAt(i).Y * list2.ElementAt(i).Y);
            return list;
        }

        private PointPairList buildRandom(Random rnd, double min, double max, int N)
        {
            //Random rnd = new Random();
            PointPairList list = new PointPairList();
            double a = 0;
            for (int i = 0; i < N; i += 1)
            {
                a = rnd.NextDouble() * (max - min) + min;
                list.Add(i, a);
            }
            return list;
        }
        public PointPairList multiSumRandom(int N, int n, double min, double max)
        {
            Random rnd = new Random();
            PointPairList list1 = buildRandom(rnd, min, max, N);

            for (int i = 0; i < n - 1; i++)
            {
                PointPairList list2 = buildRandom(rnd, min, max, N);
                list1 = addLists(list1, list2);
            }

            PointPairList list = new PointPairList();

            for (int i = 0; i < N; i++)
                list.Add(i, list1.ElementAt(i).Y / n);
            return list;
        }
        private double fun(double a, double t, double f, int k)
        {
            return a * Math.Sin(2 * Math.PI * f * k * t);
        }  
        public PointPairList buildSin(double a, double t, double f, int N)
        {
            PointPairList list = new PointPairList();

            for (int k = 0; k < N; k++)
            {
                list.Add(k, fun(a, t, f, k));
            }
            return list;
        }

        public PointPairList buildSinCardio(double a, double t, double f, int N, double alpha)
        {
            PointPairList list = new PointPairList();

            for (int k = 0; k < N; k++)
            {
                list.Add(k, fun(a, t, f, k)*Math.Pow(Math.E, -alpha*k));
            }
            return list;
        }

        public PointPairList convolution(PointPairList list1, PointPairList list2)
        {
            PointPairList list = new PointPairList();
            int N = list1.Count();
            int M = list2.Count();
            if (N<M) { int a = N; N = M; M = a; }
            double sum = 0;

            for (int i=0; i<N; i++)
            {
                for (int j=0; j < M; j++)
                {
                    if ((i - j) >= 0 && (i-j)<N) sum += list1.ElementAt(i - j).Y * list2.ElementAt(j).Y;
                    else break;
                }
                list.Add(i, sum);
                sum = 0;
            }
            
            return list;
        }


    public PointPairList cropList (PointPairList list, int start, int end)
        {
            PointPairList listNew= new PointPairList();
            for (int i=start; i<end; i++)
            {
                listNew.Add(i-start, list.ElementAt(i).Y);
            }
            return listNew;
        }

    public PointPairList stickList(Dictionary<int, PointPairList> list)
        {
            PointPairList listNew = new PointPairList();
            PointPairList list0 = new PointPairList();
            int N = list.Count;
            int n = (list.ElementAt(0).Value).Count;
            for (int i=0; i<N; i++)
            {
                list0 = list.ElementAt(i).Value;
                for (int j=0; j < list0.Count; j++)
                {
                    listNew.Add(i*n+j, list0.ElementAt(j).Y);
                }
            }
            return listNew;
        }
    

    public PointPairList[] roundMask(int diameter, int frame, int volTrue, int volFalse)
    {
        frame=frame*2;
        PointPairList[] round = new PointPairList[diameter+frame];

        for (int i = 0; i < diameter + frame; i++)
        {
            round[i] = new PointPairList();
            for (int j = 0; j < diameter + frame; j++)
            { 
                int x = (int)Math.Sqrt(Math.Pow(i - (diameter +frame)/ 2, 2) + Math.Pow(j - (diameter + frame) / 2, 2));
                if (x < diameter / 2) round[i].Add(j, volTrue);
                else round[i].Add(j, volFalse);
            }
        }
        return round;
    
        }
    }
}