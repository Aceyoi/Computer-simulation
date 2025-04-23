using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp16
{
    public partial class Form1 : Form
    {
        // ��������� ������ (������ ����������)
        private double particleMass = 1.0; // ����� ������� (��)
        private double particleCharge = 1e-6; // ����� ������� (��)
        private double electricField = 100; // ������������� ���� (�/�)
        private double initialVelocity = 1.0; // ��������� �������� (�/�)

        // ��������� ��������
        private const double TimeStep = 0.01; // ��� �� ������� (�)
        private double currentTime = 0;

        // ��������� � �������� �������
        private double x, y; // ��������� ������� (�)
        private double vx, vy; // �������� ������� (�/�)

        // ������� ������� �������������
        private const double WorldWidth = 2.0; // ������ ������� (�)
        private const double WorldHeight = 1.0; // ������ ������� (�)

        // ����������� ��������
        private Bitmap canvas;
        private Graphics graphics;
        private System.Windows.Forms.Timer animationTimer;

        public Form1()
        {
            InitializeComponent();

            // ������������� �������
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(canvas);
            pictureBox.Image = canvas;

            // ��������� ������� ��������
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 20; // 20 �� (~50 ������ � �������)
            animationTimer.Tick += AnimationTimer_Tick;

            // ������������� �������� NumericUpDown
            numMass.Value = (decimal)particleMass;
            numCharge.Value = (decimal)(particleCharge * 1e6); // ���������� � ����
            numField.Value = (decimal)electricField;
            numVelocity.Value = (decimal)initialVelocity;

            // ������������� ���������� �������
            InitializeParticle();
        }

        private void InitializeParticle()
        {
            // ��������� ��������� ������� (� �������� ����� ����������)
            x = 0.1;
            y = WorldHeight / 2;

            // ��������� �������� (����������� ���������)
            vx = initialVelocity;
            vy = 0;

            currentTime = 0;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // ���������� ���������� ������
            UpdatePhysics();

            // ��������� �����
            DrawScene();
        }

        private void UpdatePhysics()
        {
            // ����, ����������� �� ������� (F = q * E)
            // ����������� ���� ������� �� ����� ������ � ����������� ����
            double fx = 0;
            double fy = particleCharge * electricField; // ���� ���������� ����� ����

            // ��������� (a = F / m)
            double ax = fx / particleMass;
            double ay = fy / particleMass;

            // ���������� �������� (v = v0 + a * dt)
            vx += ax * TimeStep;
            vy += ay * TimeStep;

            // ���������� ��������� (x = x0 + v * dt)
            x += vx * TimeStep;
            y += vy * TimeStep;

            currentTime += TimeStep;

            // �������� �� ����� �� �������
            if (x < 0 || x > WorldWidth || y < 0 || y > WorldHeight)
            {
                InitializeParticle();
            }
        }

        private void DrawScene()
        {
            // ������� ������
            graphics.Clear(Color.White);

            // ������� ��� �������� �� ������� ��������� � ��������
            float scaleX = (float)(pictureBox.Width / WorldWidth);
            float scaleY = (float)(pictureBox.Height / WorldHeight);

            // ������ �������� ������������
            float plateWidth = 10;
            float topPlateY = 0.2f * pictureBox.Height;
            float bottomPlateY = 0.8f * pictureBox.Height;

            graphics.FillRectangle(Brushes.Gray, 0, topPlateY - plateWidth / 2,
                                 pictureBox.Width, plateWidth);
            graphics.FillRectangle(Brushes.Gray, 0, bottomPlateY - plateWidth / 2,
                                 pictureBox.Width, plateWidth);

            // ������ �������
            float particleSize = 8;
            float screenX = (float)x * scaleX;
            float screenY = (float)y * scaleY;

            graphics.FillEllipse(Brushes.Red, screenX - particleSize / 2,
                                screenY - particleSize / 2, particleSize, particleSize);

            // ������� ����������
            string info = $"�����: {currentTime:F2} �\n" +
                         $"���������: ({x:F2}, {y:F2}) �\n" +
                         $"��������: ({vx:F2}, {vy:F2}) �/�\n" +
                         $"���������:\n" +
                         $"  �����: {particleMass:F3} ��\n" +
                         $"  �����: {particleCharge * 1e6:F2} ����\n" +
                         $"  ����: {electricField} �/�\n" +
                         $"  ���. ��������: {initialVelocity} �/�";

            graphics.DrawString(info, SystemFonts.DefaultFont, Brushes.Black, 10, 10);

            // ��������� pictureBox
            pictureBox.Invalidate();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            animationTimer.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            animationTimer.Stop();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            animationTimer.Stop();
            InitializeParticle();
            DrawScene();
        }

        private void numMass_ValueChanged(object sender, EventArgs e)
        {
            particleMass = (double)numMass.Value;
        }

        private void numCharge_ValueChanged(object sender, EventArgs e)
        {
            particleCharge = (double)numCharge.Value * 1e-6; // ����������� ���� � ��
        }

        private void numField_ValueChanged(object sender, EventArgs e)
        {
            electricField = (double)numField.Value;
        }

        private void numVelocity_ValueChanged(object sender, EventArgs e)
        {
            initialVelocity = (double)numVelocity.Value;
            // ��������� ��������� �������� ��� ��������� ���������
            if (!animationTimer.Enabled)
            {
                vx = initialVelocity;
                vy = 0;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // ��������� ��������� �� ��������� ����������
            particleMass = (double)numMass.Value;
            particleCharge = (double)numCharge.Value * 1e-6; // ����������� ���� � ��
            electricField = (double)numField.Value;
            initialVelocity = (double)numVelocity.Value;

            // ������������� �������� � ������ �����������
            animationTimer.Stop();
            InitializeParticle();
            DrawScene(); // ���� �������� �����������
        }
    }
}