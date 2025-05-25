using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace MonteCarloIntegration
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Функция для интегрирования: (3x + 4) / (x^4)
        private double Function(double x)
        {
            return (3 * x + 4) / Math.Pow(x, 4);
        }

        // Метод Монте-Карло с сохранением ошибок
        private (List<int> kValues, List<double> errors) MonteCarloIntegration(int n, int step)
        {
            double a = 0.18;
            double b = 0.98;
            double exactValue = 1.0; // Пока неизвестное точное значение
            double s = 0;
            List<int> kValues = new List<int>();
            List<double> errors = new List<double>();

            for (int k = 1; k <= n; k++)
            {
                double x = a + (b - a) * random.NextDouble();
                s += Function(x);

                if (k % step == 0)
                {
                    double integralApproximation = (b - a) / k * s;
                    double error = Math.Abs(exactValue - integralApproximation);
                    kValues.Add(k);
                    errors.Add(error);
                }
            }

            return (kValues, errors);
        }

        private void ComputeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TrialsTextBox.Text, out int n))
            {
                MessageBox.Show("Введите корректное число испытаний.");
                return;
            }

            var (kValues, errors) = MonteCarloIntegration(n, 10000);
            PlotErrors(kValues, errors);

            ResultTextBlock.Text = $"Расчёт завершён. Последняя ошибка: {errors.Last():F6}";
        }

        private void PlotErrors(List<int> kValues, List<double> errors)
        {
            PlotCanvas.Children.Clear();

            if (kValues.Count == 0) return;

            double width = PlotCanvas.ActualWidth;
            double height = PlotCanvas.ActualHeight;

            if (width == 0 || height == 0)
            {
                width = 700;
                height = 300;
            }

            double maxX = kValues.Max();
            double maxY = errors.Max();

            // Основа графика: линии осей
            Line xAxis = new Line
            {
                X1 = 0,
                Y1 = height,
                X2 = width,
                Y2 = height,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Line yAxis = new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = height,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            PlotCanvas.Children.Add(xAxis);
            PlotCanvas.Children.Add(yAxis);

            // Засечки и подписи на оси X
            int xTicks = 10;
            for (int i = 0; i <= xTicks; i++)
            {
                double x = i * width / xTicks;
                Line tick = new Line
                {
                    X1 = x,
                    Y1 = height - 5,
                    X2 = x,
                    Y2 = height + 5,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                PlotCanvas.Children.Add(tick);

                TextBlock label = new TextBlock
                {
                    Text = ((int)(i * maxX / xTicks)).ToString(),
                    FontSize = 10
                };
                Canvas.SetLeft(label, x - 10);
                Canvas.SetTop(label, height + 5);
                PlotCanvas.Children.Add(label);
            }

            // Засечки и подписи на оси Y
            int yTicks = 10;
            for (int i = 0; i <= yTicks; i++)
            {
                double y = height - (i * height / yTicks);
                Line tick = new Line
                {
                    X1 = -5,
                    Y1 = y,
                    X2 = 5,
                    Y2 = y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                PlotCanvas.Children.Add(tick);

                TextBlock label = new TextBlock
                {
                    Text = (i * maxY / yTicks).ToString("F2"),
                    FontSize = 10
                };
                Canvas.SetLeft(label, -40);
                Canvas.SetTop(label, y - 8);
                PlotCanvas.Children.Add(label);
            }

            // Построение линии ошибок
            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };

            for (int i = 0; i < kValues.Count; i++)
            {
                double x = (kValues[i] / maxX) * width;
                double y = height - (errors[i] / maxY) * height;
                polyline.Points.Add(new Point(x, y));
            }

            PlotCanvas.Children.Add(polyline);
        }
    }
}
