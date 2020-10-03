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
            //Загружаем и сохраняем только изображения
            openFileDialog1.Filter = "Image files Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            saveFileDialog1.Filter = "Image files Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            
            //Список фильтров
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

            //Список методов изменения размера
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

        //Кнопка "Выберите изображение"
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            filename = openFileDialog.FileName;
            pictureBox1.Image = new Bitmap(filename);
            comboBox1.Visible = true;
            button5.Visible = true;
        }
        //Кнопка "Применить" для негатива и эквалайзера
        private void button2_Click(object sender, EventArgs e)
        {
            if (filename == "") MessageBox.Show("Вы не выбрали изображение!");
            else {
                tabControl2.SelectTab(tabPage4);
                Image image= new Bitmap(filename);
                Bitmap newIm = new Bitmap(image);
                if (comboBox1.SelectedIndex == 1) newIm = proc.negative(image);
                else if (comboBox1.SelectedIndex == 4) newIm = proc.corrCDF(image);
                pictureBox2.Image = newIm; 
            }
            
        }
        //Выпадающий список выбора фильтра
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Method filter = (Method)comboBox1.SelectedItem;
            switch (filter.Id)
            {
                case 0:
                    break;
                case 1://негатив
                    button2.Visible = true;
                    trackBar1.Visible = false;
                    label1.Visible = false;
                    button3.Visible = false;
                    label3.Visible = false;
                    break;
                case 2://гамма
                    button2.Visible = false;
                    trackBar1.Visible = true;
                    label1.Visible = true;
                    label3.Visible = true;
                    button3.Visible = true;
                    break;
                case 3://логарифм
                    button2.Visible = false;
                    trackBar1.Visible = true;
                    label1.Visible = true;
                    label3.Visible = true;
                    button3.Visible = true;
                    break;
                case 4://эквалайзер
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
        //Выбор коэффициента для гаммы и логарифма
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2) label1.Text = String.Format("Текущее значение: {0}", (double)trackBar1.Value/100);
            else if (comboBox1.SelectedIndex == 3) label1.Text = String.Format("Текущее значение: {0}", (double)trackBar1.Value/2);
        }
        //Кнопка "Применить" для гаммы и логарифма
        private void button3_Click(object sender, EventArgs e)
        {
            if (filename == "") MessageBox.Show("Вы не выбрали изображение!");
            else {
                tabControl2.SelectTab(tabPage4);
                Image image = new Bitmap(filename);
                Bitmap newIm = new Bitmap(image);
                if (comboBox1.SelectedIndex == 2) newIm = proc.gamma(image, 1, (double)trackBar1.Value / 100);
                else if (comboBox1.SelectedIndex == 3) newIm = proc.logarifm(image, trackBar1.Value / 2);
                pictureBox2.Image = newIm;
            }
            
        }
        //Кнопка "Сохранить"
        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image==null) MessageBox.Show("Исходное изображение не изменено!");
            else {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                pictureBox2.Image.Save(saveFileDialog1.FileName);
            }
        }
        //кнопка "Построить гистограмму"
        private void button5_Click(object sender, EventArgs e)
        {
            if (filename == "") MessageBox.Show("Вы не выбрали изображение!");
            else {
                tabControl2.SelectTab(tabPage5);
                Image image = new Bitmap(filename);
                // Получим панель для рисования
                GraphPane pane = zedGraph.GraphPane;

                // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
                pane.CurveList.Clear();

                // Создадим список точек
                /*PointPairList list = analysis.imgHistogram(image, 0);
                LineItem myCurve = pane.AddCurve("Изображение", list, Color.Red, SymbolType.None);*/
                PointPairList list_R = analysis.imgHistogram(image, 0);
                PointPairList list_G = analysis.imgHistogram(image, 1);
                PointPairList list_B = analysis.imgHistogram(image, 2);
                LineItem myCurve_R = pane.AddCurve("R", list_R, Color.Red, SymbolType.None);
                LineItem myCurve_G = pane.AddCurve("G", list_G, Color.Green, SymbolType.None);
                LineItem myCurve_B = pane.AddCurve("B", list_B, Color.Blue, SymbolType.None);
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
            
        }
        //Кнопка "Применить" для методов изменения размера
        private void button6_Click(object sender, EventArgs e)
        {
            if (filename == "") MessageBox.Show("Вы не выбрали изображение!");
            else
            {
                tabControl2.SelectTab(tabPage4);
                Image image = new Bitmap(filename);
                Bitmap newIm = new Bitmap(image);
                double k;
                k = (double)numericUpDown1.Value;

                if (radioButton1.Checked)
                {
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
        }
        //Подавление шумов
        private void button7_Click(object sender, EventArgs e)
        {
            if (filename == "") MessageBox.Show("Вы не выбрали изображение!");
            else
            {
                tabControl2.SelectTab(tabPage4);
                Image image = new Bitmap(filename);
                Bitmap newIm = new Bitmap(image);
                int k = (int)numericUpDown3.Value;
                if (radioButton3.Checked)
                {
                    newIm = proc.MMFilter(image, k, 1);
                }
                else if (radioButton4.Checked)
                {
                    newIm = proc.MMFilter(image, k, 0);
                }
                pictureBox2.Image = newIm;
            }
        }
        //Пороговое преобразование
        private void button8_Click(object sender, EventArgs e)
        {
            if (filename == "") MessageBox.Show("Вы не выбрали изображение!");
            else
            {
                tabControl2.SelectTab(tabPage4);
                Image image = new Bitmap(filename);
                Bitmap newIm = new Bitmap(image);
                int k = (int)numericUpDown2.Value;
                newIm = proc.globalThresholdConversion(image, k);
                pictureBox2.Image = newIm;
            }
        }
        //Выделение контуров
        private void button9_Click(object sender, EventArgs e)
        {
            if (filename == "") MessageBox.Show("Вы не выбрали изображение!");
            else
            {
                tabControl2.SelectTab(tabPage4);
                Image image = new Bitmap(filename);
                Bitmap newIm = new Bitmap(image);
                int k = (int)numericUpDown4.Value;
                double t= (double)trackBar2.Value;
                if (radioButton5.Checked)
                {
                    newIm = proc.gradient(image, k);
                }
                else if (radioButton6.Checked)
                {
                    newIm = proc.laplasian(image, k, t/100);
                }
                pictureBox2.Image = newIm;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            label8.Visible = true;
            label9.Visible = true;
            trackBar2.Visible = true;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label9.Text = String.Format("Текущее значение: {0}%", trackBar2.Value);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            label8.Visible = false;
            label9.Visible = false;
            trackBar2.Visible = false;
        }
    }
    class Method {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
