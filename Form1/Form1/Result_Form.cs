using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // for GraphicsPath
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form1
{
    public partial class Result_Form : Form
    {
        // variable to track button pressed state
        private bool btnPressed = false;

        public Result_Form(int score, string timeTaken)
        {
            InitializeComponent();

            lblScore.Text = score.ToString();
            lblTime.Text = "Time: " + timeTaken;

            // ===== Exit Button Design =====
            btnExit.BackColor = Color.Transparent;
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnExit.Width = 120;
            btnExit.Height = 45;

            btnExit.Paint += CustomButton_Paint;

            btnExit.MouseEnter += (s, e) => btnExit.BackColor = Color.SteelBlue;
            btnExit.MouseLeave += (s, e) => btnExit.BackColor = Color.DodgerBlue;

            btnExit.MouseDown += (s, e) => { btnPressed = true; btnExit.Invalidate(); };
            btnExit.MouseUp += (s, e) => { btnPressed = false; btnExit.Invalidate(); };

            // ===== Back Button Design =====
            btnBack.BackColor = Color.Transparent;
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnBack.Width = 120;
            btnBack.Height = 45;

            btnBack.Paint += CustomButton_Paint;

            btnBack.MouseEnter += (s, e) => btnBack.BackColor = Color.SteelBlue;
            btnBack.MouseLeave += (s, e) => btnBack.BackColor = Color.DodgerBlue;

            btnBack.MouseDown += (s, e) => { btnPressed = true; btnBack.Invalidate(); };
            btnBack.MouseUp += (s, e) => { btnPressed = false; btnBack.Invalidate(); };
        }

        private void Result_Form_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // go back to first form
            Form1 f1 = new Form1();
            f1.Show();
            this.Close();
        }

        // function to make rounded rectangle
        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        // custom paint for rounded gradient button
        private void CustomButton_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = btn.ClientRectangle;

            Color startColor = btnPressed ? Color.FromArgb(53, 122, 189) : Color.FromArgb(74, 144, 226);
            Color endColor = btnPressed ? Color.FromArgb(33, 102, 156) : Color.FromArgb(53, 122, 189);

            using (GraphicsPath path = GetRoundedPath(rect, 20))
            using (LinearGradientBrush brush =
                   new LinearGradientBrush(rect, startColor, endColor, LinearGradientMode.Horizontal))
            {
                btn.Region = new Region(path);
                e.Graphics.FillPath(brush, path);
            }

            // move text when pressed
            Point textLocation = new Point(rect.X, rect.Y);
            if (btnPressed)
                textLocation.Offset(1, 1);

            TextRenderer.DrawText(
                e.Graphics,
                btn.Text,
                btn.Font,
                new Rectangle(textLocation, rect.Size),
                Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }
    }
}
