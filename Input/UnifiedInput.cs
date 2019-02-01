using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SquareGrid.Input
{
    public class UnifiedInput
    {
        public Vector2 Location = Vector2.Zero;
        public bool Action;
        public Vector2 DragFrom = Vector2.Zero;

        public List<Procedure<Vector2>> TapListeners;
        public List<Procedure<Vector2>> MoveListeners;
        public List<Operation<Vector2>> DraggingListeners;
        public List<Operation<Vector2>> DraggedListeners;

        public bool Hidden
        {
            get
            {
                if (Game.Touch.Location == Vector2.Zero && Game.Mouse.Hidden) return true;
                return Action;
            }
            set
            {
                if(value)
                    Game.Touch.Location = Vector2.Zero;
                Game.Mouse.Hidden = value;
            }
        }

        public BaseGame Game;

        public UnifiedInput(BaseGame game)
        {
            Game = game;
            TapListeners = new List<Procedure<Vector2>>();
            MoveListeners = new List<Procedure<Vector2>>();
            DraggingListeners = new List<Operation<Vector2>>();
            DraggedListeners = new List<Operation<Vector2>>();

            Game.Mouse.LeftClickListeners.Add(Tap);
            Game.Mouse.MoveListeners.Add(Move);
            Game.Mouse.DraggedListeners.Add(Dragged);
            Game.Mouse.DraggingListeners.Add(Dragging);
            Game.Touch.TapListeners.Add(Tap);
            Game.Touch.MoveListeners.Add(Move);
            Game.Touch.DraggedListeners.Add(Dragged);
            Game.Touch.DraggingListeners.Add(Dragging);
        }

        private void Dragging(Vector2 a, Vector2 b)
        {
            var remove = new List<Operation<Vector2>>();
            foreach (var draggingListener in DraggingListeners)
            {
                try
                {
                    draggingListener(a, b);
                }
                catch
                {
                    remove.Add(draggingListener);
                }
            }
            DraggingListeners.RemoveAll(remove.Contains);
        }

        private void Dragged(Vector2 a, Vector2 b)
        {
            var remove = new List<Operation<Vector2>>();
            foreach (var draggedListener in DraggedListeners)
            {
                try
                {
                    draggedListener(a, b);
                }
                catch
                {
                    remove.Add(draggedListener);
                }
            }
            DraggedListeners.RemoveAll(remove.Contains);
        }

        private void Move(Vector2 value)
        {
            var remove = new List<Procedure<Vector2>>();
            foreach (var moveListener in MoveListeners)
            {
                try
                {
                    moveListener(value);
                }
                catch
                {
                    remove.Add(moveListener);
                }
            }
            MoveListeners.RemoveAll(remove.Contains);
        }

        private void Tap(Vector2 value)
        {
            var remove = new List<Procedure<Vector2>>();
            foreach (var tapListener in TapListeners)
            {
                try
                {
                    tapListener(value);
                }
                catch
                {
                    remove.Add(tapListener);
                }
            }
            TapListeners.RemoveAll(remove.Contains);
        }

        public void Update(GameTime gameTime)
        {
            Location = (Game.Touch.Location == Vector2.Zero) ? Game.Mouse.Location : Game.Touch.Location;
            DragFrom = (Game.Touch.DragFrom == Vector2.Zero) ? Game.Mouse.DragFrom : Game.Touch.DragFrom;
            Action = (Game.Touch.Location == Vector2.Zero) ? (Game.Mouse.State.LeftButton == ButtonState.Pressed) : Game.Touch.Location != Vector2.Zero;
        }
    }
}
