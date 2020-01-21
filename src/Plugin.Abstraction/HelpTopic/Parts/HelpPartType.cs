namespace Plugin.Abstraction.HelpTopic.Parts
{
    public enum HelpPartType
    {
        Summary,
        ActionResponse,
        ActionRequest,
        Property,
        ExternalLink,
        RelatedTopic,
        AddtionalResources,
        ControllerVersion,

        /// <summary>
        /// Automatically assigned
        /// </summary>
        UsedBy,

        /// <summary>
        /// Automatically assigned
        /// </summary>
        Uses,
    }
}