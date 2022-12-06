namespace Eliot.Utilities
{
    public static class StringExtensions
    {
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <returns>Left side from index.</returns>
        public static string Left(this string s, int index)
        {
            return s.Substring(0, index);
        }

        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <returns>Mid side from index.</returns>
        public static string Mid(this string s, int index)
        {
            return s.Substring(index, s.Length - index);
        }

        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Right(this string s, int index)
        {
            return s.Substring(0, s.Length - index);
        }
    }
}
