using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SeeSharp.Rendering
{
    /// <summary>
    ///     Renderer advanced settings base form class
    /// </summary>
    /// <remarks>
    ///     The RendererConfigForm class provides a base abstract form class that allows you to define your own custom configuration form, for example the normal/cave rendering option for the Standard Renderer.  It contains one property that must be implemented by your code.
    /// </remarks>
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<RendererConfigForm, Form>))]
    public abstract class RendererConfigForm : Form
    {
        /// <summary>
        ///     The ConfigStrings property.  This must return or accept (and apply) an IEnumerable of string/string key-value pairs, representing the command-line format key/value pairs for the config options presented on the form.
        /// </summary>
        /// <remarks>
        ///    The ConfigStrings property provides a way for See Sharp to read and write custom settings for your renderer.  The meanings of each key and its potential values is dependent upon and unique to your renderer.
        /// </remarks>
        public abstract List<KeyValuePair<String, String>> ConfigStrings { get; set; }
    }
}
