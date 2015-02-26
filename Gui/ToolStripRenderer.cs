using System.ComponentModel;
using System.Windows.Forms;

namespace SeeSharp.Gui
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    class ToolStripProfessionalRendererNoSideLine : ToolStripProfessionalRenderer
    {
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs E)
        {
            // *** Do nothing, because I dislike that stupid little line
        }
    }
}
