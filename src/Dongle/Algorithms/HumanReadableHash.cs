using System.Text;

namespace Dongle.Algorithms
{
    public static class HumanReadableHash
    {
        private static readonly char[] Chars = new[]
                                                   {
                                                       '0', '1', '2', '3', '4', '5', '6', '7',
                                                       '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
                                                       'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                                       'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V'
                                                   };

        public static string Compute(string str, Encoding encoding)
        {
            encoding = encoding ?? Encoding.ASCII;
            var crc32 = Crc32.Compute(encoding.GetBytes(str));
            var builder = new StringBuilder();
            for (var i = 0; i < 6; i++)
            {
                builder.Append(Chars[(crc32 & (31 << (5 * i))) >> (5 * i)]);
            }
            return builder.ToString();
        }
    }
}
