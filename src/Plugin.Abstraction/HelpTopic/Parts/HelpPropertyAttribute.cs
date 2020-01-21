using System;

namespace Plugin.Abstraction.HelpTopic.Parts
{
    public class HelpPropertyAttribute : HelpPartAttribute
    {
        public HelpPropertyAttribute(string summary, Type[] relatedTypes= null)
        {
            Summary = summary;
            RelatedTypes = relatedTypes;
        }
        public override HelpPartType PartType => HelpPartType.Property;
        public string Name { get; set; }
        public string Summary { get; set; }
        public Type[] RelatedTypes { get; set; }
        public Type PropertyType { get; set; }
    }


}