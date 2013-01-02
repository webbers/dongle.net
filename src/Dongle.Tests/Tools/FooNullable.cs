using System.Data.SqlTypes;

namespace Dongle.Tests.Tools
{
    class FooNullable : INullable
    {
        public bool IsNull
        {
            get
            {
                return true;
            }
        }
    }
}