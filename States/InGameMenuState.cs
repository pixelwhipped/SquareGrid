using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SquareGrid.Common;
using SquareGrid.Interfaces;
using SquareGrid.UI.Menus;
using SquareGrid.UI.Square_Grid;

namespace SquareGrid.States
{
    public class InGameMenuState : Component
    {
        private readonly Menu _menu;
        private readonly MenuItem _settingsItem;
        private readonly MenuItem _continue;
        private readonly MenuItem _exitItem;
        public InGameMenuState(BaseGame game, IComponent parent)
            : base(game,parent)
        {            

            _settingsItem = new MenuItem(game,
                                    game.Font.MeasureString(
                                        new Vector2(10, 100),
                                        Strings.Settings), Strings.Settings);
            _continue = new MenuItem(game,
                                    game.Font.MeasureString(
                                        new Vector2(10, _settingsItem.Bounds.Y + _settingsItem.Bounds.Height),
                                        Strings.Continue), Strings.Continue);
            _exitItem = new MenuItem(game,
                        game.Font.MeasureString(
                            new Vector2(10, _continue.Bounds.Y + _continue.Bounds.Height),
                            Strings.Exit), Strings.Exit);
            _menu = new Menu(game, HandleMenuSelect,
                            _settingsItem,
                            _continue,
                            _exitItem
                );
        }

        public void HandleMenuSelect(MenuItem item)
        {
            switch (item.Name)
            {
                case Strings.Settings:
                    {
                        NextComponent = new GameSettings(Game, this);
                        break;
                    }
                case Strings.Continue:
                    {
                        Back();
                        break;
                    }
                default:
                    {
                        NextComponent = new MainMenu(Game,null);
                        break;
                    }
            }
        }
        public override IComponent Update(GameTime gameTime, BaseGame game)
        {
            _menu.Update(gameTime);
            return NextComponent ?? this;
        }

        public override void Draw(GameTime gameTime, BaseGame game)
        {
            _menu.Draw(gameTime);
        }
    }
}
