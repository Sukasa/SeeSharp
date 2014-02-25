using System.Collections.Generic;

namespace SeeSharp
{
    /// <summary>
    ///     Mini palette entry.  Contains a minimum amount of data, for fast access during rendering
    /// </summary>
    public struct MiniPaletteEntry
    {
        /// <summary>
        ///     The palette colour to use for this biome/block/metadata combo
        /// </summary>
        public Colour Colour;
        // TODO: I want to work on this, see if I can reduce it to an Int32 key instead.  That would reduce the struct size from 12 bytes (64-bit pointer) to 8 bytes on x64, which will help caching and data size.
        /// <summary>
        ///     If this block has specific entity values to check, this list will be non-null
        /// </summary>
        public List<PaletteEntry> EntityColours;
    }
}
