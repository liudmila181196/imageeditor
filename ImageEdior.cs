using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nazarova
{
    public partial class ImageEdior : Form
    {
        string filename = "";
        Model model = new Model();
        RdWr rdwr = new RdWr();
        Analysis analysis = new Analysis();
        Processing proc = new Processing();
        public ImageEdior()
        {
            InitializeComponent();
            button1.Click += button1_Click;

            List<Filter> filters = new List<Filter>
            {
                new Filter{ Id = 0, Name = "Выберите преобразование" },
                new Filter{ Id = 1, Name = "Негатив" },
                new Filter{ Id = 2, Name = "Гамма-коррекция" },
                new Filter{ Id = 3, Name = "Логарифмическое преобразование" },
                new Filter{ Id = 4, Name = "Эквалайзер" }                
            };

            comboBox1.DataSource = filters;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image image= new Bitmap(filename);
            Bitmap newIm = new Bitmap(image);
            if (comboBox1.SelectedIndex == 1) newIm = proc.negative(image);
            else if (comboBox1.SelectedIndex == 4) newIm = proc.corrCDF(image);
            pictureBox2.Image = newIm;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter filter = (Filter)comboBox1.SelectedItem;
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
            Image image = new Bitmap(filename);
            Bitmap newIm = new Bitmap(image);
            if (comboBox1.SelectedIndex==2) newIm = proc.gamma(image, 1, (double)trackBar1.Value/100);
            else if (comboBox1.SelectedIndex == 3) newIm = proc.logarifm (image, trackBar1.Value/2);
            pictureBox2.Image = newIm;
            
        }

    }
    class Filter {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
