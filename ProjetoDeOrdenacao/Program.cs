using System.Diagnostics;
using System.Globalization;
using System.Text;
using ProjetoDeOrdenacao;


CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

const int REPETICOES = 5;

int[] volumes = { 1000, 10000, 50000, 100000 };
int numVolumes = volumes.Length;

string[] tiposDado = { "Aleatorio", "Ordenado", "Semi-ordenado" };
int numTipos = tiposDado.Length;

string[] algoritmos = { "Heap Sort", "Merge Sort", "Selection Sort" };
int numAlgoritmos = algoritmos.Length;

int totalInternos = numVolumes * numTipos * numAlgoritmos;
int totalExternos = numVolumes * numAlgoritmos;
int totalTestes = totalInternos + totalExternos;
var resultados = new List<Resultado>(totalTestes);

Console.WriteLine("Executando testes, isso pode levar alguns segundos (Selection Sort em 100 mil eh o mais demorado)...\n");


foreach (string algoritmo in algoritmos)
{
    int[] aquecimento = new int[1000];
    GeradorDados.GerarAleatorio(aquecimento, 1000);
    MedirOrdenacao(algoritmo, aquecimento, 1000);
}


for (int v = 0; v < numVolumes; v++)
{
    int n = volumes[v];

    for (int t = 0; t < numTipos; t++)
    {
        for (int a = 0; a < numAlgoritmos; a++)
        {
            double[] tempos = new double[REPETICOES];
            bool todasOrdenadas = true;

            for (int rep = 0; rep < REPETICOES; rep++)
            {
                int[] dados = new int[n];

                if (tiposDado[t] == "Aleatorio")
                    GeradorDados.GerarAleatorio(dados, n);
                else if (tiposDado[t] == "Ordenado")
                    GeradorDados.GerarOrdenado(dados, n);
                else
                    GeradorDados.GerarSemiOrdenado(dados, n);

                
                tempos[rep] = MedirOrdenacao(algoritmos[a], dados, n);

                if (!Estatistica.EstaOrdenado(dados, n))
                    todasOrdenadas = false;
            }

            var r = Consolidar(n, tiposDado[t], algoritmos[a], tempos, todasOrdenadas);
            resultados.Add(r);
            ImprimirLinha(r);
        }
    }
    Console.WriteLine();
}


Console.WriteLine("Lendo dados externos de arquivo...\n");

string[] arquivosExternos =
{
    "dados_externos_1000.txt",
    "dados_externos_10000.txt",
    "dados_externos_50000.txt",
    "dados_externos_100000.txt"
};

for (int v = 0; v < numVolumes; v++)
{
    int n = volumes[v];

    int[] baseDados = new int[n];
    int lidos = GeradorDados.LerVetorDeArquivo(arquivosExternos[v], baseDados, n);
    if (lidos != n)
    {
        Console.Error.WriteLine($"Aviso: '{arquivosExternos[v]}' deveria ter {n} valores, mas foram lidos {lidos}.");
    }

    for (int a = 0; a < numAlgoritmos; a++)
    {
        double[] tempos = new double[REPETICOES];
        bool todasOrdenadas = true;

        for (int rep = 0; rep < REPETICOES; rep++)
        {
            
            int[] dados = new int[n];
            Array.Copy(baseDados, dados, n);

            tempos[rep] = MedirOrdenacao(algoritmos[a], dados, n);

            if (!Estatistica.EstaOrdenado(dados, n))
                todasOrdenadas = false;
        }

        var r = Consolidar(n, "Externo", algoritmos[a], tempos, todasOrdenadas);
        resultados.Add(r);
        ImprimirLinha(r);
    }
}
Console.WriteLine();


try
{
    using var csv = new StreamWriter("resultados.csv", false, new UTF8Encoding(false));
    csv.NewLine = "\n";
    csv.WriteLine($"Volume,TipoDado,Algoritmo,TempoMedioSegundos,TempoMinimoSegundos,TempoMaximoSegundos,DesvioPadrao,OrdenadoCorretamenteEmTodas_{REPETICOES}Execucoes");
    foreach (var r in resultados)
    {
        csv.WriteLine($"{r.Volume},{r.TipoDado},{r.Algoritmo},{r.TempoMedio:F6},{r.TempoMinimo:F6},{r.TempoMaximo:F6},{r.DesvioPadrao:F6},{(r.OrdenadoCorretamente ? "SIM" : "NAO")}");
    }
    Console.WriteLine("Resultados salvos em 'resultados.csv'.");
}
catch (IOException)
{
    Console.Error.WriteLine("Nao foi possivel gravar o arquivo CSV.");
}


Console.WriteLine($"\n===== TABELAS RESUMO (tempo em segundos, media de {REPETICOES} execucoes) =====");
for (int v = 0; v < numVolumes; v++)
{
    foreach (string tipo in tiposDado.Append("Externo"))
    {
        Console.WriteLine($"\n--- Volume: {volumes[v]} | Tipo de dado: {tipo} ---");
        Console.WriteLine($"{"Algoritmo",-16} {"Media",12} {"Minimo",12} {"Maximo",12} {"DesvPad",12}");
        foreach (var r in resultados)
        {
            if (r.Volume == volumes[v] && r.TipoDado == tipo)
            {
                Console.WriteLine($"{r.Algoritmo,-16} {r.TempoMedio,12:F6} {r.TempoMinimo,12:F6} {r.TempoMaximo,12:F6} {r.DesvioPadrao,12:F6}");
            }
        }
    }
}

return 0;


static double MedirOrdenacao(string algoritmo, int[] dados, int n)
{
    long inicio = Stopwatch.GetTimestamp();

    if (algoritmo == "Heap Sort")
        Ordenacao.HeapSort(dados, n);
    else if (algoritmo == "Merge Sort")
        Ordenacao.MergeSort(dados, 0, n - 1);
    else
        Ordenacao.SelectionSort(dados, n);

    return Stopwatch.GetElapsedTime(inicio).TotalSeconds;
}


static Resultado Consolidar(int n, string tipo, string algoritmo, double[] tempos, bool todasOrdenadas)
{
    double soma = 0.0;
    double minimo = tempos[0];
    double maximo = tempos[0];
    for (int rep = 0; rep < tempos.Length; rep++)
    {
        soma += tempos[rep];
        if (tempos[rep] < minimo) minimo = tempos[rep];
        if (tempos[rep] > maximo) maximo = tempos[rep];
    }
    double media = soma / tempos.Length;
    double desvio = Estatistica.CalcularDesvioPadrao(tempos, tempos.Length, media);

    return new Resultado
    {
        Volume = n,
        TipoDado = tipo,
        Algoritmo = algoritmo,
        TempoMedio = media,
        TempoMinimo = minimo,
        TempoMaximo = maximo,
        DesvioPadrao = desvio,
        OrdenadoCorretamente = todasOrdenadas
    };
}

static void ImprimirLinha(Resultado r)
{
    Console.WriteLine($"Volume: {r.Volume,7} | Tipo: {r.TipoDado,-14} | Algoritmo: {r.Algoritmo,-15} | Media: {r.TempoMedio,10:F6} s | Min: {r.TempoMinimo,10:F6} s | Max: {r.TempoMaximo,10:F6} s | DesvPad: {r.DesvioPadrao,10:F6} | Ordenado em todas: {(r.OrdenadoCorretamente ? "SIM" : "NAO")}");
}
