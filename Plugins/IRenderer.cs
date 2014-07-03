using System;
using System.Drawing;
using SeeSharp.Rendering;

namespace SeeSharp.Plugins
{
    /// <summary>
    ///     Renderer interface used by See Sharp to manage rendering operations
    /// </summary>
    /// <remarks>
    ///     The IRenderer interface defines the set of functions, properties, and events used to manage rendering.  Any renderer that implements this interface as a class library will be automatically loaded by See Sharp if the DLL is placed in the application folder or a subfolder.
    /// </remarks>
    public interface IRenderer : IComponent
    {

        #region Setup Functions

        /// <summary>
        ///     Configure the renderer before rendering
        /// </summary>
        /// <remarks>
        ///     Configure() will provide the renderer with the user's specified configuration prior to rendering.  It does not constitute an initialization.
        /// </remarks>
        /// <param name="Config">
        ///     Render configuration as selected by user
        /// </param>
        void Configure(RenderConfiguration Configuration);

        /// <summary>
        ///     Initialize the renderer, for example allocate bitmap objects or allocate data structures.  Called once, prior to rendering or previewing.
        /// </summary>
        /// <remarks>
        ///     Initialize() should prep the renderer for either previewing or rendering.  It will be called after Configure().  It should be noted that it is possible for Initialize() to be called, and then an immediate abort() instead of rendering or previewing.
        /// </remarks>
        void Initialize();

        #endregion

        #region Rendering Functions

        /// <summary>
        ///     Render the world to a map file, e.g. a .png
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Render() is called when the renderer has been initialized, and should properly render a full map file for the world.  Rendering may be either multi-threaded or single-threaded.  <see cref="ProgressUpdate"/> should be raised periodically.
        ///     </para>
        ///     <para>
        ///         In the case of GUI-based rendering, the renderer is shifted to a background thread automatically, so there is no need to handle GUI events or make provision for them in renderer code.    However, the renderer's internal execution model should respect the threading configuration provided where possible.
        ///         Additionally, the render should be abortable at any time via <see cref="Abort"/>.
        ///     </para>
        ///     <para>
        ///         You should not throw exceptions from this function.  Raise <see cref="RenderError"/> with <code>IsFatal</code> set to <see langword="true"/>.
        ///     </para>
        /// </remarks>
        /// <seealso cref="ProgressUpdateHandler">
        void Render();

        /// <summary>
        ///     Called if the renderer should stop rendering.  An abort implies immediate stoppage of rendering
        /// </summary>
        /// <remarks>
        ///     Abort() is called when the user aborts a render, or when The renderer should release all self-allocated objects, but disk cleanup (i.e. for multi-file-output renders) need not be performed
        /// </remarks>
        void Abort();

        /// <summary>
        ///     Produce a small preview image to be used in the GUI.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <code>Preview()</code> is called when the renderer should produce a small 192x192px preview image for the gui, as opposed to a full render.
        ///         When implemented, Preview() should complete as quickly as possible while still providing a clear representation of the final render
        ///     </para>
        ///     <para>
        ///         You should not throw exceptions from this function.  Raise <see cref="RenderError"/> with <code>IsFatal</code> set to <see langword="true"/>.
        ///     </para>
        /// </remarks>
        /// <seealso cref="Render"/>
        /// <returns>
        ///     A 192x192px 32bppARGB Bitmap representing the preview
        /// </returns>
        Bitmap Preview();

        #endregion

        #region Renderer Identification

        /// <summary>
        ///     Renderer name.  Used in the CLI interface, e.g. "-Renderer MyRenderer"
        /// </summary>
        String RendererName { get; }

        /// <summary>
        ///     Renderer friendly name.  Used in the GUI.
        /// </summary>
        String RendererFriendlyName { get; }

        #endregion

        #region Event Definitions

        /// <summary>
        ///     Raise this event to update the user on current render progress.  Failing to call this will leave your user in the dark!
        /// </summary>
        event ProgressUpdateHandler ProgressUpdate;

        /// <summary>
        ///     Raise this event in the event of an error during rendering, whether fatal or not.
        /// </summary>
        event RenderingErrorHandler RenderError;

        #endregion


        /// <summary>
        ///     Configuration form for any advanced settings
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The ConfigurationForm property is used so the renderer can specify a basic windows form with any renderer-specific configuration settings not present on the main gui.
        ///         The form must inherit from <see cref="RendererConfigForm"/>, so that the requisite property needed to support the gui is defined.
        ///     </para>
        ///     <para>
        ///         If you have no advanced settings, you can return <see langword="null"/> for a default "no advanced settings" dialog. 
        ///     </para>
        /// </remarks>
        RendererConfigForm ConfigurationForm { get; }

        /// <summary>
        ///     Called if the renderer should print help info to console
        /// </summary>
        void PrintHelpInfo();

        /// <summary>
        ///     Whether the renderer is aborting.
        /// </summary>
        /// <remarks>
        ///     This property should return true if Abort() is called, or if the renderer self-initiates an abort. 
        /// </remarks>
        Boolean IsAborting { get; }

    }
}
