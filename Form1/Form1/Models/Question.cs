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
        public string Text { get; set; } //question text
        public QuestionType Type { get; set; }  // tf or mcq
        public string[] Options { get; set; }  // possible answers
        public int CorrectIndex { get; set; }  // index of the correct answer in Options array

        //Auto-property in get; set; cuz public field with private backing storage.
    }
}
