using Conde.RainhasToroidais;
using Xunit;

namespace Conde.RainhasToroidais.Testes;

/// <summary>
/// O 18, o 19 e o 17 sem o atalho da translacao: dezenas de segundos cada.
/// Rodam uma vez so, no job proprio do CI.
/// </summary>
[Trait("Category", "Pesado")]
public class PesadosTestes
{
    [Fact]
    public void Polya_no_18_e_no_19()
    {
        Assert.Equal(0, Medidas.Medir(18).Toro);
        var l = Medidas.Medir(19);
        Assert.Equal(820496, l.Toro);
        Assert.Equal(304, l.Lineares);
        Assert.Equal(304, l.FormulaLinear);
    }

    [Fact]
    public void O_17_sem_o_atalho_da_translacao()
    {
        Assert.Equal(140692, Busca.Toro(17, translacao: false));
    }
}
