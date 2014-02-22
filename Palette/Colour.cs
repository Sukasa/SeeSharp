using System;
using System.Runtime.InteropServices;

namespace SeeSharp
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Colour
    {
        // *** Members
        [FieldOffset(0)] public UInt32 Color;
        [FieldOffset(0)] public byte B;
        [FieldOffset(1)] public byte G;
        [FieldOffset(2)] public byte R;
        [FieldOffset(3)] public byte A;


        // *** Functions
        public Colour Altitude(int Altitude)
        {
            UInt32 Blend = Altitude > 127 ? 0xFFFFFFFF : 0x00000000U;
            UInt32 Amount = (UInt32)(Math.Abs(Altitude - 128));
            if ((Altitude & 1) == 1)
                Amount += 5;

            Color = (Color & 0xFF000000) | // Alpha
                    (((Color & 0x00FF00FFU) + ((((Blend & 0x00FF00FFU) - (Color & 0x00FF00FFU)) * Amount + 0x00800080U) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + ((((Blend & 0x0000FF00U) - (Color & 0x0000FF00U)) * Amount + 0x00008000U) >> 8)) & 0x0000FF00U); // Green

            return this;
        }
        public Colour LightLevel(uint LightLevel)
        {
            return Blend(new Colour { Color = Color & 0xFF000000 }, (15 - LightLevel) << 3);
        }
        public Colour Blend(Colour Top)
        {
            // It takes twice as long to call the other Blend() as it does to simply calculate it in this function by repeating the code.
            // No joke.
            Color = (Top.Color & 0xFF000000) | // Alpha
                    (((Color & 0x00FF00FFU) + ((((Top.Color & 0x00FF00FFU) - (Color & 0x00FF00FFU)) * (UInt32)Top.A + 0x00800080U) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + ((((Top.Color & 0x0000FF00U) - (Color & 0x0000FF00U)) * (UInt32)Top.A + 0x00008000U) >> 8)) & 0x0000FF00U); // Green
            return this;
        }
        public Colour Blend(Colour Top, UInt32 UseAlpha)
        {
            Color = (Top.Color & 0xFF000000) | // Alpha
                    (((Color & 0x00FF00FFU) + ((((Top.Color & 0x00FF00FFU) - (Color & 0x00FF00FFU)) * UseAlpha + 0x00800080U) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + ((((Top.Color & 0x0000FF00U) - (Color & 0x0000FF00U)) * UseAlpha + 0x00008000U) >> 8)) & 0x0000FF00U); // Green
            return this;
        }
        public Colour FullAlpha()
        {
            Color |= 0xFF000000;
            return this;
        }


        // *** Colour Constants
        public static Colour Black
        {
            get
            {
                return new Colour { Color = 0xFF000000 };
            }
        }
        public static Colour White
        {
            get
            {
                return new Colour { Color = 0xFFFFFFFF };
            }
        }
        public static Colour Transparent
        {
            get
            {
                return new Colour { Color = 0x00000000U };
            }
        }
    }
}
