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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Button basic design settings
            btnStart.BackColor = Color.Transparent;
            btnStart.ForeColor = Color.White;
            btnStart.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnStart.Width = 130;
            btnStart.Height = 50;

            // Connect custom paint event to draw the button style
            btnStart.Paint += btnStart_Paint;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Clear items before adding new ones
            comboChapters.Items.Clear();

            // Add chapters to the dropdown list
            comboChapters.Items.Add("Chapter 1");
            comboChapters.Items.Add("Chapter 2");
            comboChapters.Items.Add("Chapter 3");
            comboChapters.Items.Add("Chapter 4");
            comboChapters.Items.Add("Chapter 5");
            comboChapters.Items.Add("Chapter 6");
            comboChapters.Items.Add("Chapter 7");

            // Select the first chapter by default
            comboChapters.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
        }

        // This function creates a rounded rectangle shape
        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Draw rounded corners
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        // Custom drawing for the button (rounded + gradient)
        private void btnStart_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = btn.ClientRectangle;

            using (GraphicsPath path = GetRoundedPath(rect, 22))
            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                Color.FromArgb(74, 144, 226),   // Gradient start color
                Color.FromArgb(53, 122, 189),   // Gradient end color
                LinearGradientMode.Horizontal))
            {
                // Apply rounded shape to the button
                btn.Region = new Region(path);

                // Fill the button with gradient color
                e.Graphics.FillPath(brush, path);
            }

            // Draw button text in the center
            TextRenderer.DrawText(
                e.Graphics,
                btn.Text,
                btn.Font,
                rect,
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
    }
}
