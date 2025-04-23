using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp16
{
    public partial class Form1 : Form
    {
        // Параметры модели (теперь изменяемые)
        private double particleMass = 1.0; // масса частицы (кг)
        private double particleCharge = 1e-6; // заряд частицы (Кл)
        private double electricField = 100; // напряженность поля (В/м)
        private double initialVelocity = 1.0; // начальная скорость (м/с)

        // Параметры анимации
        private const double TimeStep = 0.01; // шаг по времени (с)
        private double currentTime = 0;

        // Положение и скорость частицы
        private double x, y; // положение частицы (м)
        private double vx, vy; // скорость частицы (м/с)

        // Размеры области моделирования
        private const double WorldWidth = 2.0; // ширина области (м)
        private const double WorldHeight = 1.0; // высота области (м)

        // Графические элементы
        private Bitmap canvas;
        private Graphics graphics;
        private System.Windows.Forms.Timer animationTimer;

        public Form1()
        {
            InitializeComponent();

            // Инициализация графики
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(canvas);
            pictureBox.Image = canvas;

            // Настройка таймера анимации
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 20; // 20 мс (~50 кадров в секунду)
            animationTimer.Tick += AnimationTimer_Tick;

            // Инициализация значений NumericUpDown
            numMass.Value = (decimal)particleMass;
            numCharge.Value = (decimal)(particleCharge * 1e6); // показываем в мкКл
            numField.Value = (decimal)electricField;
            numVelocity.Value = (decimal)initialVelocity;

            // Инициализация параметров частицы
            InitializeParticle();
        }

        private void InitializeParticle()
        {
            // Начальное положение частицы (в середине между пластинами)
            x = 0.1;
            y = WorldHeight / 2;

            // Начальная скорость (параллельно пластинам)
            vx = initialVelocity;
            vy = 0;

            currentTime = 0;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Обновление физической модели
            UpdatePhysics();

            // Отрисовка сцены
            DrawScene();
        }

        private void UpdatePhysics()
        {
            // Сила, действующая на частицу (F = q * E)
            // Направление силы зависит от знака заряда и направления поля
            double fx = 0;
            double fy = particleCharge * electricField; // Сила направлена вдоль поля

            // Ускорение (a = F / m)
            double ax = fx / particleMass;
            double ay = fy / particleMass;

            // Обновление скорости (v = v0 + a * dt)
            vx += ax * TimeStep;
            vy += ay * TimeStep;

            // Обновление положения (x = x0 + v * dt)
            x += vx * TimeStep;
            y += vy * TimeStep;

            currentTime += TimeStep;

            // Проверка на выход за границы
            if (x < 0 || x > WorldWidth || y < 0 || y > WorldHeight)
            {
                InitializeParticle();
            }
        }

        private void DrawScene()
        {
            // Очистка холста
            graphics.Clear(Color.White);

            // Масштаб для перевода из мировых координат в экранные
            float scaleX = (float)(pictureBox.Width / WorldWidth);
            float scaleY = (float)(pictureBox.Height / WorldHeight);

            // Рисуем пластины конденсатора
            float plateWidth = 10;
            float topPlateY = 0.2f * pictureBox.Height;
            float bottomPlateY = 0.8f * pictureBox.Height;

            graphics.FillRectangle(Brushes.Gray, 0, topPlateY - plateWidth / 2,
                                 pictureBox.Width, plateWidth);
            graphics.FillRectangle(Brushes.Gray, 0, bottomPlateY - plateWidth / 2,
                                 pictureBox.Width, plateWidth);

            // Рисуем частицу
            float particleSize = 8;
            float screenX = (float)x * scaleX;
            float screenY = (float)y * scaleY;

            graphics.FillEllipse(Brushes.Red, screenX - particleSize / 2,
                                screenY - particleSize / 2, particleSize, particleSize);

            // Выводим информацию
            string info = $"Время: {currentTime:F2} с\n" +
                         $"Положение: ({x:F2}, {y:F2}) м\n" +
                         $"Скорость: ({vx:F2}, {vy:F2}) м/с\n" +
                         $"Параметры:\n" +
                         $"  Масса: {particleMass:F3} кг\n" +
                         $"  Заряд: {particleCharge * 1e6:F2} мкКл\n" +
                         $"  Поле: {electricField} В/м\n" +
                         $"  Нач. скорость: {initialVelocity} м/с";

            graphics.DrawString(info, SystemFonts.DefaultFont, Brushes.Black, 10, 10);

            // Обновляем pictureBox
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
            particleCharge = (double)numCharge.Value * 1e-6; // преобразуем мкКл в Кл
        }

        private void numField_ValueChanged(object sender, EventArgs e)
        {
            electricField = (double)numField.Value;
        }

        private void numVelocity_ValueChanged(object sender, EventArgs e)
        {
            initialVelocity = (double)numVelocity.Value;
            // Обновляем начальную скорость при изменении параметра
            if (!animationTimer.Enabled)
            {
                vx = initialVelocity;
                vy = 0;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // Обновляем параметры из элементов управления
            particleMass = (double)numMass.Value;
            particleCharge = (double)numCharge.Value * 1e-6; // преобразуем мкКл в Кл
            electricField = (double)numField.Value;
            initialVelocity = (double)numVelocity.Value;

            // Перезапускаем анимацию с новыми параметрами
            animationTimer.Stop();
            InitializeParticle();
            DrawScene(); // Явно вызываем перерисовку
        }
    }
}