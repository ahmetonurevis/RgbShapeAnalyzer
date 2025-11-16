using System.Drawing;

namespace RgbShapeAnalyzer
{
    public class ShapeInfo
    {
        public string ShapeName { get; set; } = "";
        public string ColorText { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }
        public int Area { get; set; }
        public int PixelCount { get; set; }
        public Rectangle Bounds { get; set; }

        // şekli temsil eden 10x10 matris
        public bool[,] Matrix { get; set; }
    }
}
