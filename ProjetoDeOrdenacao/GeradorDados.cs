namespace ProjetoDeOrdenacao;


public static class GeradorDados
{
    private static readonly Random Rng = new();

    
    public static void GerarAleatorio(int[] arr, int n)
    {
        for (int i = 0; i < n; i++)
        {
            arr[i] = Rng.Next(1000000);
        }
    }

   
    public static void GerarOrdenado(int[] arr, int n)
    {
        for (int i = 0; i < n; i++)
        {
            arr[i] = i;
        }
    }

    
    public static void GerarSemiOrdenado(int[] arr, int n)
    {
        GerarOrdenado(arr, n);
        int trocas = n / 10;
        for (int i = 0; i < trocas; i++)
        {
            int pos1 = Rng.Next(n);
            int pos2 = Rng.Next(n);
            Ordenacao.Troca(arr, pos1, pos2);
        }
    }

    
    public static int LerVetorDeArquivo(string nomeArquivo, int[] arr, int n)
    {
        if (!File.Exists(nomeArquivo))
        {
            Console.Error.WriteLine($"Nao foi possivel abrir '{nomeArquivo}'. Rode antes o programa SistemaGeradorDeNumeros");
            Environment.Exit(1);
        }

        int i = 0;
        using var leitor = new StreamReader(nomeArquivo);
        string? linha;
        while (i < n && (linha = leitor.ReadLine()) != null)
        {
            if (!int.TryParse(linha.Trim(), System.Globalization.NumberStyles.Integer,
                              System.Globalization.CultureInfo.InvariantCulture, out int valor))
            {
                break; 
            }
            arr[i] = valor;
            i++;
        }
        return i;
    }
}
