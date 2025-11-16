using System;

namespace RgbShapeAnalyzer
{
    public static class ShapeTemplates
    {
        public const int GridSize = 10;

        public static bool[,] Square = BuildSquare();
        public static bool[,] Circle = BuildCircle();
        public static bool[,] Triangle = BuildTriangle();
        public static bool[,] Bar = BuildBar();

        private static bool[,] BuildSquare()
        {
            int n = GridSize;
            bool[,] m = new bool[n, n];

            for (int y = 2; y < n - 2; y++)
                for (int x = 2; x < n - 2; x++)
                    m[x, y] = true;

            return m;
        }

        private static bool[,] BuildCircle()
        {
            int n = GridSize;
            bool[,] m = new bool[n, n];

            double cx = (n - 1) / 2.0;
            double cy = (n - 1) / 2.0;
            double r = n * 0.35;

            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                    if (Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy)) <= r)
                        m[x, y] = true;

            return m;
        }

        private static bool[,] BuildTriangle()
        {
            int n = GridSize;
            bool[,] m = new bool[n, n];

            for (int y = 3; y < n - 1; y++)
            {
                double ratio = (double)(y - 3) / (n - 4);
                int half = (int)(ratio * (n / 2.0));
                int cx = n / 2;

                for (int x = cx - half; x <= cx + half; x++)
                    if (x >= 0 && x < n)
                        m[x, y] = true;
            }

            return m;
        }

        private static bool[,] BuildBar()
        {
            int n = GridSize;
            bool[,] m = new bool[n, n];

            int barW = 2;
            int cx = n / 2;

            for (int y = 1; y < n - 1; y++)
            {
                for (int x = cx - barW; x <= cx + barW; x++)
                {
                    if (x >= 0 && x < n)
                        m[x, y] = true;
                }
            }

            return m;
        }

        public static double Compare(bool[,] a, bool[,] b)
        {
            int n = GridSize;
            int same = 0;

            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                    if (a[x, y] == b[x, y])
                        same++;

            return (double)same / (n * n);
        }
    }
}
