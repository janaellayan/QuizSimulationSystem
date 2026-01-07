using Form1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static Form1.Models.QuizGenerator;

namespace Form1
{
    public partial class pre_quiz : Form
    {
        // variable to track button pressed state
        private bool btnPressed = false;
        private bool btnBackPressed = false;
        string selectedChapter;
        string studentName;
        string studentID;

        public pre_quiz(string chapter, string studentName, string studentID)
        {
            InitializeComponent();
            selectedChapter = chapter;
            this.studentName = studentName;
            this.studentID = studentID;
            lblSelectedChapter.Text = selectedChapter;

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

            // Go Back button styling
            btnBack.BackColor = Color.Transparent;
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnBack.Width = 130;
            btnBack.Height = 50;

            btnBack.Paint += btnBack_Paint;
            btnBack.MouseEnter += (s, e) => btnBack.BackColor = Color.SteelBlue;
            btnBack.MouseLeave += (s, e) => btnBack.BackColor = Color.DodgerBlue;
            btnBack.MouseDown += (s, e) => { btnBackPressed = true; btnBack.Invalidate(); };
            btnBack.MouseUp += (s, e) => { btnBackPressed = false; btnBack.Invalidate(); };
        }

     

        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            try
            {
                // loads 5 random questions and store in static state
                string chapterfile = selectedChapter.ToLower().Replace(" ", "");
                QuizState.SelectedQuestions = QuizGenerator.RandomizeQuestions(chapterfile);
                QuizState.StudentName = studentName;
                QuizState.StudentID = studentID;
                QuizState.StartTime = DateTime.Now;  // for timer

                Quiz_Form quizForm = new Quiz_Form(selectedChapter, studentName, studentID);
                quizForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading quiz: " + ex.Message);
            }
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

        // Custom drawing for Start Quiz button
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

        // Custom drawing for Go Back button
        private void btnBack_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = btn.ClientRectangle;

            // Change color if button is pressed
            Color startColor = btnBackPressed ? Color.FromArgb(53, 122, 189) : Color.FromArgb(74, 144, 226);
            Color endColor = btnBackPressed ? Color.FromArgb(33, 102, 156) : Color.FromArgb(53, 122, 189);

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
            if (btnBackPressed)
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            // go back to user info form
            User_Info user_Info = new User_Info();
            user_Info.Show();
            this.Hide();
        }

        private void pre_quiz_Load(object sender, EventArgs e)
        {

        }
    }
}
