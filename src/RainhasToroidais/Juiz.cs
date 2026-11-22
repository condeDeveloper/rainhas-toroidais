namespace Conde.RainhasToroidais;

/// <summary>
/// Dois juizes que nao usam mascara nenhuma.
///
/// O primeiro e a forca bruta: gera TODAS as n! permutacoes (uma rainha por
/// linha e por coluna ja vem de graca) e confere cada uma pela definicao,
/// par por par. Vai ate n = 11, que sao 39.916.800 permutacoes.
///
/// O segundo anda por COLUNA, e nao por linha, guardando as rainhas num
/// vetor e conferindo cada nova contra todas as anteriores com a definicao.
/// E lento, mas alcanca o 13, onde a forca bruta nao chega.
/// </summary>
public static class Juiz
{
    public static long ForcaBruta(int n)
    {
        if (n < 1 || n > 11) throw new ArgumentOutOfRangeException(nameof(n), "a forca bruta vai ate 11");
        var p = Enumerable.Range(0, n).ToArray();
        long total = 0;
        do
        {
            if (Tabuleiro.SolucaoNoToro(p))
                total++;
        } while (Proxima(p));
        return total;
    }

    /// <summary>A proxima permutacao em ordem lexicografica; falso quando acabou.</summary>
    public static bool Proxima(int[] p)
    {
        int i = p.Length - 2;
        while (i >= 0 && p[i] >= p[i + 1]) i--;
        if (i < 0) return false;
        int j = p.Length - 1;
        while (p[j] <= p[i]) j--;
        (p[i], p[j]) = (p[j], p[i]);
        Array.Reverse(p, i + 1, p.Length - i - 1);
        return true;
    }

    /// <summary>Busca por coluna, conferindo cada rainha contra as outras pela definicao.</summary>
    public static long PorColuna(int n)
    {
        var linhaDaColuna = new int[n];
        long total = 0;
        Coloca(0);
        return total;

        void Coloca(int c)
        {
            if (c == n) { total++; return; }
            for (int r = 0; r < n; r++)
            {
                bool ok = true;
                for (int c2 = 0; c2 < c && ok; c2++)
                {
                    int r2 = linhaDaColuna[c2];
                    if (r2 == r
                        || Tabuleiro.Mod(r + c, n) == Tabuleiro.Mod(r2 + c2, n)
                        || Tabuleiro.Mod(r - c, n) == Tabuleiro.Mod(r2 - c2, n))
                        ok = false;
                }
                if (!ok) continue;
                linhaDaColuna[c] = r;
                Coloca(c + 1);
            }
        }
    }
}
