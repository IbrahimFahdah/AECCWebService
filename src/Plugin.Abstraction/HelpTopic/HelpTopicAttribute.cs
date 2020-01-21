using System;
using System.Collections.Generic;
using Plugin.Abstraction.HelpTopic.Parts;

namespace Plugin.Abstraction.HelpTopic
{
    /// <summary>
    /// This is used for help topics. A help topic content is typically shown on its own view.  
    /// </summary>
    public class HelpTopicAttribute : Attribute
    {
        public HelpTopicAttribute(HelpTopicType topicType,string title)
        {
            TopicType = topicType;
            Title = title;
        }

        public HelpTopicType TopicType { get; }


        public string Title { get; set; }

        /// <summary>
        /// Unique id to access the help topic
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The parts make the help topic
        /// </summary>
        public List<HelpPartAttribute> Parts { get; set; } = new List<HelpPartAttribute>();

        /// <summary>
        /// Other higher level help topics refer to this help topic. 
        /// </summary>
        public List<HelpTopicAttribute> Uses { get; set; } = new List<HelpTopicAttribute>();

        /// <summary>
        /// Other lower level help topics referred by this help topic. 
        /// </summary>
        public List<HelpTopicAttribute> UsedBy { get; set; } = new List<HelpTopicAttribute>();
    }
}