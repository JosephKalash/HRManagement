namespace HRManagement.Core.Enums
{
    public enum Religions
    {
        Islam,
        Christianity,
        Hinduism,
        Buddhism,
        Sikhism,
        Judaism,
    }

    public static class ReligionTranslations
    {
        public static string GetArabicTranslation(Religions religion)
        {
            return religion switch
            {
                Religions.Islam => "الإسلام",
                Religions.Christianity => "المسيحية",
                Religions.Judaism => "اليهودية",
                Religions.Hinduism => "الهندوسية",
                Religions.Buddhism => "البوذية",
                Religions.Sikhism => "السيخية",
                _ => "Unknown",
            };
        }
    }
}