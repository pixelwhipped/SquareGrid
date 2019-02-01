using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquareGrid.Content;
using SquareGrid.Input;

namespace SquareGrid.UI.Menus
{
    public class Menu
    {
        private readonly MenuItem[] _items;

        private int _index;
        private readonly BaseGame _game;
        private readonly Procedure<MenuItem> _selectEvent;
        private UnifiedInput UnifiedInput { get { return _game.UnifiedInput; } }

        private readonly List<Vector2> _taps;
        public Menu(BaseGame game, Procedure<MenuItem> selectEvent, MenuItem item, params MenuItem[] items)
        {

            item.Selected = true;
            foreach (var menuItem in items)
            {
                menuItem.Selected = false;
            }
            var i = new List<MenuItem> { item };
            i.AddRange(items);
            _items = i.ToArray();
            _game = game;
            _selectEvent = selectEvent;
            _taps = new List<Vector2>();
            UnifiedInput.TapListeners.Add(Tap);
        }

        public void Tap(Vector2 value)
        {
            _taps.Add(value);
        }


        public void Update(GameTime gameTime)
        {
            
            #region Keyboard Control
            if (_game.KeyboardInput.TypedKey(Keys.Down))
            {
                _game.Audio.Play(Cues.Move02);
                _items[_index].Selected = false;
                if (_index + 1 >= _items.Length)
                {
                    _index = 0;
                }
                else
                {
                    _index++;
                }
                _items[_index].Selected = true;
            }
            else if (_game.KeyboardInput.TypedKey(Keys.Up))
            {
                _game.Audio.Play(Cues.Move01);
                _items[_index].Selected = false;
                if (_index - 1 < 0)
                {
                    _index = _items.Length - 1;
                }
                else
                {
                    _index--;
                }
                _items[_index].Selected = true;
            }
            else if (_game.KeyboardInput.TypedKey(Keys.Enter) || _game.KeyboardInput.TypedKey(Keys.Space))
            {
                _game.Audio.Play(Cues.Fail);
                _selectEvent(_items[_index]);
            }
            #endregion

            #region Mouse Touch        
            var mindex = _index;
            var clicked = false;
            for (var index = 0; index < _items.Length; index++)
            {
                if (_items[index].Bounds.Contains(UnifiedInput.Location) && UnifiedInput.Action)
                {
                    mindex = index;
                    clicked = true;
                    break;
                }
                if (_items[index].Bounds.Contains(UnifiedInput.Location) && !UnifiedInput.Hidden)
                {
                    mindex = index;
                    break;
                }

            }
            if(mindex != _index)
            {
                foreach (var menuItem in _items)
                {
                    menuItem.Selected = false;
                }
                _items[mindex].Selected = true;
                _index = mindex;
                _game.Audio.Play(Cues.Move01);
            }

            if (clicked)
            {                    
                _game.Audio.Play(Cues.Fail);
                _selectEvent(_items[_index]);
            }
            

            #endregion 
            foreach (var menuItem in _items)
            {
                menuItem.Update(gameTime);
                if (menuItem.Selected && menuItem.GetType() == typeof (NameMenuItem))
                {
                    if (!_game.KeyboardInput.IsOSKVisable)
                    {
                        _game.KeyboardInput.IsOSKVisable = true;
                    }

                }
                else if (menuItem.Selected && menuItem.GetType() != typeof(NameMenuItem))
                {
                    _game.KeyboardInput.IsOSKVisable = false;
                }
                
            }
        }
        public void Draw(GameTime gameTime)
        {
            foreach (var menuItem in _items)
            {
                menuItem.Draw(gameTime);
            }
        }
    }
}
