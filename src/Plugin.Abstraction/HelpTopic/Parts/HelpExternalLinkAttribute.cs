using System;

namespace Plugin.Abstraction.HelpTopic.Parts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HelpExternalLinkAttribute : HelpPartAttribute
    {
        public HelpExternalLinkAttribute(string htmlLinkDetails)
        {
            HtmlLinkDetails = htmlLinkDetails;
        }
        public override HelpPartType PartType => HelpPartType.ExternalLink;
        public string HtmlLinkDetails { get; set; }
    }
}