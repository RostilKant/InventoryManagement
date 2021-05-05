using System.Text.Json;

namespace Entities.ErrorModels
{
    public record GlobalError
    {
        public int StatusCode { get; init; }
        public string Message { get; init; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}