using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SquareGrid.Common;
using SquareGrid.Interfaces;
using SquareGrid.UI;
using SquareGrid.UI.Menus;
using SquareGrid.UI.Square_Grid;
using SquareGrid.Utilities;

namespace SquareGrid.States
{
    public class PlayerState : Component
    {        
        private readonly StartState _startState;
        private readonly List<Player> _players;
        private readonly List<int> _colors;

        private readonly Menu _menu;
        private readonly MenuItem _startNextItem;
        private readonly NameMenuItem _nameItem;
        private readonly ColorMenuItem _colorItem;
        private readonly PlayerTypeMenuItem _typeItem;
        private readonly MenuItem _backItem;
        private Player _player;
        private readonly Tween _invalidName;

        public PlayerState(BaseGame game, StartState startState, List<Player> players, List<int> colors)
            : base(game, startState)
        {
            //game.KeyboardInput.IsOSKVisable = true;
            _startState = startState;
            _players = players;
            _colors = colors;
            _player = new Player
            {
                Color = colors[0],
                Name = game.GameData.Data.PlayerNames[players.Count],
                Score = 0,
                Type = players.Count == 0 ? PlayerType.Human : PlayerType.Computer
            };
            _startNextItem = new MenuItem(game,
                                     game.Font.MeasureString(new Vector2(10, 100),
                                     ((players.Count == startState.GameType.Players - 1)
                                     ? Strings.Start : Strings.Next)),
                                     ((players.Count == startState.GameType.Players - 1)
                                     ? Strings.Start : Strings.Next));
            _nameItem = new NameMenuItem(game, game.Font.MeasureString(
                                                   new Vector2(10, _startNextItem.Bounds.Y + _startNextItem.Bounds.Height),
                                                   _player.Name),
                () => _player.Name,
                p => { _player.Name = p; }, () => _player.Color, Strings.Name);
            int max = 0;
            int mindex = 0;
            for (var index = 0; index < colors.Count; index++)
            {
                var color = colors[index];
                var m = game.Font.MeasureString(
                    new Vector2(10, _nameItem.Bounds.Y + _nameItem.Bounds.Height),
                    GameColors.Colors[color].Name.ToUpperInvariant());
                if (m.Width > max)
                {
                    max = m.Width;
                    mindex = index;
                }
            }
            _colorItem = new ColorMenuItem(game,
                                               game.Font.MeasureString(
                                                   new Vector2(10, _nameItem.Bounds.Y + _nameItem.Bounds.Height),
                                                   GameColors.Colors[colors[mindex]].Name.ToUpperInvariant()), () => _player.Color,
                                           delegate(int p) { _player.Color = p; }, Strings.Color, colors);
            _typeItem = new PlayerTypeMenuItem(game,
                                               game.Font.MeasureString(
                                                   new Vector2(10, _colorItem.Bounds.Y + _colorItem.Bounds.Height),
                                                   Strings.Computer), () => _player.Type, p =>
                                                   {
                                                       _player.Type = p;
                                                   }, Strings.Type);
            _backItem = new MenuItem(game,
                        game.Font.MeasureString(
                            new Vector2(10, _typeItem.Bounds.Y + _typeItem.Bounds.Height),
                            Strings.Back), Strings.Back);
            _menu = new Menu(game, HandleMenuSelect,
                            _startNextItem,
                            _nameItem,
                            _colorItem,
                            _typeItem,
                            _backItem
                );
            _invalidName = new Tween(new TimeSpan(0, 0, 0, 1), 1, 0);
            _invalidName.Finish();
        }

        public void HandleMenuSelect(MenuItem item)
        {
            switch (item.Name)
            {
                case Strings.Next:
                    {
                        if (_player.Name == string.Empty)
                        {
                            _invalidName.Reset();
                        }
                        else
                        {
                            Game.GameData.Data.PlayerNames[_players.Count] = _player.Name;
                            _players.Add(_player);

                            var c = new List<int>(_colors);
                            c.Remove(_player.Color);
                            NextComponent = new PlayerState(Game, _startState, _players, c);
                        }
                        break;
                    }
                case Strings.Start:
                    {
                        if (_player.Name == string.Empty)
                        {
                            _invalidName.Reset();
                        }
                        else
                        {
                            Game.GameData.Data.PlayerNames[_players.Count] = _player.Name;
                            _players.Add(_player);

                            NextComponent = new GameVSState(Game,this, _startState.GameType, _players.ToArray());
                        }
                        break;
                    }
                case Strings.Name:
                    {
                        break;
                    }
                case Strings.Color:
                    {
                        var i = item as ColorMenuItem;
                        if (i != null)
                        {
                           // i.Update();
                        }
                        break;
                    }
                case Strings.Type:
                    {
                        var i = item as PlayerTypeMenuItem;
                        if (i != null)
                        {
                            //i.Update();
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
            if (NextComponent != null)
                return NextComponent;
            _menu.Update(gameTime);
            _invalidName.Update(gameTime.ElapsedGameTime);
            return this;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            _menu.Draw(gameTime);
            var x = Game.Font.MeasureString(Strings.InvalidName);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Game.Font, Strings.InvalidName, new Vector2(Center.X - (x.X / 2), Center.Y - (x.Y / 2)), Color.White * _invalidName);
            SpriteBatch.End();
        }

        public override void Back()
        {
            if (_players.Count == 0)
            {
                NextComponent = PreviousComponent;
            }
            else
            {

                if (_players.Count >= 1)
                {
                    _colors.Add(_players[_players.Count - 1].Color);
                    _players.Remove(_players[_players.Count - 1]);
                    NextComponent = new PlayerState(Game, _startState, _players, _colors);

                }
                else
                {
                    NextComponent = PreviousComponent;
                }
            }
        }
    }
}
