namespace BoschCartaoDigitalBackEnd.Extentions
{
    public static class StringExtentions
    {
        public static string NormalizarString(this string texto)
        {
            return texto.ToUpper().Trim();
        }
    }
}