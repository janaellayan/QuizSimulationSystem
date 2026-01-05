using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form1.Models
{

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
    }
    // to classify the questions in our lists 
    public class Question
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public string[] Options { get; set; }
        public int CorrectIndex { get; set; }
    }
}
