using Microsoft.Extensions.Logging;
using PersonalityQuiz.Data;
using PersonalityQuiz.Services;

namespace PersonalityQuiz
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            string dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "quiz.db"
        );

            builder.Services.AddSingleton<QuizDatabase>(s => new QuizDatabase(dbPath));
            builder.Services.AddSingleton<QuestionService>();
            return builder.Build();
        }
    }
}
