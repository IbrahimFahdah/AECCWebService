using Plugin.Abstraction.HelpTopic;
using Plugin.Abstraction.HelpTopic.Parts;

namespace AECC.Plugin1.Model
{
    [HelpTopic(HelpTopicType.Response, nameof(SumResponse))]
    public class SumResponse
    {
        public SumResponse(string helpUrl, string apiVer)
        {
            Help = helpUrl;
            APIVersion = apiVer;
        }

        [HelpProperty("The help link for more details.")]
        public string Help { get; set; }

        [HelpProperty("API version")]
        public string APIVersion { get; set; }

        [HelpProperty("The error details if the request fails.")]
        public string Error { get; set; } = "";

        [HelpProperty("True if the request is valid and the response was created successfully, otherwise false.")]
        public bool Succeeded { get; set; } = true;

        [HelpProperty("Sum data of two numbers")]
        public double Sum { get; set; }

    

    }
}