using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

    public static class QuizGenerator
    {
        private static readonly Random _random = new Random();
        // i added this outside of the method to avoid reseeding on each call 
        // which would lead to same sequences of random numbers
        // and thus not random at all
        // according to my research this is a common pitfall when using Random
        // also it's thread-safe in this context since we are not using multiple threads here
        // because we added readonly
        // im so happy with this part of code

        public static List<Question> RandomizeQuestions(string chapterName, int count = 5)
        {
            string path = Path.Combine(Application.StartupPath, "Questions", $"{chapterName}.json");
            List<Question> allQuestions = LoadQuestions(path);

            //until farfalla gets the rest of qs
            // works good with testing
            /*if (allQuestions.Count < count)
                throw new ArgumentException($"Need {count} questions, have {allQuestions.Count}");
            */ 

            // Fisher-Yates shuffle yummy
            List<Question> shuffled = new List<Question>(allQuestions);
            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                int swapIndex = _random.Next(i + 1);
                Question temp = shuffled[i];
                shuffled[i] = shuffled[swapIndex];
                shuffled[swapIndex] = temp;
            }

            return shuffled.Take(count).ToList();
        }
        private static List<Question> LoadQuestions(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Missing: {path}");
            //just in case 
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Question>>(json);
        }
    }
    }
