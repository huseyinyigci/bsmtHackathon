using System;
using System.ComponentModel.DataAnnotations;

namespace bsmtHackathon.Models
{
    public class YemekPlaniDb
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;

        // DİKKAT: string yanına ? koyduk
        public string? PlanJsonData { get; set; }
    }
}