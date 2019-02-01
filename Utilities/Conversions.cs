using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SquareGrid.Common;

namespace SquareGrid.Utilities
{
    public static class Conversion
    {
        public static string DifficultyToString(Difficulty difficulty)
        {
            string str;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    str = "EASY";
                    break;
                case Difficulty.Normal:
                    str = "NORMAL";
                    break;
                case Difficulty.Hard:
                    str = "HARD";
                    break;
                default:
                    str = "NORMAL";
                    break;
            }
            return str;
        }

        public static Difficulty ToDifficulty(string str)
        {
            str = str.Trim().ToUpperInvariant();
            var diff = Difficulty.Normal;
            if (str.Equals("EASY", StringComparison.CurrentCultureIgnoreCase))
            {
                diff = Difficulty.Easy;
            }
            else if (str.Equals("NORMAL", StringComparison.CurrentCultureIgnoreCase))
            {
                diff = Difficulty.Normal;
            }
            else if (str.Equals("HARD", StringComparison.CurrentCultureIgnoreCase))
            {
                diff = Difficulty.Hard;
            }
            return diff;
        }

        public static string PlayerTypeToString(PlayerType playerType)
        {
            string str;
            switch (playerType)
            {
                case PlayerType.Human:
                    str = Strings.Human;
                    break;
                case PlayerType.Computer:
                    str = Strings.Computer;
                    break;
                default:
                    str = Strings.Human;
                    break;
            }
            return str;
        }

        public static PlayerType ToPlayerType(string str)
        {
            str = str.Trim().ToUpperInvariant();
            var diff = PlayerType.Human;
            if (str.Equals(Strings.Human, StringComparison.CurrentCultureIgnoreCase))
            {
                diff = PlayerType.Human;
            }
            else if (str.Equals(Strings.Computer, StringComparison.CurrentCultureIgnoreCase))
            {
                diff = PlayerType.Computer;
            }
            return diff;
        }

        public static string HighScoreToString(HighScore highScore)
        {
            return highScore.Score + Strings.Space + highScore.Color + Strings.Space + highScore.Name.Trim().ToUpperInvariant();
        }

        public static HighScore ToHighScore(string str)
        {
            var score = new HighScore();
            if (str == null || str.ToCharArray().Count(p => p == ' ') < 3)
                return new HighScore
                {
                    Score = (BaseGame.Random.Next(100)*5),
                    Color = BaseGame.Random.Next(BaseGame.Random.Next(GameColors.Colors.Length)),
                    Name = Names.GetName()
                };
            var s = string.Empty;
            var hIndex = 0;

            for (var index = 0; index < str.ToCharArray().Length; index++)
            {
                var c = str.ToCharArray()[index];
                if (c == ' ')
                {
                    switch (hIndex)
                    {
                        case 0:
                        {
                            int sout;
                            if (Int32.TryParse(s, out sout))
                            {
                                score.Score = sout;
                            }
                            else
                            {
                                score.Score = (BaseGame.Random.Next(100) * 5);
                            }
                            s = string.Empty;
                            break;
                        }
                        case 1:
                        {
                            int sout;
                            if (Int32.TryParse(s, out sout))
                            {
                                score.Color = sout < GameColors.Colors.Length ? sout : BaseGame.Random.Next(GameColors.Colors.Length);
                            }
                            else
                            {
                                score.Color = BaseGame.Random.Next(GameColors.Colors.Length);
                            }
                            s = string.Empty;
                            break;
                        }
                        case 2:
                        {
                            if (index == str.ToCharArray().Length)
                            {
                                var n = s.Trim().ToUpperInvariant();
                                score.Name = String.IsNullOrWhiteSpace(n) ? Names.GetName() : n;
                            }
                            else
                            {
                                s += c;
                            }
                            break;
                        }
                    }
                    hIndex++;
                }
                else
                {
                    s += c;
                }
            }
            return score;
        }
    }
}
