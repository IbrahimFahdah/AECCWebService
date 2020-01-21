using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Plugin.Abstraction.HelpTopic.Parts;

namespace Plugin.Abstraction.HelpTopic
{
    /// <summary>
    /// Passed to the help view to generate the help content.
    /// </summary>
    public class HelpTopicModel
    {
        /// <summary>
        /// Provide access to the plugin help resources such as images etc. 
        /// </summary>
        readonly IHelpResourceProvider _helpResourceProvider;

        public HelpTopicModel(IHelpResourceProvider helpResourceProvider)
        {
            _helpResourceProvider = helpResourceProvider;
        }

        public List<HelpTopicAttribute> Topics = new List<HelpTopicAttribute>();

        public string HelpURL { get; set; }

        public string ControllerURL { get; set; }

        public string DisplayID { get; set; }


        public HelpControllerVersionAttribute GetControllerVersion(HelpTopicAttribute topic)
        {
            if (topic.TopicType != HelpTopicType.Controller)
                return null;
            var version = topic.Parts.FirstOrDefault(x => x.PartType == HelpPartType.ControllerVersion);
            return (version as HelpControllerVersionAttribute);
        }

        /// <summary>
        /// Get a help topic summary.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public string GetSummary(HelpTopicAttribute topic)
        {
            var summary = topic.Parts.FirstOrDefault(x => x.PartType == HelpPartType.Summary);
            return InsertResources((summary as HelpSummaryAttribute)?.Summary);
        }

        /// <summary>
        /// Get the list of actions for the controller.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<HelpTopicAttribute> GetControllerActions(HelpTopicAttribute topic)
        {
            if (topic.TopicType != HelpTopicType.Controller)
                return null;
            return topic.Uses.Where(x => x.TopicType == HelpTopicType.Action).ToList();
        }

        /// <summary>
        /// Get the list of actions that return the response type.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<HelpTopicAttribute> GetResponseCallingActions(HelpTopicAttribute topic)
        {
            if (topic.TopicType != HelpTopicType.Response)
                return null;
            return topic.UsedBy.Where(x => x.TopicType == HelpTopicType.Action).ToList();
        }

        /// <summary>
        /// Get the controller of the action topic.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public HelpTopicAttribute GetActionController(HelpTopicAttribute topic)
        {
            if (topic.TopicType != HelpTopicType.Action)
                return null;
            return topic.UsedBy.FirstOrDefault(x => x.TopicType == HelpTopicType.Controller);
        }

        /// <summary>
        /// Get the request help topic used for an action.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public HelpTopicAttribute GetActionRequest(HelpTopicAttribute topic)
        {
            if (topic.TopicType != HelpTopicType.Action)
                return null;
            return topic.Uses.FirstOrDefault(x => x.TopicType == HelpTopicType.Request);
        }

        /// <summary>
        /// Get the response help topic used for an action.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public HelpTopicAttribute GetActionResponse(HelpTopicAttribute topic)
        {
            if (topic.TopicType != HelpTopicType.Action)
                return null;
            return topic.Uses.FirstOrDefault(x => x.TopicType == HelpTopicType.Response);
        }


        public List<HelpPropertyAttribute> GetProperties(HelpTopicAttribute topic)
        {
            return topic.Parts.Where(x => x.PartType == HelpPartType.Property).Cast<HelpPropertyAttribute>().ToList();
        }

        public List<HelpExternalLinkAttribute> GetExternalLinks(HelpTopicAttribute topic)
        {
            return topic.Parts.Where(x => x.PartType == HelpPartType.ExternalLink).Cast<HelpExternalLinkAttribute>().ToList();
        }

        public List<HelpTopicAttribute> GetUses(HelpTopicAttribute topic)
        {
            //=========for these cases some 'Uses' are excluded as already dealt with using other ways.
            if (topic.TopicType == HelpTopicType.Controller)
                return topic.Uses.Where(x => x.TopicType != HelpTopicType.Action).ToList();

            if (topic.TopicType == HelpTopicType.Action)
                return topic.Uses.Where(x => x.TopicType != HelpTopicType.Response &&
                                             x.TopicType != HelpTopicType.Request).ToList();

            if (topic.TopicType == HelpTopicType.Response ||
                topic.TopicType == HelpTopicType.Request ||
                topic.TopicType == HelpTopicType.DataType)
                return topic.Uses.Where(x => x.TopicType != HelpTopicType.DataType).ToList();
            //\=========

            return topic.Uses;
        }

        public List<HelpTopicAttribute> GetUsedBy(HelpTopicAttribute topic)
        {  
            //=========for these cases some 'Used by' are excluded as already dealt with using other ways.
            if (topic.TopicType == HelpTopicType.Response)
                return topic.UsedBy.Where(x => x.TopicType != HelpTopicType.Action).ToList();
            if (topic.TopicType == HelpTopicType.Action)
                return topic.UsedBy.Where(x => x.TopicType != HelpTopicType.Controller).ToList();
            //\=========
            return topic.UsedBy;
        }

        public string GetHtmlLinkDetails(HelpExternalLinkAttribute helpExternalLinkAttribute)
        {
            if (string.IsNullOrWhiteSpace(helpExternalLinkAttribute.HtmlLinkDetails))
                return helpExternalLinkAttribute.HtmlLinkDetails;

            return InsertResources(helpExternalLinkAttribute.HtmlLinkDetails);
        }

        public string InsertResources(string str)
        {
            if (_helpResourceProvider == null || string.IsNullOrWhiteSpace(str))
                return str;

            str = str.Replace(HelpConst.URL, ControllerURL, StringComparison.CurrentCulture);

            string pattern = HelpConst.EmbddedResPattern;

            MatchCollection col = Regex.Matches(str, pattern, RegexOptions.Singleline);
            foreach (Match m in col)
            {
                GroupCollection groups = m.Groups;

                var res = _helpResourceProvider.GetResource(groups[1].Value);
                if (res != null && groups[1].Value.ToLower().Contains(HelpConst.png))
                {
                    //it is image resource
                    string s = Convert.ToBase64String((byte[])res);
                    str = str.Replace(groups[0].Value, string.Format(HelpConst.EmbeddedImg, s));
                }
            }
            return str;
        }


    }
}