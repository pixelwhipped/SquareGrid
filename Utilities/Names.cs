using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGrid.Utilities
{
    public static class Names
    {
        private static readonly string[] RandomNames =
        {
            "Ava", "Mia", "Zoe", "Eva", "Mya", "Amy", "Ana", "Zion", "Andy", "Zane", "Ezra",
            "Drew", "Troy", "Emma", "Ella", "Lily", "Leah", "Anna", "Zoey", "Maya", "Lucy",
            "Mary", "Ruby", "Jade", "Sara", "Jada", "Lyla", "Lila", "Nora", "Eden", "Kate",
            "Lola", "Erin", "Maci", "Lexi", "Hope", "Lukas", "Tyson", "Shawn", "Jared",
            "Emily", "Chloe", "Grace", "Avery", "Sofia", "Sarah", "Layla", "Riley", "Khloe",
            "Bella", "Kayla", "Alexa", "Julia", "Kylie", "Faith", "Maria", "Molly", "Naomi",
            "Paige", "Rylee", "Ellie", "Lilly", "Lydia", "Sadie", "Megan", "Jayla", "Reese",
            "Laila", "Kylee", "Jenna", "Piper", "Daisy", "Haley", "Katie", "Elena", "Keira",
            "Clara", "Alana", "Alice", "Allie", "Diana", "Angel", "Elise", "Leila", "Ariel",
            "Miley", "Carly", "Tessa", "Amber", "Amaya", "Jayda", "Eliza", "Karen", "Nadia",
            "Sergio", "Keegan", "Chance", "Corbin", "Sophia", "Olivia", "Alexis", "Hailey",
            "Alyssa", "Hannah", "Nevaeh", "Ashley", "Kaylee", "Taylor", "Evelyn", "Amelia",
            "Aubrey", "Peyton", "Audrey", "Claire", "Lauren", "Sophie", "Sydney", "Camila",
            "Morgan", "Gianna", "Brooke", "Bailey", "Payton", "Andrea", "Autumn", "Ariana",
            "Stella", "Mariah", "Rachel", "London", "Jordyn", "Isabel", "Harper", "Nicole",
            "Violet", "Reagan", "Gracie", "Aliyah", "Ashlyn", "Vivian", "Angela", "Hayden",
            "Summer", "Eliana", "Alivia", "Aniyah", "Jordan", "Skylar", "Briana", "Shelby",
            "Amanda", "Leslie", "Melody", "Aurora", "Kendra", "Alaina", "Sienna", "Jayden",
            "Sierra", "Hadley", "Alicia", "Aubree", "Maggie", "Kinley", "Kelsey", "Marley",
            "Alayna", "Callie", "Alexia", "Teagan"
        };

        public static string GetName()
        {
            //Todo Test _names[_names.Lenght]
            return RandomNames[BaseGame.Random.Next(RandomNames.Length-1)].ToUpperInvariant();
        }
    }
}
