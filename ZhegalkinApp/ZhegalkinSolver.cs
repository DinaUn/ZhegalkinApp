using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhegalkinApp
{
    public class ZhegalkinSolver
    {
        // Масив коефіцієнтів a[0..2n-1]
        public int[] Coefficients { get; private set; } = Array.Empty<int>();

        public string StepsLog { get; private set; } = string.Empty;

        // Головний метод
        public void Solve(int[] f, int n)
        {
            int m = 1 << n;
            var a = new int[m];
            var A = new int[m, m];
            var log = new System.Text.StringBuilder();

            log.AppendLine($"Кількість змінних: n = {n}");
            log.AppendLine($"Розмір системи: m = 2^{n} = {m}");
            log.AppendLine(new string('─', 50));
            log.AppendLine("Розв'язання (пряме підставлення над GF(2)):");
            log.AppendLine();

            // формування матриці A
            for (int i = 0; i < m; i++)
                for (int j = 0; j <= i; j++)
                    A[i, j] = ((i & j) == j) ? 1 : 0;

            // пряме підставлення
            for (int i = 0; i < m; i++)
            {
                int xorSum = 0;
                var terms = new List<string>();

                for (int j = 0; j < i; j++)
                {
                    if (A[i, j] == 1)
                    {
                        xorSum ^= a[j];
                        terms.Add($"a[{j}]={a[j]}");
                    }
                }

                a[i] = f[i] ^ xorSum;

                string name = GetMonomialName(i, n);
                if (terms.Count == 0)
                    log.AppendLine($"  a[{i}] = f[{i}] = {f[i]}  →  {a[i]}  ({name})");
                else
                    log.AppendLine($"  a[{i}] = f[{i}] ⊕ {{ {string.Join(", ", terms)} }} = {a[i]}  ({name})");
            }

            log.AppendLine();
            log.AppendLine(new string('─', 50));

            var nonZero = Enumerable.Range(0, m)
                .Where(j => a[j] == 1)
                .Select(j => GetMonomialName(j, n))
                .ToList();

            log.AppendLine(nonZero.Count == 0
                ? "Поліном: f = 0"
                : $"Поліном: f = {string.Join(" ⊕ ", nonZero)}");

            Coefficients = a;
            StepsLog = log.ToString();
        }

        // Повертає назву одночлена за маскою j
        private static string GetMonomialName(int j, int n)
        {
            if (j == 0) return "1 (константа)";
            var sb = new System.Text.StringBuilder();
            for (int k = 0; k < n; k++)
                if ((j & (1 << k)) != 0)
                    sb.Append($"x{k + 1}");
            return sb.ToString();
        }

    }
}
