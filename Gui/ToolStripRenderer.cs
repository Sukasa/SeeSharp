using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeeSharp.Gui
{
    class ToolStripProfessionalRendererNoSideLine : ToolStripProfessionalRenderer
    {
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // *** Do nothing, because I dislike that stupid little line
        }
    }
}
