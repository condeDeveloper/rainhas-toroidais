using System.Numerics;

namespace Conde.RainhasToroidais;

/// <summary>
/// A busca linha por linha com tres mascaras: colunas ocupadas, diagonais
/// r + c (mod n) ocupadas e diagonais r - c (mod n) ocupadas. No toro as
/// diagonais sao classes de resto, e cada uma das tres mascaras tem n bits.
/// </summary>
public static class Busca
{
    /// <summary>
    /// Conta as solucoes no toro; chama visita em cada uma se pedido.
    ///
    /// Com <paramref name="translacao"/>, a rainha da primeira linha fica presa
    /// na coluna 0 e o total e multiplicado por n: no toro, deslocar todas as
    /// rainhas uma coluna para a direita leva solucao em solucao, e cada
    /// solucao tem exatamente uma translacao com a primeira rainha na coluna 0.
    /// Quem pede as solucoes uma a uma (visita) recebe a busca inteira.
    /// </summary>
    public static long Toro(int n, Action<int[]>? visita = null, bool translacao = true)
    {
        if (n < 1 || n > 30) throw new ArgumentOutOfRangeException(nameof(n));
        uint cheio = (1u << n) - 1;
        var col = new int[n];
        long total = 0;
        if (translacao && visita == null)
        {
            col[0] = 0;
            Desce(1, 1u, 1u, 1u);
            return total * n;
        }
        Desce(0, 0u, 0u, 0u);
        return total;

        void Desce(int r, uint cols, uint soma, uint dif)
        {
            if (r == n)
            {
                total++;
                visita?.Invoke((int[])col.Clone());
                return;
            }
            uint livres = cheio & ~cols;
            while (livres != 0)
            {
                int c = BitOperations.TrailingZeroCount(livres);
                livres &= livres - 1;
                int s = (r + c) % n, d = (r - c + n) % n;
                if ((soma >> s & 1) != 0 || (dif >> d & 1) != 0) continue;
                col[r] = c;
                Desce(r + 1, cols | 1u << c, soma | 1u << s, dif | 1u << d);
            }
        }
    }

    /// <summary>
    /// So as solucoes do toro com a primeira rainha na coluna 0, uma de cada
    /// classe de translacao. Toda propriedade que nao muda com a translacao
    /// (ser uma reta modular, por exemplo) se conta aqui e se multiplica por n.
    /// </summary>
    public static long Representantes(int n, Action<int[]> visita)
    {
        if (n < 1 || n > 30) throw new ArgumentOutOfRangeException(nameof(n));
        uint cheio = (1u << n) - 1;
        var col = new int[n];
        long total = 0;
        if (n == 1) { visita(new[] { 0 }); return 1; }
        Desce(1, 1u, 1u, 1u);
        return total;

        void Desce(int r, uint cols, uint soma, uint dif)
        {
            if (r == n)
            {
                total++;
                visita((int[])col.Clone());
                return;
            }
            uint livres = cheio & ~cols;
            while (livres != 0)
            {
                int c = BitOperations.TrailingZeroCount(livres);
                livres &= livres - 1;
                int s = (r + c) % n, d = (r - c + n) % n;
                if ((soma >> s & 1) != 0 || (dif >> d & 1) != 0) continue;
                col[r] = c;
                Desce(r + 1, cols | 1u << c, soma | 1u << s, dif | 1u << d);
            }
        }
    }

    /// <summary>Conta as solucoes do tabuleiro comum (a busca classica por deslocamento).</summary>
    public static long Plano(int n, Action<int[]>? visita = null)
    {
        if (n < 1 || n > 30) throw new ArgumentOutOfRangeException(nameof(n));
        uint cheio = (1u << n) - 1;
        var col = new int[n];
        long total = 0;
        Desce(0, 0u, 0u, 0u);
        return total;

        void Desce(int r, uint cols, uint esq, uint dir)
        {
            if (r == n)
            {
                total++;
                visita?.Invoke((int[])col.Clone());
                return;
            }
            uint livres = cheio & ~(cols | esq | dir);
            while (livres != 0)
            {
                int c = BitOperations.TrailingZeroCount(livres);
                livres &= livres - 1;
                col[r] = c;
                uint b = 1u << c;
                Desce(r + 1, cols | b, ((esq | b) << 1) & cheio, (dir | b) >> 1);
            }
        }
    }
}
