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
        public RdWr(Dictionary<PointPairList, String> list, double b)
        {
            InitializeComponent();
            displayGraph(list, b);
        }

        public RdWr(Dictionary<PointPairList, String> list)
        {
            InitializeComponent();
            displayGraph(list);
        }

        public RdWr()
        {
            
        }

        public void displayGraph(Dictionary<PointPairList, String> listOfLists, double b)
        {
            ZedGraph.MasterPane masterPane = zedGraph.MasterPane;
            masterPane.PaneList.Clear();
            Analysis analysis = new Analysis();

            foreach (KeyValuePair<PointPairList, String> points in listOfLists)
            {
                GraphPane pane = createPane(points.Key, points.Value);
                String text = analysis.deviations(points.Key, b);
                pane.Title.Text = text.Substring(0, text.IndexOf("\n"));
                text = text.Substring(text.IndexOf("\n"), text.Length-text.IndexOf("\n"));
                pane.XAxis.Title.Text = text;
                masterPane.Add(pane);
            }

            using (Graphics g = CreateGraphics())
            {
                masterPane.SetLayout(g, PaneLayout.ForceSquare);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }



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

        public PointPairList norm(PointPairList list, double s)
        {
            double xmax = list.Max(point => point.Y), xmin= list.Min(point => point.Y);
            int N = list.Count();
            PointPairList listNew = new PointPairList();
            for (int i = 0; i < N; i++)
            {
                listNew.Add(i, (((list.ElementAt(i).Y - xmin) / (xmax - xmin)) - 0.5) * 2 * s);
            }
            return listNew;
        }

        private GraphPane createPane(PointPairList list, String name)
        {
            GraphPane pane = new GraphPane();
            pane.AddCurve(name, list, Color.Blue, SymbolType.None);
            return pane;
        }

        public PointPairList readFile(String name)
        {
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList list = new PointPairList();
            FileStream fs = new FileStream(@name, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            for (int i = 0; i < 1000; i++)
            {
                float x = br.ReadSingle();
                list.Add(i, x);
            }
            fs.Close();
            return list;
        }
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
                for (int j=0; j<w; j++)
                {
                    float x = br.ReadSingle();
                    list[i].Add(j, x);
                }
            }
            fs.Close();
            return list;
        }

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
    }

}
