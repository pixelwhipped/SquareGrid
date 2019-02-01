using System.Collections.Generic;
using SquareGrid.Common;

namespace SquareGrid
{
    public class GameData
    {
        public string[] PlayerNames;
        public Difficulty Difficulty;
        public List<HighScore> HighScores;
        public List<GameGrid> GameGridsVsMode;  
        public GameData()
        {
            PlayerNames = new[]
            {
                Strings.Player1,
                Strings.Player2,
                Strings.Player3,
                Strings.Player4,
                Strings.Player5,
                Strings.Player6
            };
            Difficulty = Difficulty.Easy;
            HighScores = new List<HighScore>(new[]
            {
                new HighScore
                {Color = 1, Name = Strings.Highscore1, Score = 480},
                new HighScore
                {Color = 2, Name = Strings.Highscore2, Score = 410},
                new HighScore
                {Color = 3, Name = Strings.Highscore3, Score = 280},
                new HighScore
                {Color = 4, Name = Strings.Highscore4, Score = 265},
                new HighScore
                {Color = 5, Name = Strings.Highscore5, Score = 245}
            });
            GameGridsVsMode = new List<GameGrid>(new[]
            {
                new GameGrid
                {
                    XTiles = 4,
                    YTiles = 4,
                    TileSize = 64,
                    Locked = false,
                    GameMode = GameMode.Vs
                },
                new GameGrid
                {
                    XTiles = 8,
                    YTiles = 4,
                    TileSize = 48,
                    Locked = true,
                    GameMode = GameMode.Vs
                },
                new GameGrid
                {
                    XTiles = 8,
                    YTiles = 8,
                    TileSize = 48,
                    Locked = true,
                    GameMode = GameMode.Vs
                },
                new GameGrid
                {
                    XTiles = 8,
                    YTiles = 15,
                    TileSize = 32,
                    Locked = true,
                    GameMode = GameMode.Vs
                },
                new GameGrid
                {
                    XTiles = 15,
                    YTiles = 15,
                    TileSize = 24,
                    Locked = true,
                    GameMode = GameMode.Vs
                }
            });
        }
                     
    }
}
