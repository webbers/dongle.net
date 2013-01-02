using System;

namespace Dongle.Serialization.Attributes
{
    public class IgnoreAttribute : Attribute
    {

    }

    public class FixedWidthAttribute : Attribute
    {
        public int Width { get; set; }

        public FixedWidthAttribute(int width)
        {
            Width = width;
        }
    }
}