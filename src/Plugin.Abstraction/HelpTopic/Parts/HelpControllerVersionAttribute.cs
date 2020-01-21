namespace Plugin.Abstraction.HelpTopic.Parts
{
    public class HelpControllerVersionAttribute : HelpPartAttribute
    {
        public HelpControllerVersionAttribute(string version,string lastUpdated, string releaseHistory)
        {
            Version = version;
            LastUpdated = lastUpdated;
            ReleaseHisotry = releaseHistory;
        }
        public override HelpPartType PartType => HelpPartType.ControllerVersion;

        public string Version { get; set; }

        public string LastUpdated { get; set; }

        public string ReleaseHisotry { get; set; }
    }
}