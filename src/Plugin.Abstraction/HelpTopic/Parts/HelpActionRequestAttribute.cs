using System;

namespace Plugin.Abstraction.HelpTopic.Parts
{
    public class HelpActionRequestAttribute : HelpRequestResponseAttribute
    {
        public HelpActionRequestAttribute(Type type):base(type)
        {
        }
        public override HelpPartType PartType => HelpPartType.ActionRequest;
    }
}