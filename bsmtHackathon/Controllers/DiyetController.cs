using Microsoft.AspNetCore.Mvc;
using bsmtHackathon.Models;
using bsmtHackathon.Services;
using System.Text.RegularExpressions;

namespace bsmtHackathon.Controllers
{
    public class DiyetController : Controller
    {
        private readonly GeminiService _geminiService;

        public DiyetController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpGet]
        public IActionResult ProfilOlustur()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlanOlustur(
    DiyetProfilViewModel p,
    string? AcilDurumNotu,
    List<string>? SatinAlinanlar,
    string? YenilenYemekBilgisi)
        {
            // 1. Null Kontrolleri
            p.Alerjiler ??= "Yok";
            p.Ekipmanlar ??= "Standart";
            p.MevcutEnvanter ??= "Boş";

            // 2. Senaryo Oluşturma
            string guncellemeNotu = "";

            if (SatinAlinanlar != null && SatinAlinanlar.Count > 0)
            {
                guncellemeNotu += $"- KULLANICI ALIŞVERİŞ YAPTI: Şu ürünleri envantere ekle: {string.Join(", ", SatinAlinanlar)}.\n";
            }

            if (!string.IsNullOrEmpty(YenilenYemekBilgisi))
            {
                guncellemeNotu += $"- KULLANICI YEMEK YEDİ: Şu yemeğin malzemelerini envanterden düş: {YenilenYemekBilgisi}.\n";
            }

            if (!string.IsNullOrEmpty(AcilDurumNotu))
            {
                guncellemeNotu += $"- KULLANICI BİLDİRİMİ: {AcilDurumNotu}.\n";
            }

            // 3. Sertleştirilmiş Prompt (DÜZELTİLMİŞ VERSİYON)
            string prompt = $@"
        SİSTEM ROLÜ: Sen konuşan bir asistan DEĞİLSİN. Sen sadece JSON verisi üreten bir API motorusun.
        ASLA sohbet cümlesi kurma. Çıktın '{{' harfi ile başlamalı.
        
        GÖREV:
        Kullanıcının profiline ve güncel olaylara göre 7 GÜNLÜK tam beslenme planı oluştur.
        
        MEVCUT ENVANTER: {p.MevcutEnvanter}
        
        SON DAKİKA GÜNCELLEMELERİ (Buna göre envanteri ve planı revize et):
        {guncellemeNotu}

        KULLANICI BİLGİLERİ:
        - Profil: {p.Boy}cm, {p.Kilo}kg, {p.Hedef}
        - Bütçe: {p.Butce} TL (Bunu aşma)
        
        KURALLAR:
        1. 'Plan' listesi mutlaka 7 günü (Pazartesi-Pazar) içermelidir. Eksik gün olmasın.
        2. 'AlisverisListesi'ndeki her ürüne mantıklı bir 'TahminiFiyat' (TL cinsinden sayı) yaz. 0 yazma.
        3. 'GuncelEnvanter' alanına, yapılan ekleme/çıkarmalardan sonraki son stok durumunu yaz.
        
        İSTENEN FORMAT (SADECE BU JSON):
        {{
            ""GuncelEnvanter"": ""Güncellenmiş stok listesi..."",
            ""OneriNotu"": ""Duruma özel kısa tavsiye"",
            ""HedefAnalizi"": ""Analiz cümlesi"",
            ""AlisverisListesi"": [ 
                {{ ""Urun"": ""Tavuk"", ""Miktar"": 500, ""Birim"": ""gr"", ""TahminiFiyat"": 150 }} 
            ],
            ""Plan"": [ 
                {{ 
                    ""Gun"": ""Pazartesi"", 
                    ""Ogunler"": [ 
                        {{ 
                            ""OgunAdi"": ""Kahvaltı"", 
                            ""YemekAdi"": ""Menemen"", 
                            ""Tarif"": ""..."", 
                            ""Kalori"": 400, 
                            ""Malzemeler"": [ {{ ""Ad"": ""Yumurta"", ""Miktar"": 2, ""Birim"": ""adet"" }} ] 
                        }} 
                    ] 
                }} 
            ]
        }}
    ";

            try
            {
                // 4. Yapay Zekaya Gönder
                string aiResponse = await _geminiService.AskGeminiAsync(prompt);

                // 5. Temizlik (Markdown ve olası sohbet metinlerini tıraşla)
                // İlk '{' karakterini bul ve öncesini sil (Eğer yapay zeka "Tamam işte JSON:" dediyse bunu siler)
                int firstBracket = aiResponse.IndexOf('{');
                if (firstBracket >= 0)
                {
                    aiResponse = aiResponse.Substring(firstBracket);
                }

                // Sondaki fazlalıkları sil (Son '}' karakterinden sonrasını at)
                int lastBracket = aiResponse.LastIndexOf('}');
                if (lastBracket >= 0)
                {
                    aiResponse = aiResponse.Substring(0, lastBracket + 1);
                }

                // Deserialize Et
                var planModeli = System.Text.Json.JsonSerializer.Deserialize<DiyetPlanSonucViewModel>(aiResponse);

                // Envanter güncellendiyse View'a yansıması için p nesnesini güncelle
                if (!string.IsNullOrEmpty(planModeli?.GuncelEnvanter))
                {
                    p.MevcutEnvanter = planModeli.GuncelEnvanter;
                }

                ViewBag.ProfilVerisi = p;
                return View("PlanSonuc", planModeli);
            }
            catch (Exception ex)
            {
                // Hata olursa kullanıcıya gösterelim (Loglama yerine basitçe ekrana basıyoruz)
                ViewBag.Hata = "Yapay zeka veriyi işlerken bir sorun oluştu. Lütfen tekrar deneyin.";
                ViewBag.HataDetay = ex.Message; // Hatanın sebebini gör
                ViewBag.ProfilVerisi = p;
                return View("PlanSonuc", new DiyetPlanSonucViewModel());
            }
        }
    }
}