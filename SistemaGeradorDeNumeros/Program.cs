using System.Text;

                                     */
var rng = new Random();

GerarArquivo("dados_externos_1000.txt", 1000);
GerarArquivo("dados_externos_10000.txt", 10000);
GerarArquivo("dados_externos_50000.txt", 50000);
GerarArquivo("dados_externos_100000.txt", 100000);

Console.WriteLine("\nPronto. Os 4 arquivos foram criados na mesma pasta onde este programa rodou.");
return 0;

void GerarArquivo(string nomeArquivo, int quantidade)
{
    try
    {
        using var f = new StreamWriter(nomeArquivo, false, new UTF8Encoding(false));
        f.NewLine = "\n";
        for (int i = 0; i < quantidade; i++)
        {
            f.WriteLine(rng.Next(1000000));
        }
    }
    catch (IOException)
    {
        Console.Error.WriteLine($"Erro ao criar o arquivo '{nomeArquivo}'.");
        Environment.Exit(1);
    }
    Console.WriteLine($"Criado: {nomeArquivo} ({quantidade} numeros)");
}
