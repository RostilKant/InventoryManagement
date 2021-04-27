using System.Text.Json;

namespace Entities.ErrorModels
{
    public class GlobalError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}