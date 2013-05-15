using System.Text;

namespace Dongle.System.IO
{
    public static class DongleEncoding
    {
        public static Encoding Default
        {
            get
            {
                return Encoding.GetEncoding(1252);
            }
        }
    }
}