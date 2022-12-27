namespace CompanyInfoProducer
{
    public static class ExtensionMethods
    {
        public static int? AsInt(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;
            return null;
        }
    }
}