using Conde.RainhasToroidais;
using Xunit;

namespace Conde.RainhasToroidais.Testes;

/// <summary>A busca contra os dois juizes e contra ela mesma sem o atalho.</summary>
public class BuscaTestes
{
    [Theory]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    public void A_busca_bate_com_a_forca_bruta(int n)
    {
        Assert.Equal(Medidas.ForcaBruta(n), Busca.Toro(n));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    public void A_busca_bate_com_a_busca_por_coluna(int n)
    {
        Assert.Equal(Medidas.PorColuna(n), Busca.Toro(n));
    }

    [Fact]
    public void O_atalho_da_translacao_nao_muda_nenhuma_contagem()
    {
        for (int n = 1; n <= 15; n++)
            Assert.Equal(Busca.Toro(n, translacao: false), Busca.Toro(n));
    }

    [Fact]
    public void Toda_solucao_do_13_vale_pela_definicao_no_toro_e_no_plano()
    {
        var todas = new List<int[]>();
        Busca.Toro(13, todas.Add);
        Assert.Equal(4524, todas.Count);
        Assert.All(todas, s =>
        {
            Assert.True(Tabuleiro.SolucaoNoToro(s));
            Assert.True(Tabuleiro.SolucaoPlana(s));
        });
        Assert.Equal(todas.Count, todas.Select(s => string.Join(",", s)).Distinct().Count());
    }

    [Fact]
    public void O_tabuleiro_comum_da_os_92_do_oito()
    {
        Assert.Equal(92, Busca.Plano(8));
        var l = new List<int[]>();
        Busca.Plano(6, l.Add);
        Assert.All(l, s => Assert.True(Tabuleiro.SolucaoPlana(s)));
        Assert.Equal(4, l.Count);
    }

    [Fact]
    public void Os_representantes_sao_um_enesimo()
    {
        for (int n = 5; n <= 13; n += 2)
            Assert.Equal(Busca.Toro(n), n * Busca.Representantes(n, _ => { }));
    }

    [Fact]
    public void A_proxima_permutacao_percorre_as_24_de_quatro()
    {
        var p = new[] { 0, 1, 2, 3 };
        int k = 1;
        while (Juiz.Proxima(p)) k++;
        Assert.Equal(24, k);
        Assert.Equal(new[] { 3, 2, 1, 0 }, p);
    }
}
