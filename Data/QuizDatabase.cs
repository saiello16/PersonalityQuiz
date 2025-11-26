using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using PersonalityQuiz.Models;

namespace PersonalityQuiz.Data
{
    public class QuizDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public QuizDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Question>().Wait();
            _db.CreateTableAsync<ResultProfile>().Wait();

            Seed();
        }

        private void Seed()
        {
            // sqlite-net's AsyncTableQuery<T> doesn't have AnyAsync; use CountAsync instead
            if (_db.Table<Question>().CountAsync().Result == 0)
            {
                var defaultQuestions = new List<Question>
                {
                    new Question {
                        Text = "You rather manage a new island development than relax on the beach",
                        ImagePath = "island.jpg",
                        // All fields set explicitly to avoid implicit zeros causing imbalanced scoring
                        TrueTomNook = 2, TrueIsabelle = 0, TrueKKSlider = 0, TrueMarshal = 0, TrueRaymond = 0,
                        FalseTomNook = 0, FalseIsabelle = 1, FalseKKSlider = 0, FalseMarshal = 0, FalseRaymond = 0
                    },
                    new Question {
                        Text = "You enjoy organizing events and helping others feel welcomed",
                        ImagePath = "desk.jpg",
                        TrueTomNook = 0, TrueIsabelle = 2, TrueKKSlider = 0, TrueMarshal = 0, TrueRaymond = 0,
                        FalseTomNook = 0, FalseIsabelle = 0, FalseKKSlider = 1, FalseMarshal = 0, FalseRaymond = 0
                    },
                    new Question {
                        Text = "Playing or creating music instantly lifts your mood",
                        ImagePath = "guitar.jpg",
                        TrueTomNook = 0, TrueIsabelle = 0, TrueKKSlider = 2, TrueMarshal = 0, TrueRaymond = 0,
                        FalseTomNook = 0, FalseIsabelle = 0, FalseKKSlider = 0, FalseMarshal = 1, FalseRaymond = 0
                    },
                    new Question {
                        Text = "You prefer a stylish, compact lifestyle with high standards",
                        ImagePath = "cafe.jpg",
                        TrueTomNook = 0, TrueIsabelle = 0, TrueKKSlider = 0, TrueMarshal = 2, TrueRaymond = 0,
                        FalseTomNook = 0, FalseIsabelle = 0, FalseKKSlider = 0, FalseMarshal = 0, FalseRaymond = 1
                    },
                    new Question {
                        Text = "You choose a clever, business-like solution over sentimental ones",
                        ImagePath = "computer.jpg",
                        TrueTomNook = 0, TrueIsabelle = 0, TrueKKSlider = 0, TrueMarshal = 0, TrueRaymond = 2,
                        FalseTomNook = 1, FalseIsabelle = 0, FalseKKSlider = 0, FalseMarshal = 0, FalseRaymond = 0
                    }
                };

                _db.InsertAllAsync(defaultQuestions).Wait();
            }

            if (_db.Table<ResultProfile>().CountAsync().Result == 0)
            {
                var profiles = new List<ResultProfile>
                {
                    new ResultProfile { Title="Tom Nook", Description="The business-minded leader.", ImagePath="tomnook.png" },
                    new ResultProfile { Title="Isabelle", Description="The friendly helper!", ImagePath="isabelle.jpg" },
                    new ResultProfile { Title="K.K. Slider", Description="The musical creative soul.", ImagePath="kkslider.jpg" },
                    new ResultProfile { Title="Marshal", Description="Stylish, cool, confident.", ImagePath="marshal.png" },
                    new ResultProfile { Title="Raymond", Description="Analytical, clever, polished.", ImagePath="raymond.png" }
                };

                _db.InsertAllAsync(profiles).Wait();
            }
        }

        public Task<List<Question>> GetQuestionsAsync() =>
            _db.Table<Question>().ToListAsync();

        public Task<List<ResultProfile>> GetProfilesAsync() =>
            _db.Table<ResultProfile>().ToListAsync();
    }
}
