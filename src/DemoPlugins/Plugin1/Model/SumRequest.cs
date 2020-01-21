
using Plugin.Abstraction.HelpTopic;
using Plugin.Abstraction.HelpTopic.Parts;

namespace AECC.Plugin1.Model
{
    [HelpTopic(HelpTopicType.Request, nameof(SumRequest))]
    public class SumRequest
    {

        [HelpProperty("first number.")]
        public double a { get; set; }

        [HelpProperty("second number.")]
        public double b { get; set; }
    }
}