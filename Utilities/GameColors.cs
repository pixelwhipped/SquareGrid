using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SquareGrid.Common;

namespace SquareGrid.Utilities
{
    public struct GameColor
    {
        public Color Color;
        public string Name;
    }
    public static class GameColors
    {

        public static GameColor[] Colors =
        {
            new GameColor{Color = Color.BlueViolet, Name = Strings.ColorBlueViolet},  
            new GameColor{Color = Color.Chartreuse, Name = Strings.ColorChartreuse},  
            new GameColor{Color = Color.Crimson, Name = Strings.ColorCrimson},         
            new GameColor{Color = Color.Orange, Name = Strings.ColorOrange},        
            new GameColor{Color = Color.SlateBlue, Name = Strings.ColorSlateBlue},   
            new GameColor{Color = Color.SpringGreen, Name = Strings.ColorSpringGreen}, 
            new GameColor{Color = Color.MediumTurquoise, Name = Strings.ColorMediumTurquoise},
            new GameColor{Color = Color.DarkSlateBlue, Name = Strings.ColorDarkSlateBlue}
        };
    }
}
