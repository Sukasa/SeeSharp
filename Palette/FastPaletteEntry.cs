using System.Collections.Generic;
using System;

namespace SeeSharp
{
    /// <summary>
    ///     Mini palette entry.  Contains a minimum amount of data, for fast access during rendering
    /// </summary>
    /// <remarks>
    ///     The MiniPaletteEntry struct defines the basic data stored in fast-lookup arrays
    /// </remarks>
    public struct MiniPaletteEntry
    {
        /// <summary>
        ///     The palette colour to use for this biome/block/metadata combo
        /// </summary>
        /// <remarks>
        ///     The Colour param is the base colour modified by any applicable tints for that Biome/Block/Metadata
        /// </remarks>
        public Colour Colour;

        // TODO: I want to work on this, see if I can reduce it to an Int32 key instead.  That would reduce the struct size from 12 bytes (64-bit pointer) to 8 bytes on x64, which will help caching and data size.
        /// <summary>
        ///     List of all entity-tag block palette entries, or null if none exist
        /// </summary>
        /// <remarks>
        ///     Some blocks might have "Entity-Tagged" block entries; these entries are available in a list for access.  The list will be null if no such block palette entries exist.
        /// </remarks>
        public Int32 EntityColours;
    }
}
