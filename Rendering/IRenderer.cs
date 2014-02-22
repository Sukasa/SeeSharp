using Substrate;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeeSharp.Rendering
{
    public interface IRenderer
    {
        // *** Functions
        /// <summary>
        ///     Configure the renderer before rendering
        /// </summary>
        /// <param name="Config">
        ///     Render configuration as selected by user
        /// </param>
        void Configure(RenderConfiguration Configuration);

        /// <summary>
        ///     Initialize the renderer, for example allocate bitmap objects or allocate data structures.  Called once, prior to rendering or previewing.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Begin rendering.
        /// </summary>
        /// <remarks>
        ///     <see cref="ProgressUpdate"/> should be raised periodically
        /// </remarks>
        /// <seealso cref="ProgressUpdateHandler">
        void Render();

        /// <summary>
        ///     Called if the renderer should stop rendering.  An abort implies immediate stoppage of rendering
        /// </summary>
        /// <remarks>
        ///     The renderer should release all self-allocated objects, but disk cleanup (i.e. for multi-file renders) need not be performed
        /// </remarks>
        void Abort();

        /// <summary>
        ///     If called, should produce a small preview image to be used in the GUI.
        /// </summary>
        /// <returns>
        ///     A 192x192px 32bppARGB Bitmap representing the preview
        /// </returns>
        Bitmap Preview();


        // *** Advanced Configuration
        /// <summary>
        ///     Configuration form for any advanced settings
        /// </summary>
        /// <remarks>
        ///     If you have no advanced settings, you can return null for a default "no advanced settings" dialog. 
        /// </remarks>
        RendererConfigForm ConfigurationForm { get; }

        /// <summary>
        ///     Called if the renderer should print help info to console
        /// </summary>
        void PrintHelpInfo();

        // *** Renderer identification and status
        /// <summary>
        ///     Renderer name.  Used in the CLI interface, e.g. "-Renderer MyRenderer"
        /// </summary>
        String RendererName { get; }

        /// <summary>
        ///     Renderer friendly name.  Used in the GUI.
        /// </summary>
        String RendererFriendlyName { get; }

        /// <summary>
        ///     Whether the renderer is aborting.
        /// </summary>
        /// <remarks>
        ///     This property should return true if Abort() is called, or if the renderer self-initiates an abort. 
        /// </remarks>
        Boolean IsAborting { get; }


        // *** Events
        /// <summary>
        ///     Raise this event to update the user on current render progress.  Failing to call this will leave your user in the dark!
        /// </summary>
        event ProgressUpdateHandler ProgressUpdate;

        /// <summary>
        ///     Raise this event in the event of an error during rendering, whether fatal or not.
        /// </summary>
        event RenderingErrorHandler RenderError;
    }
}
