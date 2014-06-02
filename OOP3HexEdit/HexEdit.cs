using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OOP3HexEdit
{
    public partial class HexEdit : Panel
    {
        public double CHAR_WIDTH = 15.22;
        public double CHAR_HEIGHT = 20.12;
        
        public int COLUMN_COUNT;
        public int ROW_COUNT;
        public int VISIBLE_ROW_COUNT;

        public int LINES_SCROLLED = 0;

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
        private Label _dummy;

        private int _rowNum;
        private int _colNum;
        private string _oldChunk;

        public HexEdit()
        {
            
            InitializeComponent();
            AdjustFormScrollbars(true);
        }

        public HexEdit(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            VerticalScroll.Visible = true;
            HorizontalScroll.Visible = false;
            HorizontalScroll.Enabled = false;
            AutoScroll = true;
            AutoSize = false;
            
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

            _dummy = new Label
            {
                Width = 1,
                Height = 1,
                Visible = false,
                Left = 2,
                Top = 2
            };

            _content.MouseDown += ContentOnMouseDown;
            _editBox.KeyPress += EditBoxOnKeyPress;
            Scroll += OnScroll;

            _editBox.Parent = this;
            _content.Parent = this;
            _dummy.Parent = this;
        }

        private void OnScroll(object sender, ScrollEventArgs e)
        {
            LINES_SCROLLED = (e.NewValue - e.OldValue) > 0 ? LINES_SCROLLED + 1 : LINES_SCROLLED - 1;

        }

        private void EditBoxOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) 13) return;

            var match = Regex.Match(_editBox.Text, @"[\dA-Fa-f]{2}");
            if (match.Success)
            {
                _hexData[_rowNum*COLUMN_COUNT + _colNum] = _editBox.Text;
                _data[_rowNum*COLUMN_COUNT + _colNum] = Convert.ToByte(_editBox.Text);

                WriteToContent();
            }
            
            _editBox.Clear();
            _editBox.Hide();
        }


        private void ContentOnMouseDown(object sender, MouseEventArgs e)
        {
            _colNum = (int) Math.Round((e.X-3)/10.4/3);
            _rowNum = (int) ((e.Y - LINES_SCROLLED * 18.2)/18.2);

            if (_colNum == COLUMN_COUNT || _rowNum == VISIBLE_ROW_COUNT)
                return;

            _editBox.Left = 1 + _colNum*10*3;
            _editBox.Top = (int)(_rowNum*18.2);

            var index = (_rowNum + LINES_SCROLLED)*COLUMN_COUNT + _colNum;
            if (index > _hexData.GetLength(0))
            {
                if (!_editBox.Visible) return;
                _editBox.Clear();
                _editBox.Hide();
            }
            else
            {
                _oldChunk = _hexData[index];
                _editBox.Text = _oldChunk;
                _editBox.Visible = true;   
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }


        protected void WriteToContent()
        {
            _content.Text = "";
            var i = 0;
            _content.Text = _hexData.Aggregate("", (current, s) => current + (_hexData[i++] + " "));
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

            COLUMN_COUNT = (int)Math.Round(Width / 10.4 / 3);
            ROW_COUNT = (int)Math.Ceiling((double)_hexData.GetLength(0) / COLUMN_COUNT);
            VISIBLE_ROW_COUNT = (int) (Height/18.2);

            WriteToContent();

            var qwe = VerticalScroll.Maximum - 12;
            VerticalScroll.SmallChange = qwe / (ROW_COUNT - VISIBLE_ROW_COUNT) - 3;
            VerticalScroll.LargeChange = qwe / (ROW_COUNT - VISIBLE_ROW_COUNT) - 3;

            if (ROW_COUNT > Height/18.2)
            {
                Expand();
            }

            _content.Width = Width - 10;
            _content.Height = (int) (ROW_COUNT*18.2);
        }

        private void Expand()
        {
            _dummy.Top = (int)(ROW_COUNT*18.2) + SystemInformation.HorizontalScrollBarHeight;
            _dummy.Visible = true;
        }
    }
}
