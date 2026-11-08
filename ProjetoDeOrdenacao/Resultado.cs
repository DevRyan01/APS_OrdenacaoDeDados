namespace ProjetoDeOrdenacao;


public class Resultado
{
    public int Volume { get; set; }
    public string TipoDado { get; set; } = "";
    public string Algoritmo { get; set; } = "";
    public double TempoMedio { get; set; }
    public double TempoMinimo { get; set; }
    public double TempoMaximo { get; set; }
    public double DesvioPadrao { get; set; }
    public bool OrdenadoCorretamente { get; set; }
}
