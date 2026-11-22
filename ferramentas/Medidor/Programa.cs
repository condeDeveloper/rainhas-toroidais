using System.Diagnostics;
using Conde.RainhasToroidais;

if (args.Length == 0)
{
    Console.WriteLine("medidor <tabela|polya|lineares|planas|desenho|grandes|tudo>");
    return 1;
}

switch (args[0])
{
    case "tabela": Tabela(); break;
    case "polya": Polya(); break;
    case "lineares": Lineares(); break;
    case "planas": Planas(); break;
    case "desenho": Desenho(); break;
    case "grandes": Grandes(); break;
    case "tudo": Tabela(); Polya(); Lineares(); Planas(); Desenho(); break;
    default:
        Console.WriteLine($"nao conheco '{args[0]}'");
        return 1;
}
return 0;

void Tabela()
{
    Console.WriteLine("n rainhas no toro: a busca por mascara, a forca bruta e a busca por coluna");
    Console.WriteLine("n    busca      forca bruta   por coluna   ms");
    foreach (var l in Medidas.Tabela())
    {
        var sw = Stopwatch.StartNew();
        string bruta = l.N <= 11 ? Medidas.ForcaBruta(l.N).ToString() : "-";
        string coluna = l.N <= 13 ? Medidas.PorColuna(l.N).ToString() : "-";
        Console.WriteLine($"{l.N,-4} {l.Toro,-10} {bruta,-13} {coluna,-12} {sw.ElapsedMilliseconds}");
    }
    Console.WriteLine();
}

void Polya()
{
    Console.WriteLine("Polya (1918): ha solucao no toro exatamente quando n e primo com 6");
    Console.WriteLine("n    mdc(n,6)   solucoes   confere");
    foreach (var l in Medidas.Tabela())
    {
        int m = Medidas.Mdc(l.N, 6);
        bool previsto = m == 1;
        Console.WriteLine($"{l.N,-4} {m,-10} {l.Toro,-10} {((l.Toro > 0) == previsto ? "sim" : "NAO")}");
    }
    Console.WriteLine();
}

void Lineares()
{
    Console.WriteLine("solucoes que sao uma reta modular col = a*r + b (mod n)");
    Console.WriteLine("n    solucoes   retas   pela conta   fora da reta");
    foreach (var l in Medidas.Tabela().Where(x => x.Toro > 0))
        Console.WriteLine($"{l.N,-4} {l.Toro,-10} {l.Lineares,-7} {l.FormulaLinear,-12} {l.Toro - l.Lineares}");
    Console.WriteLine();
}

void Planas()
{
    Console.WriteLine("quantas solucoes do tabuleiro comum tambem valem no toro");
    Console.WriteLine("n    comum        toro       por cento");
    foreach (var l in Medidas.Tabela().Where(x => x.Toro > 0 && x.Planas > 0))
        Console.WriteLine($"{l.N,-4} {l.Planas,-12} {l.Toro,-10} {100.0 * l.Toro / l.Planas:F2}");
    Console.WriteLine();
}

void Desenho()
{
    var s = Medidas.PrimeiraNaoLinear(13);
    if (s == null)
    {
        Console.WriteLine("o 13 nao tem solucao fora da reta");
        return;
    }
    Console.WriteLine("a primeira solucao do 13 que nao e uma reta modular: " + string.Join(" ", s));
    foreach (var linha in Tabuleiro.Desenho(s))
        Console.WriteLine("   " + linha);
    Console.WriteLine();
}

void Grandes()
{
    Console.WriteLine("o 18 e o 19 (dezenas de segundos)");
    Console.WriteLine("n    mdc(n,6)   solucoes   retas    pela conta   ms");
    foreach (int n in Medidas.Grandes)
    {
        var sw = Stopwatch.StartNew();
        var l = Medidas.Medir(n);
        Console.WriteLine($"{n,-4} {Medidas.Mdc(n, 6),-10} {l.Toro,-10} {l.Lineares,-8} {l.FormulaLinear,-12} {sw.ElapsedMilliseconds}");
    }
    Console.WriteLine();
}
