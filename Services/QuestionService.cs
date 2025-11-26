using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalityQuiz.Data;
using PersonalityQuiz.Models;
using System;

namespace PersonalityQuiz.Services
{
    public class QuestionService
    {
        private readonly QuizDatabase _db;

        public List<Question> Questions { get; private set; } = new();
        public List<ResultProfile> Profiles { get; private set; } = new();
        public Dictionary<string, int> Scores { get; private set; } = new();

        public QuestionService(QuizDatabase db)
        {
            _db = db;
        }

        // Used by MainPage
        public int TotalQuestions => Questions?.Count ?? 0;

        public async Task LoadQuestionsAsync()
        {
            Questions = await _db.GetQuestionsAsync();
            Profiles = await _db.GetProfilesAsync();

            // Initialize/reset scores from loaded profiles so keys always match profile titles
            InitializeScoresFromProfiles();
        }

        private void InitializeScoresFromProfiles()
        {
            Scores = new Dictionary<string, int>();

            if (Profiles != null && Profiles.Count > 0)
            {
                foreach (var p in Profiles)
                {
                    Scores[p.Title] = 0;
                }
            }
            else
            {
                // Fallback (keeps previous hard-coded keys for compatibility)
                Scores = new Dictionary<string, int>
                {
                    {"Tom Nook", 0},
                    {"Isabelle", 0},
                    {"K.K. Slider", 0},
                    {"Marshal", 0},
                    {"Raymond", 0}
                };
            }
        }

        // Optional: allow explicit reset between attempts
        public void ResetScores()
        {
            if (Scores == null)
                InitializeScoresFromProfiles();
            else
            {
                var keys = Scores.Keys.ToList();
                foreach (var k in keys)
                    Scores[k] = 0;
            }
        }

        public Question GetQuestion(int index)
        {
            if (Questions == null || index < 0 || index >= Questions.Count)
                return null;
            return Questions[index];
        }

        public void RecordAnswer(Question q, bool isTrue)
        {
            if (q == null || Scores == null)
                return;

            if (isTrue)
            {
                if (Scores.ContainsKey("Tom Nook")) Scores["Tom Nook"] += q.TrueTomNook;
                if (Scores.ContainsKey("Isabelle")) Scores["Isabelle"] += q.TrueIsabelle;
                if (Scores.ContainsKey("K.K. Slider")) Scores["K.K. Slider"] += q.TrueKKSlider;
                if (Scores.ContainsKey("Marshal")) Scores["Marshal"] += q.TrueMarshal;
                if (Scores.ContainsKey("Raymond")) Scores["Raymond"] += q.TrueRaymond;
            }
            else
            {
                if (Scores.ContainsKey("Tom Nook")) Scores["Tom Nook"] += q.FalseTomNook;
                if (Scores.ContainsKey("Isabelle")) Scores["Isabelle"] += q.FalseIsabelle;
                if (Scores.ContainsKey("K.K. Slider")) Scores["K.K. Slider"] += q.FalseKKSlider;
                if (Scores.ContainsKey("Marshal")) Scores["Marshal"] += q.FalseMarshal;
                if (Scores.ContainsKey("Raymond")) Scores["Raymond"] += q.FalseRaymond;
            }
        }

        // Returns a Character object MainPage/ResultPage expect
        public Character CalculateResult()
        {
            if (Scores == null || Scores.Count == 0)
                return new Character { CharacterName = "Unknown", Description = "" };

            // Determine top score(s)
            var maxValue = Scores.Values.Max();
            var topCandidates = Scores.Where(kv => kv.Value == maxValue).Select(kv => kv.Key).ToList();

            // Randomize tie-breaker so the same profile isn't always picked when tied
            var rng = new Random();
            var top = topCandidates[rng.Next(topCandidates.Count)];

            var profile = Profiles?.FirstOrDefault(p => p.Title == top) ?? Profiles?.FirstOrDefault();

            if (profile == null)
            {
                return new Character
                {
                    CharacterName = top,
                    Description = "",
                    ImagePath = ""
                };
            }

            return new Character
            {
                CharacterName = profile.Title,
                Description = profile.Description,
                ImagePath = profile.ImagePath
            };
        }
    }
}
