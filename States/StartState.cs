using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SquareGrid.Common;
using SquareGrid.Interfaces;
using SquareGrid.UI;
using SquareGrid.UI.Menus;
using SquareGrid.UI.Square_Grid;
using SquareGrid.Utilities;

namespace SquareGrid.States
{
    public class StartState : Component
    {
        protected internal GameType GameType;        
        private readonly Menu _menu;
        private readonly MenuItem _nextItem;
        private readonly PlayersMenuItem _playerItem;
        private readonly GridMenuItem _gameGridItem;
        private readonly DifficultyMenuItem _difficultyItem;
        private readonly MenuItem _backItem;
        private readonly Tween _stageLocked;

        public StartState(BaseGame grid, IComponent parent)
            : base(grid,parent)
        {
            var g = 0;
            for (int index = 0; index < grid.GameData.Data.GameGridsVsMode.Count; index++)
            {
                if (grid.GameData.Data.GameGridsVsMode[index].Locked)
                {
                    g = index - 1;
                    break;
                }
            }
            GameType = new GameType { Players = 2, Difficulty = grid.GameData.Data.Difficulty, Grid = g };
           // _nextComponent = this;
            _nextItem = new MenuItem(grid,
                                    grid.Font.MeasureString(
                                        new Vector2(10, 100),
                                        Strings.Next), Strings.Next);
            _playerItem = new PlayersMenuItem(grid,
                                    grid.Font.MeasureString(
                                        new Vector2(10, _nextItem.Bounds.Y + _nextItem.Bounds.Height),
                                        Strings.PlayersMStr), () => GameType.Players, p => { GameType.Players = p; }, "PLAYERS");
            _gameGridItem = new GridMenuItem(grid, grid.Font.MeasureString(new Vector2(10, _playerItem.Bounds.Y + _playerItem.Bounds.Height), "GRID 00x00"),
                () => GameType.Grid,
                p => { GameType.Grid = p; }, Strings.Grid);
            _difficultyItem = new DifficultyMenuItem(grid,
                                    grid.Font.MeasureString(
                                        new Vector2(10, _gameGridItem.Bounds.Y + _gameGridItem.Bounds.Height), "NORMAL"), () => GameType.Difficulty, p =>
                                        {
                                            GameType.Difficulty = p;
                                            grid.GameData.Data.Difficulty = p;
                                        },
                                        Strings.Difficulty);
            _backItem = new MenuItem(grid,
                                    grid.Font.MeasureString(
                                        new Vector2(10, _difficultyItem.Bounds.Y + _difficultyItem.Bounds.Height),
                                        Strings.Back), Strings.Back);
            _menu = new Menu(grid, HandleMenuSelect,
                            _nextItem,
                            _playerItem,
                            _gameGridItem,
                            _difficultyItem,
                            _backItem
                );
            _stageLocked = new Tween(new TimeSpan(0, 0, 0, 1), 1, 0);
            _stageLocked.Finish();
        }
        public void HandleMenuSelect(MenuItem item)
        {
            switch (item.Name)
            {
                case Strings.Next:
                    {
                        if (Game.GameData.Data.GameGridsVsMode[GameType.Grid].Locked)
                        {
                            _stageLocked.Reset();
                        }
                        else
                        {
                            NextComponent = new PlayerState(Game, this, new List<Player>(), new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
                        }
                        break;
                    }
                case Strings.Players:
                    {
                        var i = item as PlayersMenuItem;
                        if (i != null)
                        {
                            i.Update();
                        }
                        break;
                    }
                case Strings.Grid:
                    {
                        var i = item as GridMenuItem;
                        if (i != null)
                        {
                            //i.Update();
                        }
                        break;
                    }
                case Strings.Difficulty:
                    {
                        var i = item as DifficultyMenuItem;
                        if (i != null)
                        {
                           // i.Update();
                        }
                        break;
                    }
                default:
                    {
                        Back();
                        break;
                    }
            }            
        }
        public override IComponent Update(GameTime gameTime, BaseGame game)
        {
            _menu.Update(gameTime);
            _stageLocked.Update(gameTime.ElapsedGameTime);
            return NextComponent ?? this;;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            _menu.Draw(gameTime);
            var x = Game.Font.MeasureString(Strings.Locked);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Strings.Locked, new Vector2(Center.X - (x.X / 2), Center.Y - (x.Y / 2)), Color.White * _stageLocked);
            SpriteBatch.End();
        }
    }
}
