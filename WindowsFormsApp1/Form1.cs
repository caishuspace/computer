using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Press_Click(object sender, EventArgs e)
        {
            //土体参数数量 Column1~7 分别代表 土层数目、土层、厚度、密度、粘聚力、内摩擦角、剪切模量
            int Tuti_num = dataGridView1.RowCount-1;
            textBox1.Text = Tuti_num.ToString();
            //循环获取土体参数
            //double[] Column1 = new double[Tuti_num+1]; //土层数目
            //double[] Column2 = new double[Tuti_num+1]; //土层
            double[] Column3 = new double[Tuti_num+1];  //厚度 h
            double[] Column4 = new double[Tuti_num+1];  //密度 p
            //double[] Column5 = new double[Tuti_num+1];  //粘聚力
            //double[] Column6 = new double[Tuti_num+1];  //内摩擦角
            //double[] Column7 = new double[Tuti_num+1];  //剪切模量
            double C = Convert.ToDouble(dataGridView1.Rows[Tuti_num - 1].Cells[4].Value.ToString()); //粘聚力
            double fai = Convert.ToDouble(dataGridView1.Rows[Tuti_num-1].Cells[5].Value.ToString()); //内摩擦角
            double G  = Convert.ToDouble(dataGridView1.Rows[Tuti_num-1].Cells[6].Value.ToString());  //剪切模量

            for (int i = 0; i < Tuti_num; i++)
            {
                try
                {
                    //Column1[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    //Column2[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    Column3[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    Column4[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    //  Column5[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    //   Column6[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    //    Column7[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value.ToString());
                }
                catch (Exception)
                {
                    MessageBox.Show("土体参数输入格式有误");
                }

            }

            //工程参数
            string hw = textBox3.Text;//河流表面到钻孔位置高差
            string H = textBox4.Text;//钻孔深度
            string f = comboBox1.Text;//安全系数：钻孔位置所在图层（粘性土=2，非粘性土=1.5）
            string R0 = textBox6.Text;//钻孔（扩孔）半径
            string pm = textBox7.Text;//泥浆密度
            string L = textBox8.Text;//管（孔）道总长度
            string up = textBox9.Text;//泥浆塑性粘度
            string v = textBox10.Text;//泥浆平均流速
            string Db = textBox11.Text;//钻（扩）孔直径
            string Dop = textBox12.Text;//钻杆（管道）外径
            string ty = textBox13.Text;//泥浆屈服值
            double g = 9.8;//g取9.8

            //工程化参数数字化
                double n_hw = 0;
                double n_H = 0;
                double n_f = 0;
                double n_R = 0;
                double n_pm = 0;
                double n_L = 0;
                double n_up = 0;
                double n_v = 0;  
                double n_Db = 0;
                double n_Dop = 0;
                double n_ty = 0;
            try
            {
                n_hw = Convert.ToDouble(hw);
                n_H = Convert.ToDouble(H);
                n_f = Convert.ToDouble(f);
                n_R = Convert.ToDouble(R0);
                n_pm = Convert.ToDouble(pm);
                n_L = Convert.ToDouble(L);
                n_up = Convert.ToDouble(up);
                n_v = Convert.ToDouble(v);  
                n_Db = Convert.ToDouble(Db);
                n_Dop = Convert.ToDouble(Dop);
                n_ty = Convert.ToDouble(ty);
            }
            catch (Exception)
            {
                MessageBox.Show("工程参数输入格式有误");
            }


            // 计算空隙水压力 u孔隙水压力pa、 rh水的单位重量1*104N/m3
            double u = 10000 * n_hw;
            //MessageBox.Show("u"+u.ToString());

            // 计算s0 初始有效压力
            double s0 = 0;
            for (int i = 0; i < Tuti_num; i++)
            {
                s0 +=  Column3[i] * Column4[i] * g;
            }
            //MessageBox.Show("s0"+s0.ToString());
            //计算Rpmax最大塑性区半径
            double Rpmax = (n_H - n_R) / n_f;
            //MessageBox.Show("Rpmax" + Rpmax.ToString());

            //计算最大压力
           // MessageBox.Show("fai" + fai.ToString());
            double P_max;
            double sinf = Math.Sin(Math.PI / 180 * fai); //MessageBox.Show("sinf"+sinf.ToString());
            double cosf = Math.Cos(Math.PI / 180 * fai); //MessageBox.Show("cosf"+cosf.ToString());
            double cotf = 1/Math.Tan(Math.PI / 180 * fai); //MessageBox.Show("cotf"+cotf.ToString());
            double di = (n_R * n_R) / (Rpmax * Rpmax) + (s0 * sinf + C * cosf) / G; //MessageBox.Show("di"+di.ToString());
            double mi = -sinf / (1 + sinf); //MessageBox.Show("mi"+mi.ToString());

            P_max = u + (s0 * (1+sinf)+C*cosf+C*cotf)*Math.Pow(di,mi)-C*cotf;
            textBox2.Text = P_max.ToString();


            //计算最小压力
            double pmin = n_pm * g * n_H + n_L * ((n_up * n_v) / ((n_Db - n_Dop) * (n_Db - n_Dop)) + n_ty / (n_Db - n_Dop));

            textBox1.Text = Convert.ToString(pmin);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            double pmax = Convert.ToDouble(textBox2.Text);
            double pmin = Convert.ToDouble(textBox1.Text);
            pmax = pmax/1000000.0;
            pmin = pmax/1000000.0;
            if (pmax >= 0.2 && pmax < 0.6)
            {
                pictureBox1.Image = Resources._0_2_0_6;
            }
            else if (pmax >= 0.6 && pmax < 0.8)
            {
                pictureBox1.Image = Resources._0_6_0_8;
            }
            else if (pmax < 1.0)
            {
                pictureBox1.Image = Resources._0_8_1;
            }
            else if (pmax < 1.2)
            {
                pictureBox1.Image = Resources._1_1_2;
            }
            else if (pmax < 1.4)
            {
                pictureBox1.Image = Resources._1_2_1_4;
            }
            else if (pmax < 1.6)
            {
                pictureBox1.Image = Resources._1_4_1_6;
            }
            else if (pmax < 1.8)
            {
                pictureBox1.Image = Resources._1_6_1_8;
            }
            else if (pmax < 2.0)
            {
                pictureBox1.Image = Resources._1_8_2_0;
            }
            else if (pmax < 2.2)
            {
                pictureBox1.Image = Resources._2_2_2;
            }
            else if (pmax < 2.4)
            {
                pictureBox1.Image = Resources._2_2_2_4;
            }
            else if (pmax < 2.6)
            {
                pictureBox1.Image = Resources._2_4_2_6;
            }
            else if (pmax < 2.8)
            {
                pictureBox1.Image = Resources._2_6_2_8;
            }
            else
            {
                MessageBox.Show("请确认参数是否正确");
            }

            if (pmin >= 0.2 & pmin < 0.6)
            {
                pictureBox2.Image = Resources._0_2_0_6;
            }
            else if (pmin < 0.8)
            {
                pictureBox2.Image = Resources._0_6_0_8;
            }
            else if (pmin < 1.0)
            {
                pictureBox2.Image = Resources._0_8_1;
            }
            else if (pmin < 1.2)
            {
                pictureBox2.Image = Resources._1_1_2;
            }
            else if (pmin < 1.4)
            {
                pictureBox2.Image = Resources._1_2_1_4;
            }
            else if (pmin < 1.6)
            {
                pictureBox2.Image = Resources._1_4_1_6;
            }
            else if (pmin < 1.8)
            {
                pictureBox2.Image = Resources._1_6_1_8;
            }
            else if (pmin < 2.0)
            {
                pictureBox2.Image = Resources._1_8_2_0;
            }
            else if (pmin < 2.2)
            {
                pictureBox2.Image = Resources._2_2_2;
            }
            else if (pmin < 2.4)
            {
                pictureBox2.Image = Resources._2_2_2_4;
            }
            else if (pmin < 2.6)
            {
                pictureBox2.Image = Resources._2_4_2_6;
            }
            else if (pmin < 2.8)
            {
                pictureBox2.Image = Resources._2_6_2_8;
            }
            else
            {
                MessageBox.Show("请确认参数是否正确");
            }
            }
            catch(Exception)
            {
                MessageBox.Show("参数输入有问题，请确认输入");
            }
 

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }
    }
}
