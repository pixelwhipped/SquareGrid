
using System;
using Microsoft.Xna.Framework;
using SquareGrid.Common;
using SquareGrid.Interfaces;
using SquareGrid.UI;
using SquareGrid.UI.Menus;
using SquareGrid.UI.Square_Grid;


namespace SquareGrid.States
{
    public class GameSettings : Component
    {
        private readonly Menu _menu;

        private readonly CheckedMenuItem _enableEffects;
        private readonly ScaleMenuItem _effectVolume;
        private readonly ScaleMenuItem _musicVolume;
        
        private const string MaxPercent = " 100%";

        public GameSettings(BaseGame grid, IComponent parent)
            : base(grid, parent)
        {
            _enableEffects = new CheckedMenuItem(grid,
                                                    grid.Font.MeasureString(new Vector2(10, 100), Strings.EffectsUc), () => Game.Audio.EnableAudio,
                                                                                                                p => { Game.Audio.EnableAudio = p; },
                                                    Strings.EffectsUc);
            _effectVolume = new ScaleMenuItem(grid,
                grid.Font.MeasureString(new Vector2(10, _enableEffects.Bounds.Y + _enableEffects.Bounds.Height), Strings.EffectVolume + MaxPercent), () => Game.Audio.EffectVolume,
                                                                                                                p => { Game.Audio.EffectVolume = p; }, Strings.EffectVolume);
            _musicVolume = new ScaleMenuItem(grid,
                grid.Font.MeasureString(new Vector2(10, _effectVolume.Bounds.Y + _effectVolume.Bounds.Height), Strings.MusicVolume + MaxPercent), () => Game.Audio.MusicVolume,
                                                                                                                p => { Game.Audio.MusicVolume = p; }, Strings.MusicVolume);

            MenuItem backItem = new MenuItem(grid,
                grid.Font.MeasureString(new Vector2(10, _musicVolume.Bounds.Y + _musicVolume.Bounds.Height), Strings.Back),
                Strings.Back);
            _menu = new Menu(grid, HandleMenuSelect,
                            _enableEffects,
                            _effectVolume,
                            _musicVolume,
                            backItem
                );           

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
            try
            {
                _menu.Draw(gameTime);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void HandleMenuSelect(MenuItem item)
        {
            if(item.Name==Strings.Back)Back();       
        }
    }
}
