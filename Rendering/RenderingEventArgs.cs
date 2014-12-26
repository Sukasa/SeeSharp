using System;

namespace SeeSharp.Rendering
{
    /// <summary>
    ///     Progress update event arguments
    /// </summary>
    /// <remarks>
    ///     The ProgressUpdate event is raised periodically by the renderer to update the displayed progress of the render process.
    /// </remarks>
    public class ProgressUpdateEventArgs : EventArgs
    {
        /// <summary>
        ///     The render progress, or render step progress, expressed as a value from 0.0 to 1.0
        /// </summary>
        public Single Progress;

        /// <summary>
        ///     A long-form description of the current render step and progress
        /// </summary>
        /// <remarks>
        ///     The long-form progress description is displayed in the command-line format of the app, as well as in the tooltip for the short-form description. 
        /// </remarks>
        public String ProgressDescription;

        /// <summary>
        ///     A shortened summary of the current render step
        /// </summary>
        /// <remarks>
        ///     The short progress description is displayed in the status bar of the main rendering gui
        /// </remarks>
        public String ProgressShortDescription;
    }

    /// <summary>
    ///     The arguments provided in the event of a rendering error
    /// </summary>
    /// <remarks>
    ///     A RenderingError event is raised when the renderer encounters a rendering error that may affect the render process.
    /// </remarks>
    public class RenderingErrorEventArgs : EventArgs
    {
        /// <summary>
        ///     The exception, if it exists, that caused this error to be raised.  Must be provided if <see cref="ShowInnerException"/> is true.  
        /// </summary>
        /// <remarks>
        ///     If a specific error was caused by an exception, returning it via ErrorException and setting <see cref="RenderingErrorEventArgs.ShowInnerException"/> to <see langword="true"/> will display the error message to the user
        /// </remarks>
        public Exception ErrorException;

        /// <summary>
        ///     What error message to display to the user when notifying them of the error.
        /// </summary>
        /// <remarks>
        ///     This sets the descriptive error message displayed to the user when the error is displayed.  Use it to describe the error to the user, as well as any potential consequences.
        /// </remarks>
        public String UserErrorMessage;

        /// <summary>
        ///     Ifhen the error is fatal and should result in an abort.
        /// </summary>
        /// <remarks>
        ///     IsFatal should be set to  <see langword="true"/> when the renderer is raising a fatal error.  This flag tells the error handler that the error is fatal and non-recoverable, which in turn will notify the user and automatically abort the render.  If you set this flag, expect to have Abort() called.
        /// </remarks>
        public Boolean IsFatal;

        /// <summary>
        ///     If true, the Message  parameter of ErrorException  will be used
        /// </summary>
        public Boolean ShowInnerException;

        /// <summary>
        ///     The renderer-designed error code for this error.
        /// </summary>
        /// <remarks>
        ///     The numeric identifier for the specific kind of error the renderer is raising.  It is important  to not reuse error codes between different error types, as the standard gui uses the code to allow the user to ignore specific errors and continue.
        /// </remarks>
        public Int32 ErrorCode;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="Sender">
    ///     The renderer throwing the error 
    /// </param>
    /// <param name="E">
    ///     The progress  
    /// </param>
    public delegate void ProgressUpdateHandler(object Sender, ProgressUpdateEventArgs E);
    public delegate void RenderingErrorHandler(object Sender, RenderingErrorEventArgs E);
}
