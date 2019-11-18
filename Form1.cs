using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataMatrixForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += EncodeClick;
        }

        private void EncodeClick(object sender, EventArgs e)
        {
            var image = DataMatrix.Encode(textBox1.Text);
            pictureBox1.Image = image;
        }
    }
}
