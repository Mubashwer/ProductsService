using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Products.API.IntegrationTests.TestHelpers.Extensions
{
    public static class ObjectExtensions
    {
        public static StringContent ToJsonContent(this object data)
        {
            return new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}
