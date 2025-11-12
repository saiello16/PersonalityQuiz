using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalityQuiz
{
    internal class QuizQuestion
    {
        public string Text { get; set; }
        public string ImageSource { get; set; }

        public int TrueTomNook { get; set; }
        public int TrueIsabelle { get; set; }
        public int TrueKKSlider { get; set; }
        public int TrueMarshal { get; set; }
        public int TrueRaymond { get; set; }
        public int FalseTomNook { get; set; }
        public int FalseIsabelle { get; set; }
        public int FalseKKSlider { get; set; }
        public int FalseMarshal { get; set; }
        public int FalseRaymond { get; set; }

        public QuizQuestion(string text, string imageSource, int trueTomNook, int trueIsabelle, 
            int trueKKSlider, int trueMarshal, int trueRaymond, int falseTomNook, int falseIsabelle, 
            int falseKKSlider, int falseMarshal, int falseRaymond)
        {
            Text = text;
            ImageSource = imageSource;
            TrueTomNook = trueTomNook;
            TrueIsabelle = trueIsabelle;
            TrueKKSlider = trueKKSlider;
            TrueMarshal = trueMarshal;
            TrueRaymond = trueRaymond;
            FalseTomNook = falseTomNook;
            FalseIsabelle = falseIsabelle;
            FalseKKSlider = falseKKSlider;
            FalseMarshal = falseMarshal;
            FalseRaymond = falseRaymond;
        }
    }
}
