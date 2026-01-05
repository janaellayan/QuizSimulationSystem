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
using System.IO;

namespace Form1
{
    public partial class Quiz_Form : Form
    {
        string studentID;
        string studentName;
        string chapterName;

        int totalQuestions = 5;
        int remainingSeconds = 300; // time for quiz (5 minutes = 300 seconds)
        DateTime quizStartTime;//to calculate time taken 

        private bool btnPressed = false;

        public Quiz_Form(string chapterName,string studentName,string studentID)
        {
            InitializeComponent();

            this.chapterName = chapterName;
            this.studentName = studentName;
            this.studentID = studentID;

            //show selected chapter number
            lblChapter.Text = chapterName;

            lblTimer.Text = "05:00"; //show initial time

            quizTimer.Interval = 1000;
            this.quizTimer.Tick += new System.EventHandler(this.quizTimer_Tick);
            quizTimer.Start();//start timer

            //finish button design
            btnFinish.BackColor = Color.Transparent;
            btnFinish.ForeColor = Color.White;
            btnFinish.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnFinish.Width = 130;
            btnFinish.Height = 50;

            btnFinish.Paint += btnFinish_Paint;

            btnFinish.MouseEnter += (s, e) => btnFinish.BackColor = Color.SteelBlue;
            btnFinish.MouseLeave += (s, e) => btnFinish.BackColor = Color.DodgerBlue;

            btnFinish.MouseDown += (s, e) => { btnPressed = true; btnFinish.Invalidate(); };
            btnFinish.MouseUp += (s, e) => { btnPressed = false; btnFinish.Invalidate(); };

        }

        int CalculateGrade(int score)
        {
            double percent = (double)score / totalQuestions * 100;
            if (percent >= 90) return 5;
            if (percent >= 80) return 4;
            if (percent >= 70) return 3;
            if (percent >= 60) return 2;
            if (percent >= 50) return 1;

            return 0;
        }

        int GetAttemptNumber()
        {
            string filepath = "QuizResults.csv";
             if(!File.Exists(filepath)) return 1;

            int attempts = 0;
            string[] lines =File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                if(line.StartsWith(studentID+",") && line.Contains("," + chapterName + ","))
                {
                    attempts++;
                }
            }
            return attempts +1;
        }

        void saveResultToCSV(int score,string timeTaken)
        {
            string filePath = "QuizResults.csv";

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "StudentID,StudentName,Chapter,Score," +
                    "TotalQuestions,Grade,TimeTaken,Attempt,DateTime\n");
            }
            int attempt=GetAttemptNumber();
            int grade = CalculateGrade(score);

            string line=
                studentID +","+
                studentName +","+
                chapterName +","+
                score +","+
                totalQuestions +","+
                grade +"/5,"+
                timeTaken +","+
                attempt +","+
                DateTime.Now.ToString("yyyy-MM-dd HH:mm") +"\n";

            File.AppendAllText(filePath, line);
        }

        private void Quiz_Form_Load(object sender, EventArgs e)
        {
            quizStartTime = DateTime.Now; //save start time
            remainingSeconds = 300;
        }

        private void quizTimer_Tick(object sender, EventArgs e)
        {
            // decrease time every second
            remainingSeconds--;

            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;

            //update timer label
            lblTimer.Text = minutes.ToString("D2") + ":" + seconds.ToString("D2");

            //if time finish
            if (remainingSeconds <= 0)
            {
                quizTimer.Stop();
                FinishQuiz();
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            FinishQuiz();
        }

        void FinishQuiz()
        {
            quizTimer.Stop();

            DateTime quizEndTime = DateTime.Now; // end time
            TimeSpan timeTaken = quizEndTime - quizStartTime; // calculate time taken

            string finalTime =
                timeTaken.Minutes.ToString("D2") + ":" +
                timeTaken.Seconds.ToString("D2");

            int score = 0;
            saveResultToCSV(score, finalTime);
            Result_Form resultForm = new Result_Form(score, finalTime);
            resultForm.Show();
            this.Hide();
        }

        private void btnFinish_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = btn.ClientRectangle;

            // Change color if button is pressed
            Color startColor = btnPressed ? Color.FromArgb(53, 122, 189) : Color.DodgerBlue;
            Color endColor = btnPressed ? Color.FromArgb(33, 102, 156) : Color.SteelBlue;

            using (GraphicsPath path = GetRoundedPath(rect, 20))
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, startColor, endColor, LinearGradientMode.Horizontal))
            {
                btn.Region = new Region(path); // make button rounded
                e.Graphics.FillPath(brush, path); // fill with gradient
            }

            // Draw text in center
            TextRenderer.DrawText(
                e.Graphics,
                btn.Text,
                btn.Font,
                rect,
                Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90); // top-left
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90); // top-right
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90); // bottom-right
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90); // bottom-left

            path.CloseFigure();
            return path;
        }

        private void btnFinish_Click_1(object sender, EventArgs e)
        {
            FinishQuiz(); 
        }

        private void lblTimer_Click(object sender, EventArgs e)
        {

        }

        private void pnlQuestion_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
