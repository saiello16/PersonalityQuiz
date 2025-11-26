using Microsoft.Maui.Controls;

namespace PersonalityQuiz.Models
{
    public class Character
    {
        public string CharacterName { get; set; }
        public string ImagePath { get; set; }

        // UI-friendly image source
        public ImageSource CharacterImageSource => string.IsNullOrEmpty(ImagePath) ? null : ImageSource.FromFile(ImagePath);

        public string Description { get; set; }
    }
}