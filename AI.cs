using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareGrid.Common;
using SquareGrid.States;

namespace SquareGrid
{
    public static class AI
    {
        public static void AIEasy(IGridTileOwner owner)
        {
            var ai = BaseGame.Random.NextDouble();
            if (ai < .1)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.5)))
                    AIBottomRight(owner);
                return;
            }
            if (ai < .2)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.5)))
                    AIBottomLeft(owner);
                return;
            }
            if (ai < .3)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.5)))
                    AITopLeft(owner);
                return;
            }
            if (ai < .4)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.5)))
                    AITopRight(owner);
                return;
            }
            if (ai < .5)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.5)))
                    AIRandom(owner);
                return;
            }
            if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.5)))
            {
                AIRandom(owner);
            }
            return;
        }

        public static void AINormal(IGridTileOwner owner)
        {
            var ai = BaseGame.Random.NextDouble();
            if (ai < .05)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.9)))
                    AIBottomRight(owner);
                return;
            }
            if (ai < .1)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.9)))
                    AIBottomLeft(owner);
                return;
            }
            if (ai < .15)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.9)))
                    AITopLeft(owner);
                return;
            }
            if (ai < .2)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.9)))
                    AITopRight(owner);
                return;
            }
            if (ai < .35)
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.9)))
                    AIRandom(owner);
                return;
            }
            if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.9)))
            {
                if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.9)))
                    AIRandom(owner);
            }
            return;
        }

        public static void AIHard(IGridTileOwner owner)
        {

            if (!AINextFill(owner, 3, !(BaseGame.Random.NextDouble() < 0.5)))
            {
                if (!AINextFill(owner, 1, !(BaseGame.Random.NextDouble() < 0.5)))
                {
                    var ai = BaseGame.Random.NextDouble();
                    if (ai < .33)
                    {
                        AIEasy(owner);
                        return;
                    }
                    AINormal(owner);
                }
            }
            return;

        }

        public static void AIBottomRight(IGridTileOwner owner)
        {
            for (var y = owner.CurrentTiles.Count() - 1; y >= 0; y--)
            {
                for (var x = owner.CurrentTiles[y].Count() - 1; x >= 0; x--)
                {
                    if (owner.CurrentTiles[y][x].IsClosed) continue;
                    if (AIPlace(owner, x, y, !(BaseGame.Random.NextDouble() < 0.5)))
                        return;
                }
            }
        }

        public static void AIBottomLeft(IGridTileOwner owner)
        {
            for (var y = owner.CurrentTiles.Count() - 1; y >= 0; y--)
            {
                for (var x = 0; x < owner.CurrentTiles[y].Count(); x++)
                {
                    if (owner.CurrentTiles[y][x].IsClosed) continue;
                    if (AIPlace(owner, x, y, !(BaseGame.Random.NextDouble() < 0.5)))
                        return;
                }
            }
        }

        public static void AITopLeft(IGridTileOwner owner)
        {
            for (var y = 0; y < owner.CurrentTiles.Count(); y++)
            {
                for (var x = 0; x < owner.CurrentTiles[y].Count(); x++)
                {
                    if (owner.CurrentTiles[y][x].IsClosed) continue;
                    if (AIPlace(owner, x, y, !(BaseGame.Random.NextDouble() < 0.5)))
                        return;
                }
            }
        }

        public static void AITopRight(IGridTileOwner owner)
        {
            for (var y = 0; y < owner.CurrentTiles.Count(); y++)
            {
                for (var x = owner.CurrentTiles[y].Count() - 1; x >= 0; x--)
                {
                    if (owner.CurrentTiles[y][x].IsClosed) continue;
                    if (AIPlace(owner, x, y, !(BaseGame.Random.NextDouble() < 0.5)))
                        return;
                }
            }
        }

        public static bool IsSafe(IGridTileOwner owner, int x, int y)
        {
            if (x - 1 >= 0)
            {
                var c = (owner.CurrentTiles[y][x - 1].Left ? 1 : 0) +
                            (owner.CurrentTiles[y][x - 1].Right ? 1 : 0) +
                            (owner.CurrentTiles[y][x - 1].Top ? 1 : 0) +
                            (owner.CurrentTiles[y][x - 1].Bottom ? 1 : 0);
                if (c == 3) return false;
            }
            else if (x + 1 <= owner.CurrentTiles.Count() - 1)
            {
                var c = (owner.CurrentTiles[y][x + 1].Left ? 1 : 0) +
                            (owner.CurrentTiles[y][x + 1].Right ? 1 : 0) +
                            (owner.CurrentTiles[y][x + 1].Top ? 1 : 0) +
                            (owner.CurrentTiles[y][x + 1].Bottom ? 1 : 0);
                if (c == 3) return false;
            }
            else if (y - 1 >= 0)
            {
                var c = (owner.CurrentTiles[y - 1][x].Left ? 1 : 0) +
                            (owner.CurrentTiles[y - 1][x].Right ? 1 : 0) +
                            (owner.CurrentTiles[y - 1][x].Top ? 1 : 0) +
                            (owner.CurrentTiles[y - 1][x].Bottom ? 1 : 0);
                if (c == 3) return false;
            }
            else if (y + 1 <= owner.CurrentTiles.Count() - 1)
            {
                var c = (owner.CurrentTiles[y + 1][x].Left ? 1 : 0) +
                            (owner.CurrentTiles[y + 1][x].Right ? 1 : 0) +
                            (owner.CurrentTiles[y + 1][x].Top ? 1 : 0) +
                            (owner.CurrentTiles[y + 1][x].Bottom ? 1 : 0);
                if (c == 3) return false;
            }

            return true;
        }

        public static bool AIPlace(IGridTileOwner owner, int x, int y, bool safe)
        {
            if (safe & !IsSafe(owner, x, y))
            {
                return false;
            }
            if (BaseGame.Random.NextDouble() < .5)
            {
                if (!owner.CurrentTiles[y][x].Left)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = owner.CurrentTiles[y][x].Top,
                        Bottom = owner.CurrentTiles[y][x].Bottom,
                        Left = true,
                        Right = owner.CurrentTiles[y][x].Right,
                        Color = owner.CurrentPlayer.Color
                    };
                    if (x != 0)
                    {
                        owner.CurrentTiles[y][x - 1] = new Tile
                        {
                            Top = owner.CurrentTiles[y][x - 1].Top,
                            Bottom = owner.CurrentTiles[y][x - 1].Bottom,
                            Left = owner.CurrentTiles[y][x - 1].Left,
                            Right = true,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
                if (!owner.CurrentTiles[y][x].Right)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = owner.CurrentTiles[y][x].Top,
                        Bottom = owner.CurrentTiles[y][x].Bottom,
                        Left = owner.CurrentTiles[y][x].Left,
                        Color = owner.CurrentPlayer.Color,
                        Right = true
                    };
                    if (x + 1 < owner.GameData.Data.GameGridsVsMode[owner.GameType.Grid].XTiles)
                    {
                        owner.CurrentTiles[y][x + 1] = new Tile
                        {
                            Top = owner.CurrentTiles[y][x + 1].Top,
                            Bottom = owner.CurrentTiles[y][x + 1].Bottom,
                            Left = true,
                            Right = owner.CurrentTiles[y][x + 1].Right,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
                if (!owner.CurrentTiles[y][x].Top)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = true,
                        Bottom = owner.CurrentTiles[y][x].Bottom,
                        Left = owner.CurrentTiles[y][x].Left,
                        Right = owner.CurrentTiles[y][x].Right,
                        Color = owner.CurrentPlayer.Color

                    };
                    if (y != 0)
                    {
                        owner.CurrentTiles[y - 1][x] = new Tile
                        {
                            Top = owner.CurrentTiles[y - 1][x].Top,
                            Bottom = true,
                            Left = owner.CurrentTiles[y - 1][x].Left,
                            Right = owner.CurrentTiles[y - 1][x].Right,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
                if (!owner.CurrentTiles[y][x].Bottom)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = owner.CurrentTiles[y][x].Top,
                        Bottom = true,
                        Left = owner.CurrentTiles[y][x].Left,
                        Right = owner.CurrentTiles[y][x].Right,
                        Color = owner.CurrentPlayer.Color
                    };
                    if (y + 1 < owner.GameData.Data.GameGridsVsMode[owner.GameType.Grid].YTiles)
                    {
                        owner.CurrentTiles[y + 1][x] = new Tile
                        {
                            Top = true,
                            Bottom = owner.CurrentTiles[y + 1][x].Bottom,
                            Left = owner.CurrentTiles[y + 1][x].Left,
                            Right = owner.CurrentTiles[y + 1][x].Right,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
            }
            else
            {
                if (!owner.CurrentTiles[y][x].Top)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = true,
                        Bottom = owner.CurrentTiles[y][x].Bottom,
                        Left = owner.CurrentTiles[y][x].Left,
                        Right = owner.CurrentTiles[y][x].Right,
                        Color = owner.CurrentPlayer.Color

                    };
                    if (y != 0)
                    {
                        owner.CurrentTiles[y - 1][x] = new Tile
                        {
                            Top = owner.CurrentTiles[y - 1][x].Top,
                            Bottom = true,
                            Left = owner.CurrentTiles[y - 1][x].Left,
                            Right = owner.CurrentTiles[y - 1][x].Right,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
                if (!owner.CurrentTiles[y][x].Bottom)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = owner.CurrentTiles[y][x].Top,
                        Bottom = true,
                        Left = owner.CurrentTiles[y][x].Left,
                        Right = owner.CurrentTiles[y][x].Right,
                        Color = owner.CurrentPlayer.Color
                    };
                    if (y + 1 < owner.GameData.Data.GameGridsVsMode[owner.GameType.Grid].YTiles)
                    {
                        owner.CurrentTiles[y + 1][x] = new Tile
                        {
                            Top = true,
                            Bottom = owner.CurrentTiles[y + 1][x].Bottom,
                            Left = owner.CurrentTiles[y + 1][x].Left,
                            Right = owner.CurrentTiles[y + 1][x].Right,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
                if (!owner.CurrentTiles[y][x].Left)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = owner.CurrentTiles[y][x].Top,
                        Bottom = owner.CurrentTiles[y][x].Bottom,
                        Left = true,
                        Right = owner.CurrentTiles[y][x].Right,
                        Color = owner.CurrentPlayer.Color
                    };
                    if (x != 0)
                    {
                        owner.CurrentTiles[y][x - 1] = new Tile
                        {
                            Top = owner.CurrentTiles[y][x - 1].Top,
                            Bottom = owner.CurrentTiles[y][x - 1].Bottom,
                            Left = owner.CurrentTiles[y][x - 1].Left,
                            Right = true,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
                if (!owner.CurrentTiles[y][x].Right)
                {
                    owner.CurrentTiles[y][x] = new Tile
                    {
                        Top = owner.CurrentTiles[y][x].Top,
                        Bottom = owner.CurrentTiles[y][x].Bottom,
                        Left = owner.CurrentTiles[y][x].Left,
                        Color = owner.CurrentPlayer.Color,
                        Right = true
                    };
                    if (x + 1 < owner.GameData.Data.GameGridsVsMode[owner.GameType.Grid].XTiles)
                    {
                        owner.CurrentTiles[y][x + 1] = new Tile
                        {
                            Top = owner.CurrentTiles[y][x + 1].Top,
                            Bottom = owner.CurrentTiles[y][x + 1].Bottom,
                            Left = true,
                            Right = owner.CurrentTiles[y][x + 1].Right,
                            Color = owner.CurrentPlayer.Color
                        };
                    }
                    return true;
                }
            }
            return false;
        }

        public static void AIRandom(IGridTileOwner owner)
        {
            var found = false;
            var max = 0;
            do
            {
                var y = BaseGame.Random.Next(owner.CurrentTiles.Count());
                var x = BaseGame.Random.Next(owner.CurrentTiles[y].Count());
                max++;
                if (!owner.CurrentTiles[y][x].IsClosed)
                {
                    found = true;
                    AIPlace(owner, x, y, !(BaseGame.Random.NextDouble() < 0.5));

                }
                if (found || max < 100) continue;
                found = true;
                AITopLeft(owner);
            } while (!found);
        }

        public static bool AINextFill(IGridTileOwner owner, int max, bool safe)
        {
            for (var y = 0; y < owner.CurrentTiles.Count(); y++)
            {
                for (var x = 0; x < owner.CurrentTiles[y].Count(); x++)
                {
                    if (owner.CurrentTiles[y][x].IsClosed) continue;
                    var c = (owner.CurrentTiles[y][x].Left ? 1 : 0) +
                            (owner.CurrentTiles[y][x].Right ? 1 : 0) +
                            (owner.CurrentTiles[y][x].Top ? 1 : 0) +
                            (owner.CurrentTiles[y][x].Bottom ? 1 : 0);
                    if (c == max)
                    {
                        return AIPlace(owner, x, y, safe);
                    }
                }
            }
            return false;
        }
    }
}
