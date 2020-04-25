using Newtonsoft.Json;

namespace Dependencies.Graph.Api.Dtos
{
    public class ErrorDetailsDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
