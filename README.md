# APS – Análise de performance de algoritmos de ordenação (C#)

Heap Sort, Merge Sort e Selection Sort, 4 volumes (1.000, 10.000, 50.000, 100.000), 3 tipos de dado interno (aleatório, ordenado, semi-ordenado)
mais dado externo lido de arquivo, 5 repetições por combinação, 48 combinações no total, saída em
`resultados.csv` e tabelas no terminal.

## Estrutura
- `SistemaGeradorDeNumeros`: gera os 4 arquivos `dados_externos_*.txt` (um inteiro por linha, 0 a 999999).
- `ProjetoDeOrdenacao`: programa de testes (`Ordenacao.cs`, `GeradorDados.cs`, `Estatistica.cs`, `Resultado.cs`, `Program.cs`).

## Como executar
Pré-requisito: .NET SDK 8 (ou Visual Studio 2022 com a carga de trabalho de desktop .NET).

```
# 1) gerar os arquivos externos (uma vez)
cd SistemaGeradorDeNumeros
dotnet run -c Release

# 2) copiar os 4 arquivos .txt gerados para a pasta ProjetoDeOrdenacao e rodar os testes
cd ../ProjetoDeOrdenacao
dotnet run -c Release
```

Antes das medições o programa executa cada algoritmo uma vez, fora da contagem de tempo (aquecimento), e o
projeto desativa a compilação em camadas do .NET (`TieredCompilation` no `.csproj`). Sem isso, a compilação JIT
entra no tempo dos volumes pequenos e distorce a comparação.

Rode sempre em **Release**. Em Debug os tempos ficam muito maiores e não representam o algoritmo.
O programa de testes procura os `.txt` na pasta atual e grava `resultados.csv` nela.
