using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OOP3HexEdit
{
    public partial class HexEdit : Panel
    {
        public double CHAR_WIDTH = 15.22;
        public double CHAR_HEIGHT = 20.12;
        public int COLUMN_COUNT;
        public int ROW_COUNT;



        public byte[] Data
        {
            get { return _data; }
            set
            {
                ConvertData(value);
                _data = value;
            }
        }

        private byte[] _data;
        private string[] _hexData;

        private TextBox _editBox;
        private Label _content;

        public HexEdit()
        {
            InitializeComponent();
        }

        public HexEdit(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            _editBox = new TextBox
            {
                Visible = false,
                Width = 23,
                Height = 19,
                Font = new Font("Courier New", 11F, FontStyle.Regular, GraphicsUnit.Point),
                AutoSize = false
            };

            _content = new Label
            {
                AutoSize = false,
                Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point),
                Width = Width-2,
                Height = Height-2,
                Left = Left+1,
                Top = Top+1,
                BackColor = Color.Beige
            };
            _content.MouseDown += ContentOnMouseDown;

            BackColor = Color.Blue;

            _editBox.Parent = this;
            _content.Parent = this;
        }

        private void ContentOnMouseDown(object sender, MouseEventArgs e)
        {
            var colNum = (int) Math.Round((e.X-3)/10.4/3);
            var rowNum = (int) ((e.Y - 3)/16.0);
            _editBox.Left = 1 + colNum*10*3;
            _editBox.Top = 1 + rowNum*19;
            _editBox.Text = _hexData[rowNum * COLUMN_COUNT + colNum];
            _editBox.Visible = true;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (_content == null) return;
            _content.Width = Width - 2;
            _content.Height = Height - 2;
        }


        protected void ConvertData(byte[] data)
        {
            if (data == null) 
                return;
            
            _hexData = new string[data.GetLength(0)];
            var i = 0;
            foreach (var b in data)
            {
                _hexData[i++] = Convert.ToByte(b).ToString("x2");
            }

            _content.Text = data.Select(b => Convert.ToByte(b).ToString("x2") + " ").Aggregate("", (current, temp) => current + temp);

            COLUMN_COUNT = (int)Math.Round(Width/10.4/3);
            ROW_COUNT = (int)Math.Ceiling((double)_hexData.GetLength(0)/COLUMN_COUNT);
        }
        
        
    }
}
