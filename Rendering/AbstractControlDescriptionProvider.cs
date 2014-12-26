using System;
using System.ComponentModel;

// I didn't write this.  Apparently I goofed and forgot to credit the original author.  This is used so that the RendererConfigForm class renders properly in the designer for plugin authors
namespace SeeSharp.Rendering
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type ObjectType, object Instance)
        {
            if (ObjectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(ObjectType, Instance);
        }

        public override object CreateInstance(IServiceProvider Provider, Type ObjectType, Type[] ArgTypes, object[] Args)
        {
            if (ObjectType == typeof(TAbstract))
                ObjectType = typeof(TBase);

            return base.CreateInstance(Provider, ObjectType, ArgTypes, Args);
        }
    }
}
