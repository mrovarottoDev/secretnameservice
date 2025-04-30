
public class ExceptionLogEntry
{
    public DateTime DataHora { get; set; }
    public string Rota { get; set; }
    public string MetodoHttp { get; set; }
    public string CorrelationId { get; set; }
    public string Usuario { get; set; }
    public string IpOrigem { get; set; }
    public int StatusCode { get; set; }
    public string TipoExcecao { get; set; }
    public string MensagemErro { get; set; }
    public string TempoProcessamento { get; set; }
    public long TempoEmMilissegundos { get; set; }
    public string[] StackTrace { get; set; }
    public Dictionary<string, string> CamposRequest { get; set; } = new();
}
