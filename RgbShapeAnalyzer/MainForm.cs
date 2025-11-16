using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RgbShapeAnalyzer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // -------------------------------------------------------------------
        // resim yükle
        // -------------------------------------------------------------------
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resimler|*.bmp;*.png;*.jpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxImage.Image = new Bitmap(ofd.FileName);
                lblStatus.Text = "durum: görüntü yüklendi.";
            }
        }

        // -------------------------------------------------------------------
        // analiz başlat
        // -------------------------------------------------------------------
        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (pictureBoxImage.Image == null)
            {
                MessageBox.Show("lütfen önce bir görüntü yükleyin.");
                return;
            }

            Bitmap original = new Bitmap(pictureBoxImage.Image);

            // şekilleri tespit et
            List<ShapeInfo> shapes = DetectShapes(original);

            // gösterim için çizim
            Bitmap display = new Bitmap(original);
            using Graphics g = Graphics.FromImage(display);

            foreach (var shape in shapes)
            {
                // sınır kutusu
                using (Pen p = new Pen(Color.White, 3))
                    g.DrawRectangle(p, shape.Bounds);

                // yazı içeriği
                string text =
                    $"{shape.ShapeName}\n" +
                    $"renk: {shape.ColorText}\n" +
                    $"w:{shape.Width} h:{shape.Height}\n" +
                    $"alan:{shape.Area} px";

                // okunabilir yazı kutusu
                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                {
                    SizeF size = g.MeasureString(text, font);
                    RectangleF box = new RectangleF(
                        shape.Bounds.Right + 8,
                        shape.Bounds.Top + 8,
                        size.Width + 12,
                        size.Height + 12);

                    using (Brush b = new SolidBrush(Color.FromArgb(230, 255, 255, 255)))
                        g.FillRectangle(b, box);

                    using (Pen border = new Pen(Color.Black, 2))
                        g.DrawRectangle(border, box.X, box.Y, box.Width, box.Height);

                    // yazı gölge
                    using (Brush shadow = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
                        g.DrawString(text, font, shadow, new PointF(box.X + 4, box.Y + 4));

                    // yazı
                    using (Brush txt = new SolidBrush(Color.Black))
                        g.DrawString(text, font, txt, new PointF(box.X + 2, box.Y + 2));
                }

                // grid çizimi (10x10)
                int N = ShapeTemplates.GridSize;
                int cellW = shape.Bounds.Width / N;
                int cellH = shape.Bounds.Height / N;

                for (int iy = 0; iy < N; iy++)
                {
                    for (int ix = 0; ix < N; ix++)
                    {
                        Rectangle cell = new Rectangle(
                            shape.Bounds.Left + ix * cellW,
                            shape.Bounds.Top + iy * cellH,
                            cellW, cellH);

                        if (shape.Matrix[ix, iy])
                        {
                            using (Brush fill = new SolidBrush(Color.FromArgb(140, Color.Red)))
                                g.FillRectangle(fill, cell);
                        }

                        using (Pen pen = new Pen(Color.Black, 1))
                            g.DrawRectangle(pen, cell);
                    }
                }
            }

            pictureBoxImage.Image = display;

            dgvShapes.DataSource = shapes.Select(s => new
            {
                Şekil = s.ShapeName,
                Renk = s.ColorText,
                Genişlik = s.Width,
                Yükseklik = s.Height,
                Alan = s.Area,
                Piksel = s.PixelCount,
                Sınır = s.Bounds.ToString()
            }).ToList();

            lblStatus.Text = $"{shapes.Count} şekil bulundu.";
        }

        // -------------------------------------------------------------------
        // şekilleri bul
        // -------------------------------------------------------------------
        private List<ShapeInfo> DetectShapes(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            Color bg = bmp.GetPixel(0, 0);
            bool[,] visited = new bool[w, h];

            List<ShapeInfo> shapes = new();

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (visited[x, y]) continue;

                    Color px = bmp.GetPixel(x, y);

                    // GRID çizgilerini ignore et (mor-kırmızı çizgi), çok kritik
                    if (px.R < 160 && px.G < 80 && px.B < 140)
                    {
                        visited[x, y] = true;
                        continue;
                    }

                    // arka plan toleransı
                    if (IsSimilarColor(px, bg))
                    {
                        visited[x, y] = true;
                        continue;
                    }

                    // flood fill ile bölgeyi bul
                    List<Point> region = FloodFill(bmp, visited, x, y, bg);

                    if (region.Count < 300)
                        continue;

                    int minX = region.Min(p => p.X);
                    int maxX = region.Max(p => p.X);
                    int minY = region.Min(p => p.Y);
                    int maxY = region.Max(p => p.Y);

                    Rectangle bounds = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);

                    // ortalama renk
                    int rr = 0, gg = 0, bb = 0;
                    foreach (var p in region)
                    {
                        Color c = bmp.GetPixel(p.X, p.Y);
                        rr += c.R; gg += c.G; bb += c.B;
                    }

                    Color avg = Color.FromArgb(rr / region.Count, gg / region.Count, bb / region.Count);

                    bool[,] matrix = BuildMatrix(region, bounds);

                    double sq = ShapeTemplates.Compare(matrix, ShapeTemplates.Square);
                    double ci = ShapeTemplates.Compare(matrix, ShapeTemplates.Circle);
                    double tr = ShapeTemplates.Compare(matrix, ShapeTemplates.Triangle);
                    double br = ShapeTemplates.Compare(matrix, ShapeTemplates.Bar);

                    double max = new[] { sq, ci, tr, br }.Max();

                    string label =
                        max == sq ? "kare" :
                        max == ci ? "daire" :
                        max == tr ? "üçgen" : "çubuk";

                    shapes.Add(new ShapeInfo
                    {
                        ShapeName = label,
                        ColorText = $"rgb({avg.R},{avg.G},{avg.B})",
                        Width = bounds.Width,
                        Height = bounds.Height,
                        Area = region.Count,
                        PixelCount = region.Count,
                        Bounds = bounds,
                        Matrix = matrix
                    });
                }
            }

            return shapes;
        }

        // -------------------------------------------------------------------
        // flood fill
        // -------------------------------------------------------------------
        private List<Point> FloodFill(Bitmap bmp, bool[,] visited, int sx, int sy, Color bg)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            List<Point> region = new();
            Queue<Point> q = new();

            Color start = bmp.GetPixel(sx, sy);

            visited[sx, sy] = true;
            q.Enqueue(new Point(sx, sy));

            while (q.Count > 0)
            {
                Point p = q.Dequeue();
                region.Add(p);

                int[,] dirs = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

                for (int i = 0; i < 4; i++)
                {
                    int nx = p.X + dirs[i, 0];
                    int ny = p.Y + dirs[i, 1];

                    if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;
                    if (visited[nx, ny]) continue;

                    Color px = bmp.GetPixel(nx, ny);

                    // grid çizgisi → yok say
                    if (px.R < 160 && px.G < 80 && px.B < 140)
                    {
                        visited[nx, ny] = true;
                        continue;
                    }

                    // arka plan → yok say
                    if (IsSimilarColor(px, bg))
                    {
                        visited[nx, ny] = true;
                        continue;
                    }

                    // shape ile benzer → ekle
                    if (IsSimilarColor(px, start, 70))
                    {
                        visited[nx, ny] = true;
                        q.Enqueue(new Point(nx, ny));
                    }
                    else visited[nx, ny] = true;
                }
            }

            return region;
        }

        // -------------------------------------------------------------------
        // arka plan renk benzerlik kontrolü
        // -------------------------------------------------------------------
        private bool IsSimilarColor(Color a, Color b, int tol = 60)
        {
            int d =
                Math.Abs(a.R - b.R) +
                Math.Abs(a.G - b.G) +
                Math.Abs(a.B - b.B);

            return d <= tol;
        }

        // -------------------------------------------------------------------
        // 10x10 shape matrix oluştur
        // -------------------------------------------------------------------
        private bool[,] BuildMatrix(List<Point> pixels, Rectangle bounds)
        {
            int N = ShapeTemplates.GridSize;
            bool[,] m = new bool[N, N];

            foreach (var p in pixels)
            {
                int gx = (int)((p.X - bounds.Left) * (N / (double)bounds.Width));
                int gy = (int)((p.Y - bounds.Top) * (N / (double)bounds.Height));

                gx = Math.Clamp(gx, 0, N - 1);
                gy = Math.Clamp(gy, 0, N - 1);

                m[gx, gy] = true;
            }

            return m;
        }
    }
}
