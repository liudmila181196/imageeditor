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
    public partial class RdWrImage : Form
    {
        public RdWrImage()
        {
            InitializeComponent();
        }
        public RdWrImage(string name)
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(name);
        }

        public RdWrImage(Bitmap bitmap)
        {
            InitializeComponent();
            pictureBox1.Image = bitmap;

        }

    }
}
