namespace ProjetoDeOrdenacao;


public static class Estatistica
{
    
    public static bool EstaOrdenado(int[] arr, int n)
    {
        for (int i = 1; i < n; i++)
        {
            if (arr[i - 1] > arr[i]) return false;
        }
        return true;
    }

    
    public static double RaizQuadrada(double x)
    {
        if (x <= 0.0) return 0.0;
        double chute = x;
        for (int i = 0; i < 40; i++)
        {
            chute = 0.5 * (chute + x / chute);
        }
        return chute;
    }

    
    public static double CalcularDesvioPadrao(double[] tempos, int n, double media)
    {
        if (n <= 1) return 0.0;
        double somaQuadrados = 0.0;
        for (int i = 0; i < n; i++)
        {
            double diff = tempos[i] - media;
            somaQuadrados += diff * diff;
        }
        return RaizQuadrada(somaQuadrados / (n - 1));
    }
}
