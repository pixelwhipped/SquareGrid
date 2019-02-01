using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareGrid.Input;
using SquareGrid.UI;

namespace SquareGrid.Interfaces
{
    public interface IComponent
    {
        KeyboardInput KeyboardInput { get; }
        UnifiedInput UnifiedInput { get; }
        Vector2 Center { get; }
        float Width { get; }
        float Height { get; }
        BaseGame Game { get; }
        SpriteBatch SpriteBatch { get; }
        Settings Settings { get; }

        GamePersistance<GameData> GameData { get; }        
        bool IsVisible { get; }
        float Transition { get; set; }
        IComponent Update(GameTime gameTime, BaseGame game);
        void Draw(GameTime gameTime, BaseGame game);

        bool HasPrevious { get; }
        IComponent NextComponent { get; set; }
        void Back();
    }
}
