using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SimplexWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeInputGrid();
        }

        private void InitializeInputGrid()
        {
            var dt = new DataTable();
            for (int i = 0; i < 4; i++)
                dt.Columns.Add($"X{i}");

            for (int i = 0; i < 4; i++)
                dt.Rows.Add(dt.NewRow());

            InputGrid.ItemsSource = dt.DefaultView;
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var inputTable = GetMatrixFromDataGrid(InputGrid);
                Simplex simplex = new Simplex(inputTable);

                double[] result = new double[inputTable.GetLength(1) - 1];
                var solvedTable = simplex.Calculate(result);

                ResultGrid.ItemsSource = ConvertMatrixToDataTable(solvedTable).DefaultView;

                string solution = "Результаты:\n";
                for (int i = 0; i < result.Length; i++)
                    solution += $"X{i + 1} = {result[i]:F2}\n";

                MessageBox.Show(solution, "Результаты решения", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private double[,] GetMatrixFromDataGrid(DataGrid grid)
        {
            var dt = (grid.ItemsSource as DataView).ToTable();
            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;
            var matrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var cell = dt.Rows[i][j];
                    if (cell == DBNull.Value || string.IsNullOrWhiteSpace(cell.ToString()))
                        matrix[i, j] = 0.0; // если ячейка пустая, считаем её за 0
                    else
                        matrix[i, j] = Convert.ToDouble(cell);
                }
            }

            return matrix;
        }

        private DataTable ConvertMatrixToDataTable(double[,] matrix)
        {
            var dt = new DataTable();
            for (int j = 0; j < matrix.GetLength(1); j++)
                dt.Columns.Add($"C{j}");

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < matrix.GetLength(1); j++)
                    row[j] = matrix[i, j];
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

    // ======= КЛАСС SIMPLEX ПРЯМО ЗДЕСЬ =======
    public class Simplex
    {
        private double[,] table;
        private int m, n;
        private List<int> basis;

        public Simplex(double[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            table = new double[m, n + m - 1];
            basis = new List<int>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j < n)
                        table[i, j] = source[i, j];
                    else
                        table[i, j] = 0;
                }
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }
            n = table.GetLength(1);
        }

        public double[,] Calculate(double[] result)
        {
            int mainCol, mainRow;

            while (!IsItEnd())
            {
                mainCol = findMainCol();
                mainRow = findMainRow(mainCol);
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                table = new_table;
            }

            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            return table;
        }

        private bool IsItEnd()
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        private int findMainCol()
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        private int findMainRow(int mainCol)
        {
            int mainRow = -1;
            double minRatio = double.MaxValue;

            for (int i = 0; i < m - 1; i++)
            {
                if (table[i, mainCol] > 0)
                {
                    double ratio = table[i, 0] / table[i, mainCol];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        mainRow = i;
                    }
                }
            }
            if (mainRow == -1)
                throw new Exception("Решение не существует (нет допустимого базиса).");

            return mainRow;
        }
    }
}
