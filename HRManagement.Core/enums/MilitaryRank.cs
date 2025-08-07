using System.ComponentModel;

namespace HRManagement.Core.Enums
{
    /// <summary>
    /// Represents military ranks from civilian to highest military ranks
    /// with Arabic names and corresponding level numbers
    /// </summary>
    public enum MilitaryRank
    {
        [Description("مدني")]
        [EnglishDescription("Civilian")]
        Madani = 0,

        // Enlisted Ranks (1-10)
        [Description("جندي")]
        [EnglishDescription("Private")]
        Jundi = 1,

        [Description("جندي أول")]
        [EnglishDescription("Private First Class")]
        JundiAwwal = 2,

        [Description("عريف")]
        [EnglishDescription("Corporal")]
        Areef = 3,

        [Description("رقيب")]
        [EnglishDescription("Sergeant")]
        Raqeeb = 4,

        [Description("رقيب أول")]
        [EnglishDescription("Staff Sergeant")]
        RaqeebAwwal = 5,

        [Description("رئيس رقباء")]
        [EnglishDescription("Sergeant Major")]
        RaeesRuqaba = 6,

        // Non-Commissioned Officers (11-20)
        [Description("وكيل رقيب")]
        [EnglishDescription("Warrant Officer")]
        WakeelRaqeeb = 11,

        [Description("وكيل رقيب أول")]
        [EnglishDescription("Chief Warrant Officer")]
        WakeelRaqeebAwwal = 12,

        // Company Grade Officers (21-30)
        [Description("ملازم")]
        [EnglishDescription("Second Lieutenant")]
        Mulazim = 21,

        [Description("ملازم أول")]
        [EnglishDescription("First Lieutenant")]
        MulazimAwwal = 22,

        [Description("نقيب")]
        [EnglishDescription("Captain")]
        Naqeeb = 23,

        [Description("رائد")]
        [EnglishDescription("Major")]
        Raed = 24,

        // Field Grade Officers (31-40)
        [Description("مقدم")]
        [EnglishDescription("Lieutenant Colonel")]
        Muqaddam = 31,

        [Description("عقيد")]
        [EnglishDescription("Colonel")]
        Aqeed = 32,

        [Description("عميد")]
        [EnglishDescription("Brigadier General")]
        Ameed = 33,

        // General Officers (41-50)
        [Description("لواء")]
        [EnglishDescription("Major General")]
        Liwa = 41,

        [Description("فريق")]
        [EnglishDescription("Lieutenant General")]
        Fareq = 42,

        [Description("فريق أول")]
        [EnglishDescription("General")]
        FareqAwwal = 43,

        [Description("مشير")]
        [EnglishDescription("Field Marshal")]
        Masheer = 44
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EnglishDescriptionAttribute(string description) : Attribute
    {
        public string Description { get; set; } = description;
    }
    public static class RankArabicExtensions
    {
        public static string GetArabicName(this MilitaryRank rank)
        {
            var field = rank.GetType().GetField(rank.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute))!;
            return attribute?.Description ?? rank.ToString();
        }
        public static string GetEnglishName(this MilitaryRank rank)
        {
            var field = rank.GetType().GetField(rank.ToString());
            var attribute = (EnglishDescriptionAttribute)Attribute.GetCustomAttribute(field!, typeof(EnglishDescriptionAttribute))!;
            return attribute?.Description ?? rank.ToString();
        }

        /// <summary>
        /// Gets the level number of the rank
        /// </summary>
        public static int GetLevel(this MilitaryRank rank)
        {
            return (int)rank;
        }

        /// <summary>
        /// Checks if the rank is a civilian rank
        /// </summary>
        public static bool IsCivilian(this MilitaryRank rank)
        {
            return rank == MilitaryRank.Madani;
        }

        /// <summary>
        /// Checks if the rank is an enlisted rank
        /// </summary>
        public static bool IsEnlisted(this MilitaryRank rank)
        {
            int level = (int)rank;
            return level >= 1 && level <= 10;
        }

        /// <summary>
        /// Checks if the rank is a non-commissioned officer
        /// </summary>
        public static bool IsNCO(this MilitaryRank rank)
        {
            int level = (int)rank;
            return level >= 11 && level <= 20;
        }

        /// <summary>
        /// Checks if the rank is a commissioned officer
        /// </summary>
        public static bool IsOfficer(this MilitaryRank rank)
        {
            int level = (int)rank;
            return level >= 21;
        }

        /// <summary>
        /// Checks if the rank is a general officer
        /// </summary>
        public static bool IsGeneralOfficer(this MilitaryRank rank)
        {
            int level = (int)rank;
            return level >= 41;
        }
    }
}