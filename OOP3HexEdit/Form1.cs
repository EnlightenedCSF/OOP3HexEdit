using System;
using System.Windows.Forms;

namespace OOP3HexEdit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var data = new Byte[100];
            for (Byte i = 0; i < 100; i++)
            {
                data[i] = i;
            }

            hexEdit1.Data = data;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
