namespace Conde.RainhasToroidais;

/// <summary>Uma linha da tabela: n, solucoes no toro, quantas sao retas, e o tabuleiro comum.</summary>
public sealed record Linha(int N, long Toro, long Lineares, long FormulaLinear, long Planas);

/// <summary>As tabelas do README, cada n medido uma vez e guardado.</summary>
public static class Medidas
{
    public const int Ate = 17;

    /// <summary>Os dois que passam de dez segundos: ficam no job pesado.</summary>
    public static readonly int[] Grandes = { 18, 19 };

    /// <summary>Ate onde o tabuleiro comum e contado (o 17 sozinho leva quase um minuto).</summary>
    public const int AtePlano = 15;

    private static readonly Dictionary<int, Linha> Cache = new();
    private static readonly Dictionary<int, long> CacheBruta = new();
    private static readonly Dictionary<int, long> CacheColuna = new();

    public static Linha Medir(int n)
    {
        lock (Cache)
        {
            if (Cache.TryGetValue(n, out var l)) return l;
            long retas = 0;
            long reps = Busca.Representantes(n, s => { if (Tabuleiro.Linear(s)) retas++; });
            l = new Linha(n, reps * n, retas * n, FormulaLinear(n), n <= AtePlano ? Busca.Plano(n) : -1);
            Cache[n] = l;
            return l;
        }
    }

    public static List<Linha> Tabela() => Enumerable.Range(1, Ate).Select(Medir).ToList();

    /// <summary>A forca bruta por permutacoes, guardada (n ate 11).</summary>
    public static long ForcaBruta(int n)
    {
        lock (CacheBruta)
        {
            if (!CacheBruta.TryGetValue(n, out var v))
                CacheBruta[n] = v = Juiz.ForcaBruta(n);
            return v;
        }
    }

    /// <summary>A busca por coluna, guardada.</summary>
    public static long PorColuna(int n)
    {
        lock (CacheColuna)
        {
            if (!CacheColuna.TryGetValue(n, out var v))
                CacheColuna[n] = v = Juiz.PorColuna(n);
            return v;
        }
    }

    /// <summary>
    /// Quantas retas modulares col[r] = a*r + b sao solucao: a reta vale
    /// exatamente quando a, a - 1 e a + 1 sao todos inversiveis modulo n (as
    /// colunas, as diagonais r - c e as r + c nao repetem). Cada a vale para
    /// os n valores de b.
    /// </summary>
    public static long FormulaLinear(int n)
    {
        if (n == 1) return 1;
        long bons = 0;
        for (int a = 0; a < n; a++)
            if (Mdc(a, n) == 1 && Mdc(Tabuleiro.Mod(a - 1, n), n) == 1 && Mdc(Tabuleiro.Mod(a + 1, n), n) == 1)
                bons++;
        return bons * n;
    }

    public static int Mdc(int a, int b) => b == 0 ? a : Mdc(b, a % b);

    /// <summary>A primeira solucao nao linear de n, ou nulo se todas forem retas.</summary>
    public static int[]? PrimeiraNaoLinear(int n)
    {
        int[]? achada = null;
        Busca.Representantes(n, s => { if (achada == null && !Tabuleiro.Linear(s)) achada = s; });
        return achada;
    }
}
