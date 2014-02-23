using System;
using System.Runtime.InteropServices;

namespace SeeSharp
{
    /// <summary>
    ///     Color struct.  Similar to System.Drawing.Color, but is designed for extremely fast draw and blend calls, as well as some application-specific functions
    /// </summary>
    /// <remarks>
    ///     This struct assumes little-endian packing, such as in x86 or x64
    /// </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Colour
    {
        // *** Members
        /// <summary>
        ///     32bppARGB colour value.  Memory-overlapped with individual channels
        /// </summary>
        [FieldOffset(0)] public UInt32 Color;
        /// <summary>
        ///     Blue channel
        /// </summary>
        [FieldOffset(0)] public byte B;
        /// <summary>
        ///     Green channel
        /// </summary>
        [FieldOffset(1)] public byte G;
        /// <summary>
        ///     Red channel
        /// </summary>
        [FieldOffset(2)] public byte R;
        /// <summary>
        ///     Alpha channel
        /// </summary>
        [FieldOffset(3)] public byte A;


        // *** Functions
        /// <summary>
        ///     Shade this colour towards black/white depending on altitude
        /// </summary>
        /// <param name="Altitude">
        ///     What block Y-level to shade this colour for 
        /// </param>
        /// <returns>
        ///     The Colour on which the function was called 
        /// </returns>
        /// <remarks>
        ///     This function should not be called on the colours retrieved from the palette, as it will corrupt them 
        /// </remarks>
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
        /// <summary>
        ///     Returns a new instance of this colour
        /// </summary>
        /// <returns></returns>
        public Colour Copy()
        {
            return new Colour { Color = Color };
        }
        /// <summary>
        ///     Copies the current colour and shades the new instance according to the block's light level
        /// </summary>
        /// <param name="LightLevel">
        ///     Light level from 0 to 15. 
        /// </param>
        /// <returns>
        ///     A fresh Colour struct 
        /// </returns>
        public Colour LightLevel(uint LightLevel)
        {
            return Blend(new Colour { Color = Color & 0xFF000000 }, (15 - LightLevel) << 3);
        }
        /// <summary>
        ///     Blend another colour onto this one, storing the result into this colour
        /// </summary>
        /// <param name="Top">
        ///     What colour to blend on top of this one
        /// </param>
        /// <returns>
        ///     This colour 
        /// </returns>
        /// <remarks>
        ///     The final alpha is set to the blended colour's alpha
        /// </remarks>
        public Colour Blend(Colour Top)
        {
            Color = (Top.Color & 0xFF000000) | // Alpha
                    (((Color & 0x00FF00FFU) + ((((Top.Color & 0x00FF00FFU) - (Color & 0x00FF00FFU)) * (UInt32)Top.A + 0x00800080U) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + ((((Top.Color & 0x0000FF00U) - (Color & 0x0000FF00U)) * (UInt32)Top.A + 0x00008000U) >> 8)) & 0x0000FF00U); // Green
            return this;
        }
        /// <summary>
        ///     Blend another colour onto this one with the specified alpha. storing the result into this colour
        /// </summary>
        /// <param name="Top">
        ///     What colour to blend on top of this one
        /// </param>
        /// <returns>
        ///     This colour 
        /// </returns>
        /// <remarks>
        ///  The final alpha is set to UseAlpha
        /// </remarks>
        public Colour Blend(Colour Top, UInt32 UseAlpha)
        {
            Color = (UseAlpha << 24) | // Alpha
                    (((Color & 0x00FF00FFU) + ((((Top.Color & 0x00FF00FFU) - (Color & 0x00FF00FFU)) * UseAlpha + 0x00800080U) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + ((((Top.Color & 0x0000FF00U) - (Color & 0x0000FF00U)) * UseAlpha + 0x00008000U) >> 8)) & 0x0000FF00U); // Green
            return this;
        }
        /// <summary>
        /// Set this colour to full alpha
        /// </summary>
        /// <returns></returns>
        public Colour FullAlpha()
        {
            Color |= 0xFF000000;
            return this;
        }


        // *** Colour Constants
        /// <summary>
        ///     Opaque black
        /// </summary>
        public static Colour Black
        {
            get
            {
                return new Colour { Color = 0xFF000000 };
            }
        }
        /// <summary>
        ///  Opaque White
        /// </summary>
        public static Colour White
        {
            get
            {
                return new Colour { Color = 0xFFFFFFFF };
            }
        }
        /// <summary>
        ///     Transparent Black
        /// </summary>
        public static Colour Transparent
        {
            get
            {
                return new Colour { Color = 0x00000000U };
            }
        }
    }
}
