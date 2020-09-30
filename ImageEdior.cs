using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

using ZedGraph;
using System.IO;
using System.Media;
using System.Threading;

namespace Nazarova
{
    public partial class ImageEdior : Form
    {
        private ZedGraph.ZedGraphControl zedGraph;
        string filename = "";
        Model model = new Model();
        RdWr rdwr = new RdWr();
        Analysis analysis = new Analysis();
        Processing proc = new Processing();
        
        public ImageEdior()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            openFileDialog1.Filter = "Image files Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            saveFileDialog1.Filter = "Image files Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            
            
            List<Method> filters = new List<Method>
            {
                new Method{ Id = 0, Name = "Выберите преобразование" },
                new Method{ Id = 1, Name = "Негатив" },
                new Method{ Id = 2, Name = "Гамма-коррекция" },
                new Method{ Id = 3, Name = "Логарифмическое преобразование" },
                new Method{ Id = 4, Name = "Эквалайзер" }                
            };
            comboBox1.DataSource = filters;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            List<Method> resizers = new List<Method>
            {
                new Method{ Id = 0, Name = "Выберите метод" },
                new Method{ Id = 1, Name = "Билинейная интерполяция" },
                new Method{ Id = 2, Name = "Метод ближайшего соседа" }
            };
            comboBox2.DataSource = resizers;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Id";
            comboBox2.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            filename = openFileDialog.FileName;
            pictureBox1.Image = new Bitmap(filename);
            comboBox1.Visible = true;
            button5.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl2.SelectTab(tabPage4);
            Image image= new Bitmap(filename);
            Bitmap newIm = new Bitmap(image);
            if (comboBox1.SelectedIndex == 1) newIm = proc.negative(image);
            else if (comboBox1.SelectedIndex == 4) newIm = proc.corrCDF(image);
            pictureBox2.Image = newIm;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Method filter = (Method)comboBox1.SelectedItem;
            switch (filter.Id)
            {
                case 0:
                    break;
                case 1:
                    button2.Visible = true;
                    trackBar1.Visible = false;
                    label1.Visible = false;
                    button3.Visible = false;
                    label3.Visible = false;
                    break;
                case 2:
                    button2.Visible = false;
                    trackBar1.Visible = true;
                    label1.Visible = true;
                    label3.Visible = true;
                    break;
                case 3:
                    button2.Visible = false;
                    trackBar1.Visible = true;
                    label1.Visible = true;
                    label3.Visible = true;
                    break;
                case 4:
                    button2.Visible = true;
                    trackBar1.Visible = false;
                    label1.Visible = false;
                    button3.Visible = false;
                    label3.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2) label1.Text = String.Format("Текущее значение: {0}", (double)trackBar1.Value/100);
            else if (comboBox1.SelectedIndex == 3) label1.Text = String.Format("Текущее значение: {0}", (double)trackBar1.Value/2);
            button3.Visible = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl2.SelectTab(tabPage4);
            Image image = new Bitmap(filename);
            Bitmap newIm = new Bitmap(image);
            if (comboBox1.SelectedIndex==2) newIm = proc.gamma(image, 1, (double)trackBar1.Value/100);
            else if (comboBox1.SelectedIndex == 3) newIm = proc.logarifm (image, trackBar1.Value/2);
            pictureBox2.Image = newIm;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            
            // сохраняем текст в файл
            pictureBox2.Image.Save(saveFileDialog1.FileName);


        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl2.SelectTab(tabPage5);
            Image image = new Bitmap(filename);
            // Получим панель для рисования
            GraphPane pane = zedGraph.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list = analysis.imgHistogram(image);
            LineItem myCurve = pane.AddCurve("Изображение", list, Color.Blue, SymbolType.None);
            pane.XAxis.Title.Text = "Тональный диапазон";
            pane.YAxis.Title.Text = "Частота пикселей";
            pane.Title.Text = "Гистограмма изображения";
            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            // В противном случае на рисунке будет показана только часть графика,
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph.AxisChange();

            // Обновляем график
            zedGraph.Invalidate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl2.SelectTab(tabPage4);
            Image image = new Bitmap(filename);
            Bitmap newIm = new Bitmap(image);
            double k;
            k = (double)numericUpDown1.Value;

            if (radioButton1.Checked) {
                if (comboBox2.SelectedIndex == 1) newIm = proc.bilinearInterp(image, k, k);
                else if (comboBox2.SelectedIndex == 2) newIm = proc.nearestNeighbors(image, k, k);
            }
            else if (radioButton2.Checked)
            {
                if (comboBox2.SelectedIndex == 1) newIm = proc.bilinearInterp(image, 1.0 / k, 1.0 / k);
                else if (comboBox2.SelectedIndex == 2) newIm = proc.nearestNeighbors(image, 1.0 / k, 1.0 / k);
            }
            pictureBox2.Image = newIm;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    class Method {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
