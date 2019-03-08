using System.Collections.Generic;
using System.Linq;

namespace GetMeX.Models
{
    public class ColorIdHex
    {
        private static Dictionary<byte, string> ColorDict = new Dictionary<byte, string>()
        {
            [1] = "#a4bdfc", // default color
            [2] = "#7ae7bf",
            [3] = "#dbadff",
            [4] = "#ff887c",
            [5] = "#fbd75b",
            [6] = "#ffb878",
            [7] = "#46d6db",
            [8] = "#e1e1e1",
            [9] = "#5484ed",
            [10] = "#51b749",
            [11] = "#dc2127",
        };

        public string ColorIdToHex(byte id)
        {
            return ColorDict.ContainsKey(id) ? ColorDict[id] : "#a4bdfc";
        }

        public byte ColorToId(string color)
        {
            return ColorDict.FirstOrDefault(c => c.Value == color).Key;
        }

        public List<string> GetColors()
        {
            return ColorDict.Values.ToList();
        }
    }
}
