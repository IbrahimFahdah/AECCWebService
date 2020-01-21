using System;

namespace Plugin.Abstraction.HelpTopic.Parts
{
    public abstract class HelpPartAttribute : Attribute
    {
        public abstract HelpPartType PartType { get;  }
    }
}