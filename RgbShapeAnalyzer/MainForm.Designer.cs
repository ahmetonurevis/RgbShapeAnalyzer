using System.Drawing;
using System.Windows.Forms;

namespace RgbShapeAnalyzer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Designer

        private void InitializeComponent()
        {
            panelTop = new Panel();
            lblTitle = new Label();
            btnLoadImage = new Button();
            btnAnalyze = new Button();
            pictureBoxImage = new PictureBox();
            dgvShapes = new DataGridView();
            lblStatus = new Label();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvShapes).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(30, 144, 255);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1150, 55);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(15, 0, 0, 0);
            lblTitle.Size = new Size(1150, 55);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "RGB Shape Analyzer";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnLoadImage
            // 
            btnLoadImage.BackColor = Color.FromArgb(46, 204, 113);
            btnLoadImage.FlatStyle = FlatStyle.Flat;
            btnLoadImage.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnLoadImage.ForeColor = Color.White;
            btnLoadImage.Location = new Point(680, 70);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(160, 38);
            btnLoadImage.TabIndex = 1;
            btnLoadImage.Text = "Resim Yükle";
            btnLoadImage.UseVisualStyleBackColor = false;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // btnAnalyze
            // 
            btnAnalyze.BackColor = Color.FromArgb(52, 152, 219);
            btnAnalyze.FlatStyle = FlatStyle.Flat;
            btnAnalyze.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnAnalyze.ForeColor = Color.White;
            btnAnalyze.Location = new Point(850, 70);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(180, 38);
            btnAnalyze.TabIndex = 2;
            btnAnalyze.Text = "Şekilleri Analiz Et";
            btnAnalyze.UseVisualStyleBackColor = false;
            btnAnalyze.Click += btnAnalyze_Click;
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.BackColor = Color.FromArgb(40, 40, 40);
            pictureBoxImage.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxImage.Location = new Point(20, 80);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(520, 400);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.TabIndex = 3;
            pictureBoxImage.TabStop = false;
            // 
            // dgvShapes
            // 
            dgvShapes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvShapes.BackgroundColor = Color.FromArgb(45, 45, 48);
            dgvShapes.Location = new Point(560, 120);
            dgvShapes.Name = "dgvShapes";
            dgvShapes.RowHeadersVisible = false;
            dgvShapes.Size = new Size(560, 360);
            dgvShapes.TabIndex = 4;
            // 
            // lblStatus
            // 
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(20, 500);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1100, 23);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Durum: Hazır";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 28, 28);
            ClientSize = new Size(1150, 540);
            Controls.Add(panelTop);
            Controls.Add(btnLoadImage);
            Controls.Add(btnAnalyze);
            Controls.Add(pictureBoxImage);
            Controls.Add(dgvShapes);
            Controls.Add(lblStatus);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RgbShapeAnalyzer";
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvShapes).EndInit();
            ResumeLayout(false);
        }

        private Panel panelTop;
        private Label lblTitle;
        private Button btnLoadImage;
        private Button btnAnalyze;
        private PictureBox pictureBoxImage;
        private DataGridView dgvShapes;
        private Label lblStatus;

        #endregion
    }
}
