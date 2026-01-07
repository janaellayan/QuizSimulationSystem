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
    public partial class User_Info : Form
    {
        // variable to track button pressed state
        private bool btnPressed = false;

        public User_Info()
        {
            InitializeComponent();

            // Button basic design
            btnNext.BackColor = Color.Transparent;
            btnNext.ForeColor = Color.White;
            btnNext.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnNext.Width = 130;
            btnNext.Height = 50;

            // Custom paint event for rounded + gradient button
            btnNext.Paint += btnStart_Paint;

            // Mouse enter / leave effect (hover)
            btnNext.MouseEnter += (s, e) => btnNext.BackColor = Color.SteelBlue;
            btnNext.MouseLeave += (s, e) => btnNext.BackColor = Color.DodgerBlue;

            // Mouse down / up to show pressed effect
            btnNext.MouseDown += (s, e) => { btnPressed = true; btnNext.Invalidate(); };
            btnNext.MouseUp += (s, e) => { btnPressed = false; btnNext.Invalidate(); };
        }

        private void User_Info_Load(object sender, EventArgs e)
        {
            // Clear items first
            comboChapters.Items.Clear();

            // Add chapters to combo box
            comboChapters.Items.Add("Chapter 1");
            comboChapters.Items.Add("Chapter 2");
            comboChapters.Items.Add("Chapter 3");
            comboChapters.Items.Add("Chapter 4");
            comboChapters.Items.Add("Chapter 5");
            comboChapters.Items.Add("Chapter 6");
            comboChapters.Items.Add("Chapter 7");

            // Select nothing by default
            comboChapters.SelectedIndex = -1;
        }

        // Button click event to open second form
        private void btnStart_Click(object sender, EventArgs e)
        {
            string selectedChapter = comboChapters.SelectedItem.ToString();

            string studentName =txtName.Text;
            string studentID = txtID.Text;
            string selectedChapter = comboChapters.SelectedItem?.ToString();

            //check if the name field is empty
            if (string.IsNullOrEmpty(studentName))
            {
                MessageBox.Show("Please enter your name. ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            //check if the name contain only letters and space
            if (!studentName.All(c => char.IsLetter(c) || c==''))
            {
                MessageBox.Show("Name can only contain letters and spaces. ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            //check if the id field is empty 
            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show("Please enter your ID. ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtID.Focus();
                return;
            }

            //check if id contain number only 
            if (string.IsNullOrEmpty(studentID, out _))
            {
                MessageBox.Show("ID must be numbers only. ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtID.Focus();
                return;
            }

            // check if a chapter is selected
            if (string.IsNullOrEmpty(selectedChapter))
            {
                MessageBox.Show("Please select a chapter. ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboChapters.Focus();
                return;
            }

            // open only Form2 and pass selected chapter
            Form2 form2 = new Form2(selectedChapter,studentName,studentID);
            form2.Show();
            this.Hide();
        }

        // Function to make rounded rectangle
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

        // Custom drawing for the button
        private void btnStart_Paint(object sender, PaintEventArgs e)
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

        private void txtID_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)

        {
       
        }

        private void pnlQuestion_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
