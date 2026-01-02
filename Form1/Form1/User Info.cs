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
    public partial class Form1 : Form
    {
        // variable to track button pressed state
        private bool btnPressed = false;

        public Form1()
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

        private void Form1_Load(object sender, EventArgs e)
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

            // open only Form2 and pass selected chapter
            Form2 form2 = new Form2(selectedChapter);
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
            String id = txtID.Text;
            try
            {
                // checks for whitespaces, or any non-number characters
                if (String.IsNullOrEmpty(id) || !int.TryParse(id, out int number))
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ID number cannot be empty or anything other than numbers. ", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {                  
            String name = txtName.Text;

            try {
                // checks for whitespaces, or any non-letter characters, while allowing spaces between names
                if (String.IsNullOrEmpty(name) || !name.Any(char.IsLetter)|| !name.All(c => char.IsLetter(c) || c == ' '))
                {
                    throw new Exception();
                }
            }
            catch (Exception) { 
                MessageBox.Show("Name cannot be empty or anything other than letters. ", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
