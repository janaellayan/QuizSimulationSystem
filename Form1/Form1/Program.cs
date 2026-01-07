using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form1
{
    internal static class Program
    {
        /// <summary>
        /// This quiz simulation system is a desktop application developed in C# using Visual Studio 2022,
        /// for our visual programming course. 
        /// The purpose of this project is to help students test their knowledge through structured quizzes 
        /// covering multiple subjects in a simple and engaging way.
        /// workflow: user info -> pre-quiz -> quiz -> results


        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new User_Info());
        }
    }
}
