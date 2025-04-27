namespace SecretManagerService.Application.DTOs
{
    public class SecretResult
    {
        public bool IsKeyValue { get; set; }
        public Dictionary<string, string> KeyValues { get; set; }
        public string SingleValue { get; set; }
    }
}