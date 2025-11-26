using SQLite;
using Microsoft.Maui.Controls;

namespace PersonalityQuiz.Models
{
    public class Question
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; }
        public string ImagePath { get; set; }

        // Expose an ImageSource the UI can use directly
        [Ignore]
        public ImageSource ImageSource => string.IsNullOrWhiteSpace(ImagePath) ? null : ImageSource.FromFile(ImagePath);

        // Store values exactly like your scoring system
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
    }
}
