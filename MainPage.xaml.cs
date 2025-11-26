using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using PersonalityQuiz.Services;

namespace PersonalityQuiz
{
    public partial class MainPage : ContentPage
    {
        private readonly QuestionService _questionService;
        private int _currentIndex = 0;
        private bool _loaded = false;

        public MainPage(QuestionService questionService)
        {
            InitializeComponent();
            _questionService = questionService;

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, EventArgs e)
        {
            // Load questions from database
            await _questionService.LoadQuestionsAsync();

            if (_questionService.TotalQuestions == 0)
            {
                await DisplayAlert("Error", "No questions found in database.", "OK");
                return;
            }

            _loaded = true;
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (!_loaded)
                return;

            var q = _questionService.GetQuestion(_currentIndex);

            if (q == null)
            {
                NavigateToResults();
                return;
            }

            QuestionLabel.Text = q.Text;
            QuestionImage.Source = q.ImageSource;

            QuizProgressBar.Progress = (double)_currentIndex / _questionService.TotalQuestions;
            ProgressLabel.Text = $"{_currentIndex + 1} / {_questionService.TotalQuestions}";
        }

        private async Task HandleAnswerAsync(bool answer)
        {
            var q = _questionService.GetQuestion(_currentIndex);

            if (q == null)
            {
                NavigateToResults();
                return;
            }

            // Save answer to DB-based score logic
            _questionService.RecordAnswer(q, answer);

            // Animate card swipe
            await QuestionImage.TranslateTo(answer ? 300 : -300, 0, 250, Easing.CubicIn);
            await QuestionImage.FadeTo(0, 150);

            // Reset position
            QuestionImage.TranslationX = 0;
            await QuestionImage.FadeTo(1, 150);

            // Next question
            _currentIndex++;

            if (_currentIndex >= _questionService.TotalQuestions)
                NavigateToResults();
            else
                LoadQuestion();
        }

        private async void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (!_loaded)
                return;

            if (e.Direction == SwipeDirection.Right)
                await HandleAnswerAsync(true);
            else if (e.Direction == SwipeDirection.Left)
                await HandleAnswerAsync(false);
        }

        private async void NavigateToResults()
        {
            var resultCharacter = _questionService.CalculateResult();

            await Navigation.PushAsync(new ResultPage(resultCharacter));

            // Reset for next quiz attempt if you return later
            _currentIndex = 0;
        }
    }
}
