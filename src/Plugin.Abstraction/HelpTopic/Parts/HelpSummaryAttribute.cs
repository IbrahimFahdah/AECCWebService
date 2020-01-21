namespace Plugin.Abstraction.HelpTopic.Parts
{
    public class HelpSummaryAttribute : HelpPartAttribute
    {
        public HelpSummaryAttribute(string summary)
        {
            Summary = summary;
        }
        public override HelpPartType PartType => HelpPartType.Summary;
        public string Summary { get; set; }
    }
}