using MedSchedulerUZ.Core.Common;

namespace MedSchedulerUZ.Core.Entities
{
    public class Certification : BaseEntity // Sertifikat 
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? DocumentBase64 { get; set; }
        public string? DocumentFileName { get; set; } // fayl nomi, masalan "ali_license.pdf"
        public DateTime IssuedDate { get; set; } // hujjat berilgan sana
        public DateTime? ExpiryDate { get; set; } // hujjatning muddati tugash sanasi
        public bool IsNotified { get; set; } = false; // muddati tugashiga 30 kun qolganda tizim ogohlantirish
                                                      // yuboradimi? Yuborilgan bo'lsa true —
                                                      // bir xil ogohlantirish ikki marta yuborilmasligi uchun

        // Navigation properties
        public User User { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
