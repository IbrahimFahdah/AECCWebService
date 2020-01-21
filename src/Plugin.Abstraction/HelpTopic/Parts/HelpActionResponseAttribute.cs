using System;

namespace Plugin.Abstraction.HelpTopic.Parts
{
    public class HelpActionResponseAttribute : HelpRequestResponseAttribute
    {
        public HelpActionResponseAttribute(Type type) : base(type)
        {
        }
        public override HelpPartType PartType => HelpPartType.ActionResponse;
    }
}