using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace PersonalityQuiz
{
    public partial class MainPage : ContentPage
    {
        private readonly List<QuizQuestion> questionList;
        private readonly List<Character> characters;
        private int currentIndex;
        private static readonly Random rnd = new();
        private bool quizFinished = false;

        public MainPage()
        {
            InitializeComponent();

            questionList = new List<QuizQuestion>()
            {
                new QuizQuestion(
                    "You rather manage a new island development than relax on the beach",
                    "island.jpg",
                    2, 0, 0, 0, 0,
                    0, 1, 0, 0, 0
                ),

                new QuizQuestion(
                    "You enjoy organizing events and helping others feel welcomed",
                    "desk.jpg",
                    0, 2, 0, 0, 0,
                    0, 0, 1, 0, 0
                ),

                new QuizQuestion(
                    "Playing or creating music instantly lift your mood",
                    "guitar.jpg",
                    0, 0, 2, 0, 0,
                    0, 0, 0, 1, 0
                ),

                new QuizQuestion(
                    "You prefer having a stylish, compact lifestyle with high standards",
                    "cafe.jpg",
                    0, 0, 0, 2, 0,
                    0, 0, 0, 0, 1
                ),

                new QuizQuestion(
                    "You choose a clever, business-like solution over something sentimental",
                    "computer.jpg",
                    0, 0, 0, 0, 2,
                    1, 0, 0, 0, 0
                )
            };

            characters = new List<Character>()
            {
                new Character("Tom Nook", "tomnook.png"),
                new Character("Isabelle", "isabelle.jpg"),
                new Character("K.K. Slider", "kkslider.jpg"),
                new Character("Marshal", "marshal.png"),
                new Character("Raymond", "raymond.png")
            };

            ResetQuiz();
        }

        private void ResetQuiz()
        {
            foreach (var c in characters)
                c.CharacterScore = 0;

            currentIndex = 0;
            quizFinished = false;

            ResultLabel.IsVisible = false;
            ResultImage.IsVisible = false;
            ResetButton.IsVisible = false;

            QuestionLabel.IsVisible = true;
            QuestionImage.IsVisible = true;

            QuizProgressBar.Progress = 0;
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (currentIndex < 0 || currentIndex >= questionList.Count)
                return;

            var q = questionList[currentIndex];
            QuestionLabel.Text = q.Text;
            QuestionImage.Source = q.ImageSource;
            QuizProgressBar.Progress = (double)currentIndex / questionList.Count;
            ProgressLabel.Text = $"{currentIndex + 1} / {questionList.Count}";
        }

        private void RecordAnswer(bool answeredTrue)
        {
            // Defensive guard: ignore answers if index is out of range
            if (currentIndex < 0 || currentIndex >= questionList.Count)
                return;

            var q = questionList[currentIndex];

            if (answeredTrue)
            {
                characters[0].CharacterScore += q.TrueTomNook;
                characters[1].CharacterScore += q.TrueIsabelle;
                characters[2].CharacterScore += q.TrueKKSlider;
                characters[3].CharacterScore += q.TrueMarshal;
                characters[4].CharacterScore += q.TrueRaymond;
            }
            else
            {
                characters[0].CharacterScore += q.FalseTomNook;
                characters[1].CharacterScore += q.FalseIsabelle;
                characters[2].CharacterScore += q.FalseKKSlider;
                characters[3].CharacterScore += q.FalseMarshal;
                characters[4].CharacterScore += q.FalseRaymond;
            }

            currentIndex++;

            if (currentIndex >= questionList.Count)
                ShowResult();
            else
                LoadQuestion();
        }

        private void ShowResult()
        {
            quizFinished = true;

            QuizProgressBar.Progress = 1;
            ProgressLabel.Text = $"{questionList.Count} / {questionList.Count}";

            int max = characters.Max(c => c.CharacterScore);
            var winners = characters.Where(c => c.CharacterScore == max).ToList();

            Character chosen;
            if (winners.Count == 1)
                chosen = winners[0];
            else
                chosen = winners[rnd.Next(winners.Count)];

            ResultLabel.Text = $"You match: {chosen.CharacterName}!";
            ResultImage.Source = chosen.CharacterImageSource;

            ResultLabel.IsVisible = true;
            ResultImage.IsVisible = true;
            ResetButton.IsVisible = true;

            // Hide the last question UI so it is not visible when showing results
            QuestionLabel.IsVisible = false;
            QuestionImage.IsVisible = false;
        }

        // Swipes on the question image drive answers:
        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            // Prevent swipes after quiz finished
            if (quizFinished)
                return;

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    RecordAnswer(false);
                    break;
                case SwipeDirection.Right:
                    RecordAnswer(true);
                    break;
                default:
                    // ignore Up/Down or other directions
                    break;
            }
        }

        private void OnResetClicked(object sender, EventArgs e) => ResetQuiz();
    }
}
