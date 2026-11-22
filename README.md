# n rainhas no toro

O problema das n rainhas pede n rainhas num tabuleiro n por n sem nenhuma
atacando outra. No **toro** o tabuleiro da a volta nas bordas: a coluna que
sai pela direita volta pela esquerda, a linha que sai por baixo volta por
cima. Uma diagonal deixa de ter fim e passa a ser uma classe de resto: as
casas com o mesmo `r + c` modulo n, ou com o mesmo `r - c` modulo n.

Em 1918 George Polya provou que ha solucao no toro **exatamente quando n e
primo com 6**. Este repositorio conta as solucoes de 1 a 19, confere a busca
com dois juizes e pergunta quantas delas sao a resposta facil, uma reta.

## Como rodar

```bash
dotnet test testes/RainhasToroidais.Testes/RainhasToroidais.Testes.csproj -c Release
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- tabela
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- polya
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- lineares
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- planas
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- desenho
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- grandes
```

## A busca e os juizes

- **A busca** anda linha por linha com tres mascaras de n bits: colunas, as
  diagonais `r + c` e as `r - c`, todas modulo n. Ela prende a rainha da
  primeira linha na coluna 0 e multiplica por n no fim: deslocar todas as
  rainhas uma coluna leva solucao em solucao, e cada solucao tem exatamente
  uma translacao com a primeira rainha na coluna 0. Os testes conferem que o
  atalho nao muda nenhuma contagem de 1 a 15, e o job pesado confere o 17.
- **A forca bruta** gera as n! permutacoes e confere cada uma pela definicao,
  par por par, sem mascara nenhuma. Vai ate 11 (39.916.800 permutacoes).
- **A busca por coluna** anda pela outra direcao, guarda as rainhas num
  vetor e confere cada nova contra as anteriores pela definicao. Vai ate 13.

```
n    busca      forca bruta   por coluna
1    1          1             1
5    10         10            10
7    28         28            28
11   88         88            88
12   0          -             0
13   4524       -             4524
17   140692     -             -
19   820496     -             -
```

Todos os outros n de 1 a 18 dao zero nas tres colunas que alcancam.

## O que isto mediu

**O teorema de Polya confere de 1 a 19.** Os n com solucao sao exatamente
1, 5, 7, 11, 13, 17 e 19, os primos com 6. O 18 da zero, o 19 da 820.496.

**Ate 11 toda solucao e uma reta; no 13 so 2,87% sao.** Uma reta modular e
`coluna = a * linha + b (mod n)`, e ela funciona quando `a`, `a - 1` e
`a + 1` sao todos inversiveis modulo n, o que da `n (p - 3)` retas quando n e
primo. A varredura acha exatamente essas retas e mais nada ate 11. No 13 sao
130 retas entre 4.524 solucoes; no 17, 238 entre 140.692; no 19, 304 entre
820.496 (0,037%). A reta, que e a prova de Polya, vira uma raridade.

**Toda solucao do 5 no tabuleiro comum tambem vale no toro, e a fatia nao cai
de forma monotona.** O 5 comum tem 10 solucoes e as 10 sobrevivem a volta das
bordas. No 7, 28 de 40 (70%). No 11 a fatia despenca para 3,28% (88 de
2.680), e no 13 ela SOBE para 6,14% (4.524 de 73.712).

**Prender a primeira rainha na coluna 0 divide o tempo do 17 por 66.** A
busca inteira leva 47 segundos; com o atalho, menos de um. A divisao e exata
porque a translacao nunca leva uma solucao nela mesma.

## O que as medidas me corrigiram

**Pedi para o juiz por coluna conferir o 17 e ele nao terminou.** A primeira
rodada das tabelas ficou 15 minutos sem imprimir uma linha, e eu a matei.
Medindo n por n, a busca por mascara gasta 47 segundos no 17 sem o atalho, e
o juiz por coluna, que confere cada rainha contra as outras uma a uma, e
muito mais lento que isso. Ele ficou ate o 13, e o 17 passou a ser conferido
pela propria busca sem o atalho, no job pesado.

## Os arquivos

```
src/RainhasToroidais/Tabuleiro.cs   as definicoes no toro e no plano, e a reta
src/RainhasToroidais/Busca.cs       a busca por mascaras, com e sem a translacao
src/RainhasToroidais/Juiz.cs        forca bruta por permutacao e busca por coluna
src/RainhasToroidais/Medidas.cs     as tabelas deste README, guardadas
ferramentas/Medidor/                o executavel que imprime as tabelas
testes/                             39 testes; os 2 pesados rodam num job proprio
```

39 testes, 2 deles pesados.
