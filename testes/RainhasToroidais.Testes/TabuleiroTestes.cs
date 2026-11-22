using Conde.RainhasToroidais;
using Xunit;

namespace Conde.RainhasToroidais.Testes;

/// <summary>A definicao, com casos escritos a mao.</summary>
public class TabuleiroTestes
{
    [Fact]
    public void A_reta_de_passo_dois_resolve_o_5_no_toro()
    {
        var s = new[] { 0, 2, 4, 1, 3 }; // col = 2r mod 5
        Assert.True(Tabuleiro.SolucaoNoToro(s));
        Assert.True(Tabuleiro.SolucaoPlana(s));
        Assert.True(Tabuleiro.Linear(s));
    }

    [Fact]
    public void Uma_solucao_comum_do_4_nao_vale_no_toro()
    {
        var s = new[] { 1, 3, 0, 2 };
        Assert.True(Tabuleiro.SolucaoPlana(s));
        // r + c: 1, 4, 2, 5, que modulo 4 dao 1, 0, 2, 1: as linhas 0 e 3 dividem a diagonal
        Assert.False(Tabuleiro.SolucaoNoToro(s));
    }

    [Fact]
    public void A_diagonal_que_da_a_volta_ataca()
    {
        // no 5: linha 0 coluna 4 e linha 1 coluna 0 estao na mesma diagonal quando o tabuleiro da a volta
        Assert.False(Tabuleiro.SolucaoNoToro(new[] { 4, 0, 2, 1, 3 }));
    }

    [Fact]
    public void Coluna_repetida_e_coluna_fora_do_tabuleiro()
    {
        Assert.False(Tabuleiro.SolucaoNoToro(new[] { 0, 0, 2, 4, 1 }));
        Assert.False(Tabuleiro.SolucaoNoToro(new[] { 0, 2, 4, 1, 5 }));
        Assert.False(Tabuleiro.SolucaoPlana(new[] { 0, 2, 4, 1, 5 }));
    }

    [Fact]
    public void Reta_modular_e_o_que_nao_e()
    {
        Assert.True(Tabuleiro.Linear(new[] { 3, 0, 4, 1, 5, 2, 6 })); // col = 4r + 3 mod 7
        Assert.False(Tabuleiro.Linear(new[] { 0, 2, 4, 6, 11, 9, 12, 5, 3, 1, 7, 10, 8 }));
    }

    [Fact]
    public void Modulo_sempre_positivo()
    {
        Assert.Equal(3, Tabuleiro.Mod(-2, 5));
        Assert.Equal(0, Tabuleiro.Mod(10, 5));
    }
}
