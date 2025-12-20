using Microsoft.AspNetCore.Mvc;
using bsmtHackathon.Data;   // Veritabanı erişimi için
using bsmtHackathon.Models; // Modeller için

namespace bsmtHackathon.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor: Veritabanı bağlantısını içeri alıyoruz
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // --- GİRİŞ YAP (LOGIN) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // 1. Veritabanında bu kullanıcı var mı?
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // 2. Kullanıcı bulundu! Kimliğini (ID) Session'a kaydet.
                // Artık sistem bu kişinin kim olduğunu biliyor.
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.AdSoyad);

                // 3. Ana sayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // 4. Kullanıcı yoksa hata mesajı ver
                ViewBag.Hata = "E-posta veya şifre hatalı!";
                return View();
            }
        }

        // --- KAYIT OL (REGISTER) ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string ad, string email, string password, string confirmPassword)
        {
            // Basit doğrulamalar
            if (password != confirmPassword)
            {
                ViewBag.Hata = "Şifreler uyuşmuyor!";
                return View();
            }

            // Bu e-posta daha önce kayıt olmuş mu?
            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Hata = "Bu e-posta zaten kayıtlı.";
                return View();
            }

            // Yeni kullanıcı oluştur
            var newUser = new User
            {
                AdSoyad = ad,
                Email = email,
                Password = password // Hackathon için şifreyi düz kaydediyoruz (Normalde hashlenir)
            };

            // Veritabanına ekle ve kaydet
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Giriş sayfasına yönlendir
            return RedirectToAction("Login");
        }

        // --- ÇIKIŞ YAP (LOGOUT) ---
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Hafızayı temizle
            return RedirectToAction("Index", "Home");
        }
    }
}