using System;
using Microsoft.Maui.Controls;
using PersonalityQuiz.Models;

namespace PersonalityQuiz
{
    public partial class ResultPage : ContentPage
    {
        private readonly Character _character;

        public ResultPage(Character character)
        {
            InitializeComponent();
            _character = character ?? throw new ArgumentNullException(nameof(character));
            LoadResult();
        }

        private void LoadResult()
        {
            CharacterName.Text = _character.CharacterName ?? "Unknown";
            CharacterImage.Source = _character.CharacterImageSource;

            // Use the correct label for the description
            CharacterDescription.Text =
                string.IsNullOrEmpty(_character.Description)
                ? "You match this Animal Crossing character!"
                : _character.Description;

            // Ensure the title label is consistent with the page
            TitleLabel.Text = "Your Match";
        }

        private async void OnRetakeClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
