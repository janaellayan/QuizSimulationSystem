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
using Form1.Models;


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

        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private List<int> userSelections = new List<int>();  //user choices to track the score 
        private int score = 0; // total as we run the code

        bool AllQuestionsAnswered()
        {
            int answeredCount = 0;

            foreach (int answer in userSelections)
            {
                if (answer != -1)
                    answeredCount++;
            }

            return answeredCount == questions.Count;
        }
        //to check if all the questions are answered before submiting

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
            string grade = $"{score} out of {totalQuestions}";
            //used the out of so excel dosent think its a date!

            string line=
                studentID +","+
                studentName +","+
                chapterName +","+
                score +","+
                totalQuestions +","+
                grade +","+
                timeTaken +","+
                attempt +","+
                DateTime.Now.ToString("yyyy-MM-dd HH:mm") +"\n";

            File.AppendAllText(filePath, line);
        }

        private void Quiz_Form_Load(object sender, EventArgs e)
        {
            quizStartTime = QuizState.StartTime; // start time
            remainingSeconds = 300;
            questions = QuizState.SelectedQuestions;
            currentQuestionIndex = 0;
            ShowCurrentQuestion();
        }



        //questions display method
        private void ShowCurrentQuestion()
        {
            // if we still didnt load questions or if we are done with five questions
            if (questions == null || currentQuestionIndex >= questions.Count) return;
            Question q = questions[currentQuestionIndex];

            lblQnum.Text = $"Question {currentQuestionIndex + 1}/5"; // +1 because index starts from 0
            lblQtext.Text = q.Text;  // maiin question text

            //there are four radio buttons always
            // if the question is true/false we only use two and the other two are hidden 
            if (q.Type == QuestionType.TrueFalse)
            {
                radioA.Text = q.Options[0];  // True
                radioB.Text = q.Options[1];  // False
                radioC.Visible = false;
                radioD.Visible = false;
                radioA.Visible = radioB.Visible = true;
            }
            else
            {
                radioA.Text = q.Options[0];
                radioB.Text = q.Options[1];
                radioC.Text = q.Options[2];
                radioD.Text = q.Options[3];
                radioA.Visible = radioB.Visible = radioC.Visible = radioD.Visible = true;
            }

            // clear previous selections
            radioA.Checked = radioB.Checked = radioC.Checked = radioD.Checked = false;
        }

        // the questionsdisplay one at a time, so that we deal with 4 radio buttons only per question
        // 4 buttons * five questions = 20 buttons total
        // messy, so we do it this way

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
                MessageBox.Show("Time's up! Quiz submitted automatically.", "Time Expired",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FinishQuiz();
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            //save the current questions answer
            int selected = -1;

            if (radioA.Checked)
                selected = 0;
            else if (radioB.Checked)
                selected = 1;
            else if (radioC.Checked)
                selected = 2;
            else if (radioD.Checked)
                selected = 3;

            if (userSelections.Count > currentQuestionIndex)
                userSelections[currentQuestionIndex] = selected;
            else
                userSelections.Add(selected);


            //check if answers are correct
            if (!AllQuestionsAnswered())
            {
                MessageBox.Show(
                    "Please answer all questions before submitting the quiz.",
                    "Incomplete Quiz",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            //confirmation message
            DialogResult result = MessageBox.Show(
                "Are you sure you want to submit the quiz?",
                "Confirm Submission",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                FinishQuiz();
            }
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
            for (int i = 0; i < questions.Count; i++)
            {
                if (i < userSelections.Count && userSelections[i] == questions[i].CorrectIndex && userSelections[i]!=-1)
                {
                    score++;
                }
            }

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

        private void lblTimer_Click(object sender, EventArgs e)
        {

        }

        private void pnlQuestion_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // answer index
            // here we record the answer before moving to next question
            int selected = -1;
            if (radioA.Checked)
                selected = 0;
            else if (radioB.Checked)
                selected = 1;
            else if (radioC.Checked)
                selected = 2;
            else if (radioD.Checked)
                selected = 3;
            // now this works 10/10
            // adds the selected answer index to the list
            //userSelections.Add(selected);
            //currentQuestionIndex++;
            if (userSelections.Count > currentQuestionIndex)
                userSelections[currentQuestionIndex] = selected;
            else
                userSelections.Add(selected);

            currentQuestionIndex++;

            // validation to ensure an answer is selected
            if (selected == -1)
            {
                MessageBox.Show("Please select an answer!","No answer selected",
                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            if (currentQuestionIndex < questions.Count)
            {
                ShowCurrentQuestion();
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "This is the last question. Do you want to submit the quiz?",
                    "Confirm Submission",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    FinishQuiz();
                }
                else
                {
                    currentQuestionIndex--; //goes back to the last question
                    ShowCurrentQuestion();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                ShowCurrentQuestion();
            }
            else return; // no going back from question 1
            //fixed a big issue here

            // restore the last answer 
            // without this part, the back button will shpw empty selections always
            // confusing for users and bad UX
            int previousSelection = userSelections[currentQuestionIndex];
            if (previousSelection != -1)   // if they answered this ome and didn't leave it empty
            {
                if (previousSelection == 0)
                    radioA.Checked = true;
                else if (previousSelection == 1)
                    radioB.Checked = true;
                else if (previousSelection == 2)
                    radioC.Checked = true;
                else if (previousSelection == 3)
                    radioD.Checked = true;
            }
        }
    }
}
