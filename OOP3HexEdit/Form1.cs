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
            var data = new Byte[40];
            for (Byte i = 0; i < 40; i++)
            {
                data[i] = i;
            }

            hexEdit1.Data = data;
        }
    }
}
