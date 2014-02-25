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
        ///     The ConfigStrings property.  This must return or accept (and apply) an IEnumerable of string/string key-value pairs, representing the command-line format key/value pairs for the config options presented on the form.
        /// </summary>
        /// <remarks>
        ///     The meanings of each key and its potential values is dependent upon and unique to each renderer. 
        /// </remarks>
        public abstract List<KeyValuePair<String, String>> ConfigStrings { get; set; }
    }
}
