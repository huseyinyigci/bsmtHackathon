namespace bsmtHackathon.Models
{
    public class DiyetProfilViewModel
    {
        // Kişisel Bilgiler
        public int Boy { get; set; }
        public int Kilo { get; set; }
        public string Cinsiyet { get; set; } = "Erkek"; // Varsayılan
        public string Hedef { get; set; }     // Kilo Vermek, Almak, Korumak
        public string Aktivite { get; set; }  // Hareketsiz, Orta, Yüksek

        // Kısıtlar
        public int OgunSayisi { get; set; } = 3;
        public decimal Butce { get; set; }

        // Uzun Metin Alanları
        public string? Alerjiler { get; set; }
        public string? Sevilenler { get; set; }
        public string? Sevilmeyenler { get; set; }
        public string? Ekipmanlar { get; set; } // Fırın, Airfryer vs.
        public string? MevcutEnvanter { get; set; } // Dolaptaki malzemeler
    }
}