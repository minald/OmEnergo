namespace OmEnergo.Models
{
    public static class Extensions
    {
        public static string ToStringInRussian(this bool value)
        {
            return value ? "Есть" : "Нет";
        }
    }
}
