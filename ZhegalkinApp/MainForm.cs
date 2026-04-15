using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZhegalkinApp
{
    public partial class MainForm : Form
    {
        private ComboBox cbVarCount = null;
        private DataGridView dgvTable = null;
        private Button btnCompute = null;
        private Button btnClear = null;
        private Label lblResult = null;
        private TextBox txtSteps = null;
        public MainForm()
        {
            InitializeComponent();
            BuildUI();
            cbVarCount.SelectedIndex = 0;
        }

        private void BuildUI()
        {
            // Форма
            Text = "Побудова полінома Жегалкіна";
            Size = new Size(900, 680);
            MinimumSize = new Size(820, 580);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10f);
            BackColor = Color.FromArgb(245, 247, 252);

            var lblTitle = new Label
            {
                Text = "Побудова полінома Жегалкіна — метод невизначених коефіцієнтів",
                Dock = DockStyle.Top,
                Height = 44,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 130),
                BackColor = Color.FromArgb(225, 233, 255),
            };

            // Ліва панель
            var pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 310,
                Padding = new Padding(14, 10, 8, 10),
            };

            pnlLeft.Resize += (s, e) =>
            {
                int y = pnlLeft.ClientSize.Height - 46;
                btnCompute.Location = new Point(14, y);
                btnClear.Location = new Point(156, y);
                dgvTable.Height = y - 112;
            };

            var lblN = new Label
            {
                Text = "Кількість змінних n:",
                AutoSize = true,
                Location = new Point(14, 10),
            };

            cbVarCount = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(14, 32),
                Width = 80,
            };
            cbVarCount.Items.AddRange(new object[] { "2", "3", "4" });
            cbVarCount.SelectedIndexChanged += (s, e) =>
            {
                TruthTableBuilder.GenerateTable(dgvTable, GetN());
                lblResult.Text = "Результат: —";
                txtSteps.Clear();
            };

            var lblHint = new Label
            {
                Text = "Введіть значення функції у стовпець f (0 або 1):",
                AutoSize = false,
                Width = 280,
                Height = 34,
                Location = new Point(14, 66),
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(80, 100, 160),
            };

            dgvTable = new DataGridView
            {
                Location = new Point(14, 104),
                Width = 278,
                Height = 400,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                GridColor = Color.FromArgb(200, 210, 235),
                Font = new Font("Consolas", 10f),
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                ScrollBars = ScrollBars.Vertical,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                        | AnchorStyles.Left,
            };
            dgvTable.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10f, FontStyle.Bold);
            dgvTable.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(210, 220, 245);
            dgvTable.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvTable.ColumnHeadersHeight = 28;

            // Валідація клітинки
            dgvTable.CellValidating += (s, e) =>
            {
                if (e.ColumnIndex != GetN()) return;
                string v = e.FormattedValue?.ToString()?.Trim();
                if (v == "" || v == "0" || v == "1")
                    dgvTable.Rows[e.RowIndex].ErrorText = "";
                else
                    dgvTable.Rows[e.RowIndex].ErrorText = "Лише 0 або 1!";
            };

            btnCompute = new Button
            {
                Text = "Обчислити",
                Location = new Point(30, 300),
                Width = 134,
                Height = 36,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(50, 100, 200),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
            };
            btnCompute.FlatAppearance.BorderSize = 0;
            btnCompute.Click += BtnCompute_Click;

            btnClear = new Button
            {
                Text = "Очистити",
                Location = new Point(156, 300),
                Width = 118,
                Height = 36,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 228, 245),
                ForeColor = Color.FromArgb(60, 80, 160),
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;

            pnlLeft.Controls.AddRange(new Control[]
                { lblN, cbVarCount, lblHint, dgvTable, btnCompute, btnClear });

            // Права панель
            var pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(8, 10, 14, 10),
            };

            // Панель результату
            var pnlRes = new Panel
            {
                Dock = DockStyle.Top,
                Height = 66,
                BackColor = Color.FromArgb(232, 240, 255),
                Padding = new Padding(12, 8, 12, 8),
            };

            lblResult = new Label
            {
                Text = "Результат: —",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Consolas", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 60, 140),
            };
            pnlRes.Controls.Add(lblResult);

            var lblSteps = new Label
            {
                Text = "Покрокове розв'язання системи рівнянь над GF(2):",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(80, 100, 160),
                Padding = new Padding(0, 6, 0, 0),
            };

            txtSteps = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9.5f),
                BackColor = Color.FromArgb(250, 251, 255),
                ForeColor = Color.FromArgb(30, 40, 80),
                BorderStyle = BorderStyle.FixedSingle,
            };

            pnlRight.Controls.Add(txtSteps);
            pnlRight.Controls.Add(lblSteps);
            pnlRight.Controls.Add(pnlRes);

            // Розділювач
            var splitter = new Splitter
            {
                Dock = DockStyle.Left,
                Width = 4,
                BackColor = Color.FromArgb(200, 210, 235),
            };

            Controls.Add(pnlRight);
            Controls.Add(splitter);
            Controls.Add(pnlLeft);
            Controls.Add(lblTitle);
        }

        // Обробники подій

        private void BtnCompute_Click(object sender, EventArgs e)
        {
            int n = GetN();
            int[] values = TruthTableBuilder.Validate(dgvTable, n);

            if (values == null)
            {
                MessageBox.Show(
                    "Усі клітинки стовпця f мають містити 0 або 1.\n" +
                    "Переконайтеся, що жодна клітинка не залишена порожньою.",
                    "Некоректні дані",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var solver = new ZhegalkinSolver();
            solver.Solve(values, n);

            lblResult.Text = $"f  =  {PolynomialFormatter.Format(solver.Coefficients, n)}";
            txtSteps.Text = solver.StepsLog;

            for (int i = 0; i < dgvTable.Rows.Count; i++)
                dgvTable.Rows[i].DefaultCellStyle.BackColor =
                    values[i] == 1 ? Color.FromArgb(220, 235, 255) : Color.White;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            int n = GetN();
            for (int i = 0; i < dgvTable.Rows.Count; i++)
            {
                dgvTable.Rows[i].Cells[n].Value = "";
                dgvTable.Rows[i].DefaultCellStyle.BackColor = Color.Empty;
                dgvTable.Rows[i].ErrorText = "";
            }
            lblResult.Text = "Результат: —";
            txtSteps.Clear();
        }

        private int GetN() =>
        cbVarCount.SelectedItem is string s && int.TryParse(s, out int n) ? n : 2;
    }
}
