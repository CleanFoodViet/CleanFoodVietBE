namespace CleanFoodVietAPI.Application.Utils
{
    public static class EnumUtil
    {
        //Get enum value
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        //Get enum name
        public static string GetEnumName<T>(int value)
        {
            return Enum.GetName(typeof(T), value)!;
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
