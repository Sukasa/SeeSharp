using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using Substrate;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;


namespace SeeSharp
{
    //Basic set of extension methods used in places.
    public static class SeeSharpExtensions
    {
        // If there is no tile entity, Substrate can throw an exception.  This utility function just handles that gracefully
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
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="code"></param>
        public static void UIThread(this Control @this, Action code)
        {
            if (@this.InvokeRequired)
            {
                @this.BeginInvoke(code);
            }
            else
            {
                code.Invoke();
            }
        }

        public static bool ContainsPoint(this Rectangle @this, int x, int y)
        {
            return (x >= @this.Left && x < @this.Right && y >= @this.Top && y < @this.Bottom);
        }

        public static IEnumerable<Control> GetSubControls(this Control Root)
        {
            IEnumerable<Control> Controls = Root.Controls.Cast<Control>();

            return Controls.SelectMany(X => X.GetSubControls())
                                      .Concat(Controls);
        }

        public static string ConvertToRelativePath(this string FilePath)
        {
            String RootRef = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Uri pathUri = new Uri(FilePath);

            // Folders must end in a slash
            if (!RootRef.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                RootRef += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(RootRef);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
    } 
}
