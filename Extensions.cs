using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Substrate;


namespace SeeSharp
{
    // *** Basic set of extension methods used in places.
    /// <summary>
    ///     Extension methods included in See Sharp
    /// </summary>
    public static class SeeSharpExtensions
    {
        // *** If there is no tile entity, Substrate can throw an exception.  This utility function just handles that gracefully
        /// <summary>
        ///     Get the tile entity for a block, without throwing an exception if none exists
        /// </summary>
        /// <remarks>
        ///     Substrate's library throws an exception if you try to retrieve a tile entity that does not exist.  This utility function handles this, swallowing the error and merely returning null instead of crashing.
        /// </remarks>
        /// <param name="Blocks">
        ///     The block collection to get the tile entity from
        /// </param>
        /// <param name="X">
        ///     The X position of the block to get the tile entity for
        /// </param>
        /// <param name="Y">
        ///     The Y position of the block to get the tile entity for
        /// </param>
        /// <param name="Z">
        ///     The Z position of the block to get the tile entity for
        /// </param>
        /// <returns>
        ///     The tile entity for the block, or <see langword="null"/>
        /// </returns>
        /// <seealso cref=" Substrate.TileEntity"/>
        public static TileEntity SafeGetTileEntity(this AlphaBlockCollection Blocks, int X, int Y, int Z)
        {
            try
            {
                return Blocks.GetTileEntity(X, Y, Z);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <remarks>
        ///     When calls, invokes the supplied <see cref="System.Action"/> on the control's UI thread
        /// </remarks>
        /// <param name="Code">
        ///     The <see cref="System.Action"/> to execute
        /// </param>
        /// <param name="This">
        ///     The Control to execute the <see cref="System.Action"/>  on
        /// </param>
        public static void UIThread(this Control This, Action Code)
        {
            if (This.InvokeRequired)
            {
                This.BeginInvoke(Code);
            }
            else
            {
                Code.Invoke();
            }
        }

        /// <summary>
        ///     Whether an X/Y point intersects this rectangle
        /// </summary>
        /// <param name="This">
        ///     Rectangle to compare the point against
        /// </param>
        /// <param name="X">
        ///     X value of the point to check
        /// </param>
        /// <param name="Y">
        ///     Y value of the point to check
        /// </param>
        /// <returns>
        ///     Whether the X/Y point is contained within the rectangle inclusively
        /// </returns>
        public static bool ContainsPoint(this Rectangle This, int X, int Y)
        {
            return (X >= This.Left && X < This.Right && Y >= This.Top && Y < This.Bottom);
        }

        /// <summary>
        ///     Gets all the sub-controls of a given control, recursively
        /// </summary>
        /// <param name="Root">
        ///     The control to get all subcontrols of
        /// </param>
        /// <returns>
        ///     An Enumerable list of all controls contained by or within the root control
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<Control> GetSubControls(this Control Root)
        {
            IEnumerable<Control> Controls = Root.Controls.Cast<Control>();

            return Controls.SelectMany(x => x.GetSubControls())
                                      .Concat(Controls);
        }

        /// <summary>
        ///     Converts a string representation of a file path to its path relative to See Sharp's executable
        /// </summary>
        /// <param name="FilePath">
        ///     The file path to convert
        /// </param>
        /// <returns>
        ///     The relative form of the file path, with the file path of See Sharp's executable as reference.
        /// </returns>
        public static string ConvertToRelativePath(this string FilePath)
        {
            String RootRef = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Uri PathUri = new Uri(FilePath);

            if (RootRef == null)
                return string.Empty;

            // *** Folders must end in a slash
            if (!RootRef.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                RootRef += Path.DirectorySeparatorChar;
            }
            Uri FolderUri = new Uri(RootRef);
            return Uri.UnescapeDataString(FolderUri.MakeRelativeUri(PathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static T[] GetRange<T>(this T[] Array, int StartingOffset, int Range, int Stride)
        {
            // *** Get center point
            int X = StartingOffset % Stride;
            int Y = StartingOffset / Stride;

            // *** Get bounds of range
            int MinX = Math.Max(0, X - Range);
            int MinY = Math.Max(0, Y - Range);
            int MaxX = Math.Min(Stride - 1, X + Range);
            int MaxY = Math.Min((Array.Length / Stride) - 1, Y + Range);

            // *** Define output table + write pointer
            T[] Output = new T[(MaxX - MinX + 1) * (MaxY - MinY + 1)];
            int Ptr = 0;

            // *** Copy data into range output
            for (int y = MinY; y <= MaxY; y++)
            {
                for (int x = MinX; x <= MaxX; X++)
                {
                    Output[Ptr] = Array[(y * Stride) + x];
                    Ptr++;
                }
            }

            return Output;
        }

    } 
}
