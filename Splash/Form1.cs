using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void StartGame()
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CameraControl cm = new CameraControl();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void On_Paper(object sender, EventArgs e)
        {

        }
    }
}
