using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace bsmtHackathon.Models
{
    // Ana Kutumuz (Tüm cevabı tutar)
    public class DiyetPlanSonucViewModel
    {
        [JsonPropertyName("OneriNotu")]
        public string OneriNotu { get; set; }

        [JsonPropertyName("HedefAnalizi")]
        public string HedefAnalizi { get; set; }

        [JsonPropertyName("AlisverisListesi")]
        public List<AlisverisItem> AlisverisListesi { get; set; }

        [JsonPropertyName("Plan")]
        public List<GunlukPlan> Plan { get; set; }

        [JsonPropertyName("GuncelEnvanter")]
        public string GuncelEnvanter { get; set; }
    }

    // Alışveriş Listesindeki Tek Bir Ürün
    public class AlisverisItem
    {
        [JsonPropertyName("Urun")]
        public string Urun { get; set; }

        [JsonPropertyName("Miktar")]
        public double Miktar { get; set; }

        [JsonPropertyName("Birim")]
        public string Birim { get; set; }

        [JsonPropertyName("TahminiFiyat")]
        public decimal TahminiFiyat { get; set; }
    }

    // Günlük Plan (Pazartesi, Salı...)
    public class GunlukPlan
    {
        [JsonPropertyName("Gun")]
        public string Gun { get; set; }

        [JsonPropertyName("Ogunler")]
        public List<Ogun> Ogunler { get; set; }
    }

    // Tek Bir Öğünün Detayı
    public class Ogun
    {
        [JsonPropertyName("OgunAdi")]
        public string OgunAdi { get; set; }

        [JsonPropertyName("YemekAdi")]
        public string YemekAdi { get; set; }

        [JsonPropertyName("NedenSectim")]
        public string NedenSectim { get; set; }

        [JsonPropertyName("HazirlamaSuresi")]
        public string HazirlamaSuresi { get; set; }

        [JsonPropertyName("Kalori")]
        public int Kalori { get; set; }

        [JsonPropertyName("Tarif")]
        public string Tarif { get; set; }

        [JsonPropertyName("Malzemeler")]
        public List<Malzeme> Malzemeler { get; set; }
    }

    // Yemeğin İçindeki Malzeme
    public class Malzeme
    {
        [JsonPropertyName("Ad")]
        public string Ad { get; set; }

        [JsonPropertyName("Miktar")]
        public double Miktar { get; set; }

        [JsonPropertyName("Birim")]
        public string Birim { get; set; }
    }
}