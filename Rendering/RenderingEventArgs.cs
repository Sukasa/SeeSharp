using System;

namespace SeeSharp.Rendering
{
    public class ProgressUpdateEventArgs : EventArgs
    {
        public Single Progress;
        public String ProgressDescription;
        public String ProgressShortDescription;
    }

    public class RenderingErrorEventArgs : EventArgs
    {
        public Exception ErrorException;
        public String UserErrorMessage;
        public Boolean IsFatal;
        public Boolean ShowInnerException;
        public Int32 ErrorCode;
    }


    public delegate void ProgressUpdateHandler(object sender, ProgressUpdateEventArgs e);
    public delegate void RenderingErrorHandler(object sender, RenderingErrorEventArgs e);
}
