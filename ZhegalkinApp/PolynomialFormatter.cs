using System.Collections.Generic;

namespace ZhegalkinApp
{
    public static class PolynomialFormatter
    {

        public static string Format(int[] a, int n)
        {
            var parts = new List<string>();

            for (int j = 0; j < a.Length; j++)
            {
                if (a[j] == 0) continue;

                if (j == 0)
                {
                    parts.Add("1");
                    continue;
                }

                var monomial = new System.Text.StringBuilder();
                for (int k = 0; k < n; k++)
                    if ((j & (1 << k)) != 0)
                        monomial.Append($"x{k + 1}");

                parts.Add(monomial.ToString());
            }

            return parts.Count == 0 ? "0" : string.Join(" \u2295 ", parts);
        }

    }
}
