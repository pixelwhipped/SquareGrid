using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using SquareGrid.Common;
using SquareGrid.Content;
using SquareGrid.Interfaces;
using SquareGrid.UI;
using SquareGrid.UI.Menus;
using SquareGrid.UI.Square_Grid;
using SquareGrid.Utilities;

namespace SquareGrid.States
{
    public class MainMenu : Component
    {        

        private Vector2 _gText;
        private Vector2 _rText;
        private Vector2 _iText;
        private Vector2 _dText;

        private readonly Menu _menu;
        private readonly MenuItem _helpItem;
        private readonly MenuItem _settingsItem;
        private readonly MenuItem _startItem;
        private readonly MenuItem _highScoresItem;


        public MainMenu(BaseGame game,IComponent parent)
            : base(game,parent)
        {
            _gText = new Vector2(Center.X - (Textures.G.Width + Textures.R.Width),64);            
            _rText = new Vector2(Center.X - Textures.G.Width,64);            
            _iText = new Vector2(Center.X,64);            
            _dText = new Vector2(Center.X + Textures.I.Width,64);

            _startItem = new MenuItem(game,
                                     game.Font.MeasureString(new Vector2(10, 100), Strings.Start), Strings.Start);
            _helpItem = new MenuItem(game,
                                    game.Font.MeasureString(
                                        new Vector2(10, _startItem.Bounds.Y + _startItem.Bounds.Height),
                                        Strings.Help), Strings.Help);

            _settingsItem = new MenuItem(game,
                                    game.Font.MeasureString(
                                        new Vector2(10, _helpItem.Bounds.Y + _helpItem.Bounds.Height),
                                        Strings.Settings), Strings.Settings);
            _highScoresItem = new MenuItem(game,
                                    game.Font.MeasureString(
                                        new Vector2(10, _settingsItem.Bounds.Y + _settingsItem.Bounds.Height),
                                        Strings.HighScores), Strings.HighScores);

            _menu = new Menu(game, HandleMenuSelect,
                            _startItem,
                            _helpItem,
                            _settingsItem,
                            _highScoresItem
                //_exitItem
                );
        }

        public void HandleMenuSelect(MenuItem item)
        {
            switch (item.Name)
            {
                case Strings.Start:
                    {
                        NextComponent = new StartState(Game,this);
                        break;
                    }
                case Strings.Help:
                    {
                        NextComponent = new HelpState(Game,(IComponent)this);
                        break;
                    }
                case Strings.HighScores:
                    {
                        NextComponent = new HighScoreState(Game, this);
                        break;
                    }
                case Strings.Settings:
                    {
                        NextComponent = new GameSettings(Game, (IComponent)this);
                        break;
                    }
                default:
                    {
                        //GridGame.ExitGame();
                        break;
                    }
            }
        }
        public override IComponent Update(GameTime gameTime, BaseGame game)
        {
            if (NextComponent != null) 
                return NextComponent;
            _menu.Update(gameTime);
            return this;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            if (NextComponent != null) return;
            SpriteBatch.Begin();
                SpriteBatch.Draw(Textures.Square, new Vector2((Center.X - (Textures.Square.Width / 2f))-30f, 10f), Color.White);
                SpriteBatch.Draw(Textures.G, _gText, Color.White);
                SpriteBatch.Draw(Textures.R, _rText, Color.White);
                SpriteBatch.Draw(Textures.I, _iText, Color.White);
                SpriteBatch.Draw(Textures.D, _dText, Color.White);
            SpriteBatch.End();
            _menu.Draw(gameTime);    
        }

        public override void Back()
        {
            if (HasPrevious) NextComponent = PreviousComponent;
        }
    }
}
