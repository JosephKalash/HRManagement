namespace HRManagement.Application.Helpers
{
    public static class ObjectExtensions
    {
        public static void SetIfNotNull<T>(this T? value, Action<T> setter) where T : class
        {
            if (value != null)
                setter(value);
        }

        public static void SetIfHasValue<T>(this T? value, Action<T> setter) where T : struct
        {
            if (value.HasValue)
                setter(value.Value);
        }
    }
}
