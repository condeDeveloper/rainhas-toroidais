using Conde.RainhasToroidais;
using Xunit;

namespace Conde.RainhasToroidais.Testes;

/// <summary>Todo numero do README esta preso aqui.</summary>
public class MedidasTestes
{
    // n, solucoes no toro, retas modulares
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(5, 10, 10)]
    [InlineData(7, 28, 28)]
    [InlineData(11, 88, 88)]
    [InlineData(13, 4524, 130)]
    [InlineData(17, 140692, 238)]
    public void As_solucoes_e_as_retas(int n, long toro, long retas)
    {
        var l = Medidas.Medir(n);
        Assert.Equal(toro, l.Toro);
        Assert.Equal(retas, l.Lineares);
        Assert.Equal(retas, l.FormulaLinear);
    }

    [Fact]
    public void Polya_de_1_a_17()
    {
        foreach (var l in Medidas.Tabela())
            Assert.Equal(Medidas.Mdc(l.N, 6) == 1, l.Toro > 0);
    }

    [Fact]
    public void Ate_11_toda_solucao_e_uma_reta()
    {
        Assert.All(Medidas.Tabela().Where(l => l.N <= 11), l => Assert.Equal(l.Toro, l.Lineares));
        Assert.True(Medidas.Medir(13).Lineares < Medidas.Medir(13).Toro);
    }

    [Fact]
    public void A_conta_das_retas_e_n_vezes_p_menos_3_nos_primos()
    {
        foreach (int p in new[] { 5, 7, 11, 13, 17, 19 })
            Assert.Equal((long)p * (p - 3), Medidas.FormulaLinear(p));
    }

    // n, solucoes no plano, por cento que vale no toro
    [Theory]
    [InlineData(5, 10, 100.00)]
    [InlineData(7, 40, 70.00)]
    [InlineData(11, 2680, 3.28)]
    [InlineData(13, 73712, 6.14)]
    public void Quanto_do_tabuleiro_comum_sobrevive_ao_toro(int n, long planas, double porCento)
    {
        var l = Medidas.Medir(n);
        Assert.Equal(planas, l.Planas);
        Assert.Equal(porCento, Math.Round(100.0 * l.Toro / l.Planas, 2));
    }

    [Fact]
    public void O_tabuleiro_comum_ate_15()
    {
        long[] esperado = { 1, 0, 0, 2, 10, 4, 40, 92, 352, 724, 2680, 14200, 73712, 365596, 2279184 };
        Assert.Equal(esperado, Enumerable.Range(1, 15).Select(n => Medidas.Medir(n).Planas).ToArray());
        Assert.Equal(-1, Medidas.Medir(17).Planas);
    }

    [Fact]
    public void A_primeira_solucao_do_13_fora_da_reta()
    {
        Assert.Equal(new[] { 0, 2, 4, 6, 11, 9, 12, 5, 3, 1, 7, 10, 8 }, Medidas.PrimeiraNaoLinear(13));
        Assert.Null(Medidas.PrimeiraNaoLinear(11));
    }
}
