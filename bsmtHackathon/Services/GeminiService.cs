using System.Text;
using System.Text.Json;

namespace bsmtHackathon.Services // DİKKAT: Burası senin proje isminle aynı olmalı
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly string _modelId;
        private readonly HttpClient _httpClient;

        public GeminiService(IConfiguration configuration, HttpClient httpClient)
        {
            // appsettings.json dosyasından bilgileri çekiyoruz
            _httpClient = httpClient;
        }

        public async Task<string> AskGeminiAsync(string userPrompt)
        {
            // --- BURAYA DİKKAT: API KEY ---
            string apiKey = "AIzaSyAYny-t3wRMUGIA-wzRund0bqoxf_x3EmY";

            // Hız ve ücretsiz kota için 'flash' modelini kullanıyoruz
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent?key={apiKey}";

            // 2. Gönderilecek Veri (Google'ın istediği format)
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = userPrompt }
                        }
                    }
                }
            };

            // 3. JSON'a çevir ve isteği hazırla
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                // 4. İsteği gönder
                var response = await _httpClient.PostAsync(url, jsonContent);

                // Hata varsa yakala
                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return $"Hata Oluştu! Google Status Code: {response.StatusCode}. Detay: {errorMsg}";
                }

                // 5. Cevabı oku
                var responseString = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(responseString))
                {
                    // JSON içinden sadece metin cevabını cımbızla çek
                    string text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return text;
                }
            }
            catch (Exception ex)
            {
                return $"Bir sorun çıktı: {ex.Message}";
            }
        }
    }
}