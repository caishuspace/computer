using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class index : Form
    {
        public index()
        {
            this.MaximizeBox=false;
            this.MinimizeBox=false;
            InitializeComponent();
        }

        private void index_Load(object sender, EventArgs e)
        {

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form form1 = new Form1();
            form1.ShowDialog(); 
            this.Show();
        }
    }
}
