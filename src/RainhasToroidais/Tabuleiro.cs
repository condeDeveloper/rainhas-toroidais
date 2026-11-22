namespace Conde.RainhasToroidais;

/// <summary>
/// Uma disposicao de n rainhas, uma por linha: col[r] e a coluna da rainha da
/// linha r. No tabuleiro comum duas rainhas se atacam na mesma coluna ou na
/// mesma diagonal. No TORO o tabuleiro da a volta nas bordas: a diagonal que
/// sai pela direita volta pela esquerda, entao as diagonais sao as classes de
/// r + c e de r - c modulo n.
/// </summary>
public static class Tabuleiro
{
    /// <summary>A definicao no toro, par por par, sem atalho.</summary>
    public static bool SolucaoNoToro(IReadOnlyList<int> col)
    {
        int n = col.Count;
        for (int a = 0; a < n; a++)
        {
            if (col[a] < 0 || col[a] >= n) return false;
            for (int b = a + 1; b < n; b++)
            {
                if (col[a] == col[b]) return false;
                if (Mod(a + col[a], n) == Mod(b + col[b], n)) return false;
                if (Mod(a - col[a], n) == Mod(b - col[b], n)) return false;
            }
        }
        return true;
    }

    /// <summary>A definicao no tabuleiro comum, par por par.</summary>
    public static bool SolucaoPlana(IReadOnlyList<int> col)
    {
        int n = col.Count;
        for (int a = 0; a < n; a++)
        {
            if (col[a] < 0 || col[a] >= n) return false;
            for (int b = a + 1; b < n; b++)
            {
                if (col[a] == col[b]) return false;
                if (Math.Abs(col[a] - col[b]) == b - a) return false;
            }
        }
        return true;
    }

    /// <summary>
    /// A solucao e uma reta modular quando col[r] = a*r + b (mod n) para algum
    /// a e b. Basta olhar as duas primeiras linhas: elas fixam b e a.
    /// </summary>
    public static bool Linear(IReadOnlyList<int> col)
    {
        int n = col.Count;
        if (n < 2) return true;
        int b = col[0], a = Mod(col[1] - col[0], n);
        for (int r = 0; r < n; r++)
            if (col[r] != Mod(a * r + b, n))
                return false;
        return true;
    }

    public static int Mod(int x, int n) => ((x % n) + n) % n;

    /// <summary>A disposicao desenhada, uma linha por linha do tabuleiro.</summary>
    public static List<string> Desenho(IReadOnlyList<int> col)
    {
        int n = col.Count;
        var r = new List<string>();
        for (int i = 0; i < n; i++)
            r.Add(string.Join(" ", Enumerable.Range(0, n).Select(c => c == col[i] ? "R" : ".")));
        return r;
    }
}
