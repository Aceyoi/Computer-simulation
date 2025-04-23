using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp16
{
    public partial class Form1 : Form
    {
        // Константы и параметры модели
        private const int N = 70; // Количество узлов
        private const float Tp = 1.0f; // Температура плавления
        private const float h = 1.0f; // Шаг по пространству
        private const float dt = 0.01f; // Шаг по времени
        private const float Qpl = 10.0f; // Теплота плавления
        private const int Scale = 8; // Масштаб для отображения

        // Массивы данных
        private float[] T = new float[N + 2]; // Температура
        private float[] TNext = new float[N + 2]; // Температура на следующем шаге
        private float[] K = new float[N + 2]; // Коэффициенты теплопроводности
        private float dQ = 0; // Накопленная энергия для плавления
        private int j = 1; // Положение границы фаз

        // Таймер для анимации
        private System.Windows.Forms.Timer simulationTimer;
        // Bitmap для двойной буферизации
        private Bitmap drawBuffer;


        public Form1()
        {
            InitializeComponent();
            InitializeSimulation();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Одномерная двухфазная задача Стефана";
            this.ClientSize = new Size(650, 700);
            this.DoubleBuffered = true;

            // Создаем кнопки управления
            var startButton = new Button { Text = "Старт", Location = new Point(10, 10) };
            var stopButton = new Button { Text = "Стоп", Location = new Point(100, 10) };
            var resetButton = new Button { Text = "Сброс", Location = new Point(190, 10) };

            startButton.Click += (s, e) => simulationTimer.Start();
            stopButton.Click += (s, e) => simulationTimer.Stop();
            resetButton.Click += (s, e) => ResetSimulation();

            this.Controls.Add(startButton);
            this.Controls.Add(stopButton);
            this.Controls.Add(resetButton);

            // Создаем таймер
            simulationTimer = new System.Windows.Forms.Timer { Interval = 50 };
            simulationTimer.Tick += (s, e) => {
                SimulateStep();
                this.Invalidate();
            };

            // Инициализируем буфер для рисования
            drawBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void InitializeSimulation()
        {
            // Начальные условия
            for (int i = 0; i <= N + 1; i++)
            {
                T[i] = 0.9f; // Начальная температура ниже Tp
                K[i] = 1.0f; // Коэффициент для твердой фазы
                TNext[i] = T[i];
            }

            // Граничные условия
            T[1] = T[2]; // Левый край (изолирован)
            T[N] = T[N - 1]; // Правый край (изолирован)

            j = 1; // Начальное положение границы фаз
            dQ = 0;
        }

        private void ResetSimulation()
        {
            simulationTimer.Stop();
            InitializeSimulation();
            this.Invalidate();
        }

        private void SimulateStep()
        {
            // Определяем коэффициенты теплопроводности
            for (int i = 1; i <= N; i++)
            {
                K[i] = T[i] > Tp ? 3.0f : 1.0f;
            }

            // Находим границу фаз
            for (int i = 1; i < N; i++)
            {
                if (T[i] >= Tp && T[i + 1] < Tp)
                {
                    j = i;
                    break;
                }
            }

            // Фиксируем температуру на границе фаз
            T[j] = Tp;

            // Вычисляем поток тепла через границу
            float heatFlow = Math.Abs(K[j - 1] * (T[j] - T[j - 1]) / h) -
                           Math.Abs(K[j + 1] * (T[j + 1] - T[j]) / h);

            dQ += heatFlow;

            // Проверяем, достаточно ли энергии для плавления
            if (dQ >= Qpl)
            {
                // Плавление слоя
                dQ -= Qpl;

                // Обновляем температуру в жидкой фазе (от 2 до j-1)
                for (int i = 2; i < j; i++)
                {
                    if (i >= 2 && i <= N - 1) // Проверка границ
                        CalculateTemperature(i);
                }

                // Устанавливаем температуру на новой границе
                if (j + 1 <= N) TNext[j + 1] = Tp;

                // Обновляем температуру в твердой фазе (от j+2 до N-1)
                for (int i = j + 2; i < N; i++)
                {
                    if (i >= 2 && i <= N - 1) // Проверка границ
                        CalculateTemperature(i);
                }
            }
            else
            {
                // Недостаточно энергии для плавления
                // Обновляем температуру в жидкой фазе (от 2 до j-2)
                for (int i = 2; i < j - 1; i++)
                {
                    if (i >= 2 && i <= N - 1) // Проверка границ
                        CalculateTemperature(i);
                }

                // Сглаживаем температуру около границы
                if (j - 1 >= 1 && j - 1 <= N) TNext[j - 1] = (T[j - 2] + Tp) / 2;
                if (j >= 1 && j <= N) TNext[j] = Tp;
                if (j + 1 >= 1 && j + 1 <= N) TNext[j + 1] = (T[j + 2] + Tp) / 2;

                // Обновляем температуру в твердой фазе (от j+2 до N-1)
                for (int i = j + 2; i < N; i++)
                {
                    if (i >= 2 && i <= N - 1) // Проверка границ
                        CalculateTemperature(i);
                }
            }

            // Обновляем граничные условия
            TNext[1] = TNext[2];
            TNext[N] = TNext[N - 1];

            // Копируем новые значения температуры
            Array.Copy(TNext, T, N + 2);

            // Добавляем тепло в левый край (имитация источника)
            if (T[2] < 1.05f)
            {
                T[1] += 0.05f;
                T[2] += 0.05f;
            }
        }

        private void CalculateTemperature(int i)
        {
            // Проверка границ массива
            if (i < 1 || i > N || i + 1 > N || i - 1 < 1)
                return;

            float q1 = (i == 2 && T[2] <= 1.05f) ? 0.05f : 0;

            TNext[i] = T[i] +
                      K[i] * (T[i + 1] - 2 * T[i] + T[i - 1]) * dt / (h * h) +
                      (K[i + 1] - K[i - 1]) * (T[i + 1] - T[i - 1]) * dt / (4 * h * h) +
                      q1 * dt;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var g = Graphics.FromImage(drawBuffer))
            {
                g.Clear(Color.White);

                // Рисуем оси
                g.DrawLine(Pens.Black, 20, 650, 620, 650); // Ось X
                g.DrawLine(Pens.Black, 20, 50, 20, 650);   // Ось Y

                // Подписи осей
                g.DrawString("Положение (x)", this.Font, Brushes.Black, 300, 660);
                g.DrawString("Температура (T)", this.Font, Brushes.Black, 5, 300);

                // Линия температуры плавления
                g.DrawLine(Pens.Red, 20, 650 - Tp * 500, 620, 650 - Tp * 500);
                g.DrawString($"T плавления = {Tp}", this.Font, Brushes.Red, 500, 650 - Tp * 500 - 20);

                // Рисуем график температуры
                for (int i = 2; i <= N; i++)
                {
                    float x1 = i * Scale + 10;
                    float y1 = 650 - T[i] * 500;
                    float x2 = (i - 1) * Scale + 10;
                    float y2 = 650 - T[i - 1] * 500;

                    // Цвет в зависимости от фазы
                    var pen = T[i] > Tp ? Pens.Blue : Pens.Black;

                    g.DrawLine(pen, x1, y1, x2, y2);

                    // Маркируем границу фаз
                    if (i == j)
                    {
                        g.FillEllipse(Brushes.Red, x1 - 3, y1 - 3, 6, 6);
                    }
                }

                // Отображаем информацию о состоянии
                g.DrawString($"Граница фаз: x = {j}", this.Font, Brushes.Black, 20, 60);
                g.DrawString($"Накопленная энергия: {dQ:F2}/{Qpl}", this.Font, Brushes.Black, 20, 80);
            }

            // Копируем буфер на экран
            e.Graphics.DrawImage(drawBuffer, 0, 0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (drawBuffer != null)
            {
                drawBuffer.Dispose();
                drawBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                this.Invalidate();
            }
        }
    }
}