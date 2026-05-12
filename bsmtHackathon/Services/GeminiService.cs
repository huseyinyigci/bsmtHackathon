using System.Text;
using System.Text.Json;

namespace bsmtHackathon.Services
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        // IConfiguration'ı kullanarak ayarları projenin konfigürasyonundan çekiyoruz
        public GeminiService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            // "GoogleAI" altındaki "ApiKey" değerini okur
            _apiKey = configuration["GoogleAI:ApiKey"]; 
        }

        public async Task<string> AskGeminiAsync(string userPrompt)
        {
            // Yukarıdan gelen güvenli _apiKey'i kullanıyoruz
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = userPrompt } }
                    }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return $"Hata Oluştu! Google Status Code: {response.StatusCode}. Detay: {errorMsg}";
                }

                var responseString = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(responseString))
                {
                    return doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                }
            }
            catch (Exception ex)
            {
                return $"Bir sorun çıktı: {ex.Message}";
            }
        }
    }
}