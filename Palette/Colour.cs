using System;
using System.Runtime.InteropServices;

// ReSharper disable RedundantCast
// *** Because I've found a weird issue where (byte) + (value over 255) comes out to (invalid value).

namespace SeeSharp.Palette
{
    /// <summary>
    ///     Color struct.  Similar to System.Drawing.Color, but is designed for extremely fast draw and blend calls, as well as some application-specific functions
    /// </summary>
    /// <remarks>
    ///     The Colour structure wraps a speed-oriented 32bppARGB colour value.  It supports per-channel and as-whole addressing, and basic blend functions.  This struct assumes little-endian packing, such as in x86 or x64
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

        /*
         *  These functions all include repeated code, specifically the integer blending.  This is on purpose; the overhead of the function call forms more than 50% of the
         *  total consumed CPU cycles for Blend()! 
         */

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
        ///     The Altitude() function shades a Colour towards white or black depending on the altitiude, expressed as a number of 0 to 255.  This function should not be called on the colours retrieved from the palette, as it will corrupt them.  Use <see cref="Colour.Copy"/> to create a new Colour first.
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
        /// <remarks>
        ///     Copy() allocates a new Colour struct and sets its values to the same values as this Colour.
        /// </remarks>
        /// <returns>
        ///     A fresh copy of the struct
        /// </returns>
        public Colour Copy()
        {
            return new Colour { Color = Color };
        }
        /// <summary>
        ///     Copies the current colour and shades the new instance according to the block's light level
        /// </summary>
        /// <remarks>
        ///     LightLevel() returns a copy of this Colour, and shades it towards black depending on the supplied light level.
        /// </remarks>
        /// <param name="LightLevel">
        ///     Light level from 0 to 15. 
        /// </param>
        /// <returns>
        ///     A fresh Colour struct 
        /// </returns>
        public Colour LightLevel(uint LightLevel)
        {
            UInt32 UseAlpha = (15 - LightLevel) << 3;
            Color = (Color & 0xFF000000) | // *** Alpha
                    (((Color & 0x00FF00FFU) + (((0x00800080U - (Color & 0x00FF00FFU)) * UseAlpha) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + (((0x00008000U - (Color & 0x0000FF00U)) * UseAlpha) >> 8)) & 0x0000FF00U); // Green
            return this;
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
        ///     Blend another colour onto this one, storing the result into this colour.  The final alpha is set to the blended colour's alpha value.
        /// </remarks>
        public Colour Blend(Colour Top)
        {
            Color = (Top.Color & 0xFF000000) | // *** Alpha
                    (((Color & 0x00FF00FFU) + ((((Top.Color & 0x00FF00FFU) - (Color & 0x00FF00FFU)) * (UInt32)Top.A + 0x00800080U) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + ((((Top.Color & 0x0000FF00U) - (Color & 0x0000FF00U)) * (UInt32)Top.A + 0x00008000U) >> 8)) & 0x0000FF00U); // Green
            return this;
        }
        /// <summary>
        ///     Blend another colour onto this one with the specified alpha. storing the result into this colour
        /// </summary>
        /// <param name="Top">
        ///     What colour to blend on top of this one.  Alpha is ignored for blending, but returned as part of the resulting colour.
        /// </param>
        /// <param name="UseAlpha">
        ///     What alpha value to use for the source colour
        /// </param>
        /// <returns>
        ///     This colour
        /// </returns>
        /// <remarks>
        ///     Blend another colour onto this one with a supplied alpha value, storing the result into this colour.  The final alpha is set to the blended colour's alpha
        /// </remarks>
        public Colour Blend(Colour Top, UInt32 UseAlpha)
        {
            Color = (Top.Color & 0xFF000000) | // *** Alpha
                    (((Color & 0x00FF00FFU) + ((((Top.Color & 0x00FF00FFU) - (Color & 0x00FF00FFU)) * UseAlpha + 0x00800080U) >> 8)) & 0x00FF00FFU) | // Red, Blue
                    (((Color & 0x0000FF00U) + ((((Top.Color & 0x0000FF00U) - (Color & 0x0000FF00U)) * UseAlpha + 0x00008000U) >> 8)) & 0x0000FF00U); // Green
            return this;
        }
        /// <summary>
        ///     Set this Colour to full alpha
        /// </summary>
        /// <remarks>
        ///     Sets this Colour to full alpha and returns it.
        /// </remarks>
        /// <returns>
        ///     This Colour
        /// </returns>
        public Colour FullAlpha()
        {
            Color |= 0xFF000000U;
            return this;
        }
        /// <summary>
        ///     Whether the colour value is an entity key
        /// </summary>
        /// <returns></returns>
        public bool IsEntityKey()
        {
            return (Color & 0xFFFF0000U) == 0x00FF0000U;
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
