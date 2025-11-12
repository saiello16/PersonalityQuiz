using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalityQuiz
{
    internal class Character
    {
        public string CharacterName { get; set; }
        public string CharacterImageSource { get; set; }
        public int CharacterScore { get; set; }

        public Character(string characterName, string characterImageSource)
        {
            CharacterName = characterName;
            CharacterImageSource = characterImageSource;
            CharacterScore = 0;
        }
    }
}
