namespace ProjetoDeOrdenacao;


public static class Ordenacao
{
    public static void Troca(int[] arr, int a, int b)
    {
        int temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }

    
    public static void SelectionSort(int[] arr, int tamanho)
    {
        for (int i = 0; i < tamanho - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < tamanho; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }
            if (minIndex != i)
            {
                Troca(arr, i, minIndex);
            }
        }
    }

 
    public static void Heapify(int[] arr, int n, int i)
    {
        int maior = i;
        int filhoEsq = 2 * i + 1;
        int filhoDir = 2 * i + 2;

        if (filhoEsq < n && arr[filhoEsq] > arr[maior])
            maior = filhoEsq;
        if (filhoDir < n && arr[filhoDir] > arr[maior])
            maior = filhoDir;

        if (maior != i)
        {
            Troca(arr, i, maior);
            Heapify(arr, n, maior);
        }
    }

    public static void HeapSort(int[] arr, int n)
    {
        for (int i = n / 2 - 1; i >= 0; i--)
            Heapify(arr, n, i);
        for (int i = n - 1; i > 0; i--)
        {
            Troca(arr, 0, i);
            Heapify(arr, i, 0);
        }
    }

 
    public static void Merge(int[] arr, int first, int mid, int last)
    {
        int n1 = mid - first + 1;
        int n2 = last - mid;

        int[] esq = new int[n1];
        int[] dir = new int[n2];

        for (int x = 0; x < n1; x++) esq[x] = arr[first + x];
        for (int y = 0; y < n2; y++) dir[y] = arr[mid + 1 + y];

        int i = 0, j = 0, k = first;
        while (i < n1 && j < n2)
        {
            if (esq[i] <= dir[j]) arr[k++] = esq[i++];
            else arr[k++] = dir[j++];
        }
        while (i < n1) arr[k++] = esq[i++];
        while (j < n2) arr[k++] = dir[j++];
    }

    public static void MergeSort(int[] arr, int first, int last)
    {
        if (first < last)
        {
            int mid = (first + last) / 2;
            MergeSort(arr, first, mid);
            MergeSort(arr, mid + 1, last);
            Merge(arr, first, mid, last);
        }
    }
}
