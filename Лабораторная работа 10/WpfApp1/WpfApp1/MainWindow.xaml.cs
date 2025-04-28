using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CardDrawer
{
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random();

        private readonly List<string> _suits = new List<string> { "♠", "♥", "♣", "♦" };
        private readonly List<string> _ranks = new List<string> { "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        private List<string> _deck = new List<string>();
        private readonly List<string> _history = new List<string>();

        private readonly Dictionary<string, int> _suitCounts = new Dictionary<string, int>
        {
            { "♠", 0 },
            { "♥", 0 },
            { "♣", 0 },
            { "♦", 0 }
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeDeck();
            DrawChart();
        }

        private void InitializeDeck()
        {
            _deck.Clear();
            foreach (var suit in _suits)
            {
                foreach (var rank in _ranks)
                {
                    _deck.Add($"{rank}{suit}");
                }
            }
        }

        private void DrawCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_deck.Count == 0)
            {
                MessageBox.Show("Все карты вытянуты!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                DrawCardButton.IsEnabled = false;
                return;
            }

            int index = _random.Next(_deck.Count);
            string card = _deck[index];
            _deck.RemoveAt(index);

            string suit = card.Last().ToString();

            CurrentCardText.Text = $"Текущая карта: {card}";
            CurrentCardText.Foreground = GetSuitColor(suit);

            _history.Add(card);

            var item = new TextBlock
            {
                Text = card,
                Foreground = GetSuitColor(suit),
                FontSize = 18
            };
            HistoryList.Items.Add(item);

            _suitCounts[suit]++;
            DrawChart();

            if (_deck.Count == 0)
            {
                DrawCardButton.IsEnabled = false;
            }
        }

        private void DrawChart()
        {
            SuitChart.Children.Clear();

            double canvasWidth = SuitChart.ActualWidth;
            double canvasHeight = SuitChart.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0)
            {
                SuitChart.Loaded += (s, e) => DrawChart();
                return;
            }

            double barWidth = canvasWidth / _suits.Count;
            int totalCards = _history.Count;
            int maxCount = _suitCounts.Values.Max();
            if (maxCount == 0) maxCount = 1;

            for (int i = 0; i < _suits.Count; i++)
            {
                string suit = _suits[i];
                int count = _suitCounts[suit];

                double barHeight = (count / (double)maxCount) * (canvasHeight - 60);

                Rectangle rect = new Rectangle
                {
                    Width = barWidth - 20,
                    Height = barHeight,
                    Fill = GetSuitColor(suit)
                };

                Canvas.SetLeft(rect, i * barWidth + 10);
                Canvas.SetTop(rect, canvasHeight - barHeight - 30);

                SuitChart.Children.Add(rect);

                TextBlock suitLabel = new TextBlock
                {
                    Text = suit,
                    FontSize = 16,
                    Width = barWidth,
                    TextAlignment = TextAlignment.Center
                };
                Canvas.SetLeft(suitLabel, i * barWidth);
                Canvas.SetTop(suitLabel, canvasHeight - 25);
                SuitChart.Children.Add(suitLabel);

                string percentText = totalCards > 0 ? $"{(count * 100 / (double)totalCards):0.0}%" : "0%";

                TextBlock percentLabel = new TextBlock
                {
                    Text = percentText,
                    FontSize = 14,
                    Foreground = Brushes.Black,
                    Width = barWidth,
                    TextAlignment = TextAlignment.Center
                };
                Canvas.SetLeft(percentLabel, i * barWidth);
                Canvas.SetTop(percentLabel, canvasHeight - barHeight - 50);
                SuitChart.Children.Add(percentLabel);
            }
        }

        private Brush GetSuitColor(string suit)
        {
            return (suit == "♥" || suit == "♦") ? Brushes.Red : Brushes.Black;
        }
    }
}
