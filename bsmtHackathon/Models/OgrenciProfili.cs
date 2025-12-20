using System.ComponentModel.DataAnnotations;

namespace bsmtHackathon.Models
{
    public class OgrenciProfili
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal BaslangicButcesi { get; set; }
        public decimal KalanButce { get; set; }

        // DİKKAT: string yanına ? koyduk
        public string? Sehir { get; set; }
        public string? MutfakEkipmanlari { get; set; }
        public string? Alerjiler { get; set; }
        public string? BeslenmeTercihi { get; set; }
        public string? EkstraNotlar { get; set; }
    }
}