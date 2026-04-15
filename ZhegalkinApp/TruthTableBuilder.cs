using System.Drawing;
using System.Windows.Forms;

namespace ZhegalkinApp
{
    public static class TruthTableBuilder
    {
        // таблиці у DataGridView
        public static void GenerateTable(DataGridView dgv, int n)
        {
            dgv.Columns.Clear();
            dgv.Rows.Clear();

            int rows = 1 << n;

            // Стовпці змінних x1...xn
            for (int j = 0; j < n; j++)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = $"x{j + 1}",
                    HeaderText = $"x{j + 1}",
                    Width = 46,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                };
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
                col.DefaultCellStyle.Font = new Font("Consolas", 10f);
                dgv.Columns.Add(col);
            }

            // Стовпець f
            var colF = new DataGridViewTextBoxColumn
            {
                Name = "f",
                HeaderText = "f",
                Width = 52,
                ReadOnly = false,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            colF.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colF.DefaultCellStyle.Font = new Font("Consolas", 10f, FontStyle.Bold);
            dgv.Columns.Add(colF);

            // Заповнення рядків
            for (int i = 0; i < rows; i++)
            {
                var row = new string[n + 1];
                for (int j = 0; j < n; j++)
                    row[j] = ((i >> (n - 1 - j)) & 1).ToString();
                row[n] = string.Empty;
                dgv.Rows.Add(row);
            }
        }

        // Перевіряє стовпець f
        public static int[] Validate(DataGridView dgv, int n)
        {
            int rows = 1 << n;
            var values = new int[rows];

            for (int i = 0; i < rows; i++)
            {
                string cell = dgv.Rows[i].Cells[n].Value?.ToString()?.Trim();
                if (cell != "0" && cell != "1")
                    return null;
                values[i] = int.Parse(cell);
            }
            return values;
        }

    }
}
