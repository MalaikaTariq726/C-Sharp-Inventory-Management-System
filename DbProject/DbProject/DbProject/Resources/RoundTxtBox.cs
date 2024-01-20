using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbProject.Resources
{
    [DefaultEvent("_TextChanged")]
    public partial class RoundTxtBox : UserControl
    {
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 2;
        private bool underLinedStyle = false;
        private Color borderFocusColor = Color.DarkGray;
        private bool isFocused = false;
        private int borderRadius = 0;
        private Color placeolderColor = Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceHolder = false;
        private bool isPasswordChar = false;
       public RoundTxtBox()
        {
            InitializeComponent();
        }

        public event EventHandler _TextChanged;
        [Category("Round text Box")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; this.Invalidate(); } }
        [Category("Round text Box")]
        public int BorderSize { get { return borderSize; } set { borderSize = value; this.Invalidate(); } }
        [Category("Round text Box")]
        public bool UnderLinedStyle { get { return underLinedStyle; } set { underLinedStyle = value; this.Invalidate(); } }


        private  GraphicsPath GetFigurePath(Rectangle rect,int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            if(borderRadius>1)
            {
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize=borderSize>1?borderSize:1;

                using(GraphicsPath pathborderSmoother=GetFigurePath(rectBorderSmooth,borderRadius))
                using (GraphicsPath pathBorder=GetFigurePath(rectBorder,borderRadius-borderSize))
                using (Pen penBorderSmoother =new Pen(this.Parent.BackColor,smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(pathborderSmoother);
                    if(borderSize>15)
                    {
                        SetTextRoundedRegion();
                    }
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                    if (isFocused)
                    {
                        penBorder.Color = borderFocusColor;

                    }
                    if (underLinedStyle)
                    {
                        g.DrawPath(penBorderSmoother,pathborderSmoother);
                        g.SmoothingMode = SmoothingMode.None;

                        g.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);

                    }
                    else
                    {
                        g.DrawPath(penBorderSmoother, pathborderSmoother);
                        g.DrawPath(penBorder, pathBorder);
                    }
                }


            }
            else
            {
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    if (isFocused)
                    {
                        penBorder.Color = borderFocusColor;

                    }
                    if (underLinedStyle)
                    {
                        g.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);

                    }
                    else
                    {
                        g.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);

                    }
                }
            }
            
        }

        private void SetTextRoundedRegion()
        {
           GraphicsPath path = new GraphicsPath();
            if(Multiline)
            {
                path = GetFigurePath(textBox1.ClientRectangle, borderRadius - borderSize);
                textBox1.Region = new Region(path);


            }
            else
            {
                path = GetFigurePath(textBox1.ClientRectangle,  borderSize*2);
                textBox1.Region = new Region(path);
            }
        }

        [Category("Round text Box")]
        public bool PasswordChar
        {
            get{
                return isPasswordChar; }
            set{
                isPasswordChar = value;
                textBox1.UseSystemPasswordChar = value; }
        }
        [Category("Round text Box")]
        public bool Multiline
        {
            get { return textBox1.Multiline; }
            set { textBox1.Multiline = value; }
        }
        [Category("Round text Box")]
        public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; textBox1.BackColor = value; } }
        [Category("Round text Box")]
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; textBox1.ForeColor = value; } }
        [Category("Round text Box")]
        public override Font Font { get { return base.Font; } set { base.Font = value; textBox1.Font = value;
            if(this.DesignMode)
                {
                    UpdateControlHeight();

                }
            } }
        [Category("Round text Box")]

        public string Texts
        {
            get
            {
                if(isPlaceHolder)
                {
                    return "";
                }
              else  return textBox1.Text;
            }
            set {textBox1.Text = value;
                SetPlaceHolder();
            }
        }
        [Category("Round text Box")]
        public Color BorderFocusColor
        { get { return borderFocusColor; } set { borderFocusColor = value; } }
        [Category("Round text Box")]
        public int BorderRadius { get { return borderRadius; } set { if(value>=0)borderRadius = value; this.Invalidate(); } }
        [Category("Round text Box")]
        public Color PlaceolderColor
        {
            get { return placeolderColor; }
            set { placeolderColor = value;
                if (isPasswordChar)
                    textBox1.ForeColor = value;
            }
        }
        [Category("Round text Box")]
        public string PlaceholderText
        {
            get { return placeholderText; }
            set { placeholderText = value;
                textBox1.Text = "";
                SetPlaceHolder();
        }
        }
        

        private void SetPlaceHolder()
        {
           if(string.IsNullOrWhiteSpace(textBox1.Text)&& placeholderText!="")
            {
                isPlaceHolder = true;
                textBox1.TextAlign=HorizontalAlignment.Center;
                textBox1.Text = placeholderText;
                textBox1.ForeColor = placeolderColor;
                if(isPasswordChar)
                {
                    textBox1.UseSystemPasswordChar = false;
                }
            }
        }
        public void setPasswordChar(bool check)
        {
            isPasswordChar = check;

            textBox1.UseSystemPasswordChar = check;
        }
        public void SetTextAlignment(HorizontalAlignment alignment)
        {
            textBox1.TextAlign = alignment;
        }
        private void RemovePlaceholder()
        {
            if (isPlaceHolder && placeholderText != "")
            {
                isPlaceHolder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                {
                    textBox1.UseSystemPasswordChar = true;
                }
            }
        }
        [Category("Round text Box")]
        public bool IsPlaceHolder { get => isPlaceHolder; set => isPlaceHolder = value; }
        [Category("Round text Box")]
        public bool IsPasswordChar { get => isPasswordChar; set => isPasswordChar = value; }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if(this.DesignMode)
            UpdateControlHeight();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }

        private void UpdateControlHeight()
        {
            if(textBox1.Multiline==false)
            {
                int textHeight=TextRenderer.MeasureText("Text",this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, textHeight);
                textBox1.Multiline = false;
                this.Height=textBox1.Height+this.Padding.Top + this.Padding.Bottom;
                
            }
        }
     

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_TextChanged != null)
                _TextChanged.Invoke(sender, e);
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
            SetPlaceHolder();
        }

        private void RoundTxtBox_Load(object sender, EventArgs e)
        {

        }
    }
}
