using System;

namespace Plugin.Abstraction.HelpTopic.Parts
{
    public abstract class HelpRequestResponseAttribute : HelpPartAttribute
    {
        public HelpRequestResponseAttribute(Type type)
        {
            Type = type;
        }
        public Type Type { get; set; }
    }
}