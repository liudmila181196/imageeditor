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
using System.IO;

namespace Nazarova
{
    public partial class RdWr : Form
    {

        public RdWr(Dictionary<PointPairList, String> list)
        {
            InitializeComponent();
            displayGraph(list);
        }

        public RdWr()
        {

        }


        //Отображение нескольких графиков
        public void displayGraph(Dictionary<PointPairList, String> listOfLists)
        {
            ZedGraph.MasterPane masterPane = zedGraph.MasterPane;
            masterPane.PaneList.Clear();

            foreach (KeyValuePair<PointPairList, String> points in listOfLists)
            {
                GraphPane pane = createPane(points.Key, points.Value);
                masterPane.Add(pane);
            }

            using (Graphics g = CreateGraphics())
            {
                masterPane.SetLayout(g, PaneLayout.ForceSquare);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }


        private GraphPane createPane(PointPairList list, String name)
        {
            GraphPane pane = new GraphPane();
            pane.AddCurve(name, list, Color.Blue, SymbolType.None);
            return pane;
        }

        //Чтение файла с N значениями в одномерный массив PointPairList
        public PointPairList readFile(String name, int N)
        {
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList list = new PointPairList();
            FileStream fs = new FileStream(@name, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            for (int i = 0; i < N; i++)
            {
                float x = br.ReadSingle();
                list.Add(i, x);
            }
            fs.Close();
            return list;
        }
        //Чтение файла с размерами h*w в двумерный массив PointPairList типа float
        public PointPairList[] readFileArray(String name, int h, int w)
        {
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList[] list = new PointPairList[h];
            for (int i = 0; i < h; i++)
            {
                list[i] = new PointPairList();
            }
            FileStream fs = new FileStream(@name, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    float x = br.ReadSingle();
                    list[i].Add(j, x);
                }
            }
            fs.Close();
            return list;
        }
        //Чтение файла с размерами h*w в двумерный массив PointPairList типа short
        public PointPairList[] readFileArray2(String name, int h, int w)
        {
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList[] list = new PointPairList[h];
            for (int i = 0; i < h; i++)
            {
                list[i] = new PointPairList();
            }
            FileStream fs = new FileStream(@name, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    short x = br.ReadInt16();
                    list[i].Add(j, x);
                }
            }
            fs.Close();
            return list;
        }
        //Нормализация массива 
        public PointPairList norm(PointPairList list, double s)
        {
            double xmax = list.Max(point => point.Y), xmin = list.Min(point => point.Y);
            int N = list.Count();
            PointPairList listNew = new PointPairList();
            for (int i = 0; i < N; i++)
            {
                listNew.Add(i, (((list.ElementAt(i).Y - xmin) / (xmax - xmin)) - 0.5) * 2 * s);
            }
            return listNew;
        }

    }


}
