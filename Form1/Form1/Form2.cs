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

namespace Form1
{
    public partial class Form2 : Form
    {
        // variable to track button pressed state
        private bool btnPressed = false;
        string selectedChapter;

        public Form2(string chapter)
        {
            InitializeComponent();
            selectedChapter = chapter;
            lblSelectedChapter.Text =  selectedChapter;

            // Button basic design
            btnStartQuiz.BackColor = Color.Transparent;
            btnStartQuiz.ForeColor = Color.White;
            btnStartQuiz.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnStartQuiz.Width = 130;
            btnStartQuiz.Height = 50;

            // Custom paint event for rounded + gradient button
            btnStartQuiz.Paint += btnNext_Paint;

            // Mouse enter / leave effect (hover)
            btnStartQuiz.MouseEnter += (s, e) => btnStartQuiz.BackColor = Color.SteelBlue;
            btnStartQuiz.MouseLeave += (s, e) => btnStartQuiz.BackColor = Color.DodgerBlue;

            // Mouse down / up to show pressed effect
            btnStartQuiz.MouseDown += (s, e) => { btnPressed = true; btnStartQuiz.Invalidate(); };
            btnStartQuiz.MouseUp += (s, e) => { btnPressed = false; btnStartQuiz.Invalidate(); };
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
       
            Quiz_Form quizForm = new Quiz_Form(selectedChapter);
            quizForm.Show();
            this.Hide();
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Draw 4 rounded corners
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void btnNext_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = btn.ClientRectangle;

            // Change color if button is pressed
            Color startColor = btnPressed ? Color.FromArgb(53, 122, 189) : Color.FromArgb(74, 144, 226);
            Color endColor = btnPressed ? Color.FromArgb(33, 102, 156) : Color.FromArgb(53, 122, 189);

            using (GraphicsPath path = GetRoundedPath(rect, 22))
            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                startColor,
                endColor,
                LinearGradientMode.Horizontal))
            {
                // Set rounded shape
                btn.Region = new Region(path);

                // Fill with gradient
                e.Graphics.FillPath(brush, path);
            }

            // Move text a bit if pressed for 3D effect
            Point textLocation = new Point(rect.X, rect.Y);
            if (btnPressed)
                textLocation.Offset(1, 1);

            // Draw text in center
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
