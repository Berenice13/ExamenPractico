using Newtonsoft.Json;

namespace CustomerApi.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCustomerDataAsync()
        {
            // URL y headers del servicio externo
            var request = new HttpRequestMessage(HttpMethod.Get, "https://examentecnico.azurewebsites.net/v3/api/Test/Customer");
            request.Headers.Add("Version", "2.0.6.0");
            request.Headers.Add("Authorization", "Basic Y2hyaXN0b3BoZXJAZGV2ZWxvcC5teDpUZXN0aW5nRGV2ZWxvcDEyM0AuLi4=");

            // Realiza la llamada HTTP
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Lee el JSON de respuesta
            var content = await response.Content.ReadAsStringAsync();
            var  cleanedJson = content.Replace("\\r\\n", "").Replace("\\\"", "\"").Replace(" ", "");

            return cleanedJson;
        }

        public async Task<string?> GetCustomerApiResponseAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://examentecnico.azurewebsites.net/v3/api/Test/Customer");
                request.Headers.Add("Version", "2.0.6.0");
                request.Headers.Add("Authorization", "Basic Y2hyaXN0b3BoZXJAZGV2ZWxvcC5teDpUZXN0aW5nRGV2ZWxvcDEyM0AuLi4=");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<string>(responseContent);

                return jsonResponse ?? null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al obtener datos: {e.Message}");
                return null;
            }
        }

    }
}
