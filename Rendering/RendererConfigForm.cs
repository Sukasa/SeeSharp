using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SeeSharp.Rendering
{
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<RendererConfigForm, Form>))]
    public abstract class RendererConfigForm : Form
    {
        /// <summary>
        ///     The ConfigStrings property.  This must return or accept (and apply) an IEnumerable of string-string tuples, representing the command-line format key/value pairs for the config options presented on the form.
        /// </summary>
        public abstract List<Tuple<String, String>> ConfigStrings { get; set; }
    }
}
