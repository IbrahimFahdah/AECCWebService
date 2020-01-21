using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugin.Abstraction.Controllers;
using Plugin.Abstraction.HelpTopic.Parts;

namespace Plugin.Abstraction.HelpTopic
{
    /// <summary>
    /// This class generate the help topics related to a controller.
    /// </summary>
    public class HelpTopicsProvider
    {
        ConcurrentDictionary<Type, List<HelpTopicAttribute>> dic
            = new ConcurrentDictionary<Type, List<HelpTopicAttribute>>();


        public HelpTopicsProvider()
        {

        }

        public List<HelpTopicAttribute> GetHelpTopics(PluginBaseController controller)
        {

            if (dic.ContainsKey(controller.GetType()))
                return dic[controller.GetType()];

            var _helpTopics = Build(controller);

            dic.TryAdd(controller.GetType(), _helpTopics);
            return _helpTopics;

        }

        public static List<HelpTopicAttribute> Build(PluginBaseController controller)
        {

            var ret = new List<HelpTopicAttribute>();

            var conrollerType = controller.GetType();
            var att = conrollerType.GetCustomAttribute(typeof(HelpTopicAttribute));

            if (att is HelpTopicAttribute conthelpAtt)
            {
                //controller class has the suffix "Controller", so we remove this to get the controller actual name.
                if (conthelpAtt.Title.Contains("[Controller]"))
                {
                    conthelpAtt.Title = conrollerType.Name.Replace("Controller", "");
                }

                conthelpAtt.ID = conrollerType.FullName;
                ret.Add(conthelpAtt);

                BuildControllerParts(conthelpAtt, conrollerType, ret);

            }

            return ret;

        }

        private static void BuildControllerParts(HelpTopicAttribute helpTopicAtt, Type conrollerType, List<HelpTopicAttribute> ret)
        {

            SetParts(helpTopicAtt, conrollerType.GetCustomAttributes(typeof(HelpPartAttribute)));

            //get controller actions
            var tMethods = conrollerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var tm in tMethods)
            {
                if (tm.MemberType == MemberTypes.Method)
                {
                    var att = tm.GetCustomAttribute(typeof(HelpTopicAttribute));
                    if (att is HelpTopicAttribute mhelpAtt)
                    {
                        mhelpAtt.ID = mhelpAtt.ID + "." + tm.Name;

                        ret.Add(mhelpAtt);

                        LinkTopics(helpTopicAtt, mhelpAtt);
                        BuildActionTopic(mhelpAtt, tm, ret);
                    }
                }
            }
        }



        private static void BuildActionTopic(HelpTopicAttribute helpTopicAtt, MethodInfo methodInfo, List<HelpTopicAttribute> ret)
        {

            SetParts(helpTopicAtt, methodInfo.GetCustomAttributes(typeof(HelpPartAttribute)));

            //get action request and response topic
            var types = helpTopicAtt.Parts.Where(x => x is HelpRequestResponseAttribute).Cast<HelpRequestResponseAttribute>().ToList();
            foreach (var t in types)
            {

                var att = t.Type.GetCustomAttribute(typeof(HelpTopicAttribute));
                if (att is HelpTopicAttribute tHelpAtt)
                {
                    var found = ret.FirstOrDefault(x => x.ID == t.Type.FullName);
                    if (found != null)
                    {
                        //avoid adding duplicate
                        tHelpAtt = found;
                    }

                    LinkTopics(helpTopicAtt, tHelpAtt);
                    BuildTypeTopic(tHelpAtt, t.Type, ret);
                }
            }

        }

        private static void BuildTypeTopic(HelpTopicAttribute helpTopicAtt, Type type, List<HelpTopicAttribute> ret)
        {
            var found = ret.FirstOrDefault(x => x.ID == helpTopicAtt.ID);
            if (found != null)
                return;

            ret.Add(helpTopicAtt);
            helpTopicAtt.ID = type.FullName;
            SetParts(helpTopicAtt, type.GetCustomAttributes(typeof(HelpPartAttribute)));

            //get type properties
            var tProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var tp in tProperties)
            {
                if (tp.MemberType == MemberTypes.Property)
                {
                    var att = tp.GetCustomAttribute(typeof(HelpPropertyAttribute));
                    if (att is HelpPropertyAttribute phelpAtt)
                    {
                        helpTopicAtt.Parts.Add(phelpAtt);
                        phelpAtt.Name = tp.Name;
                        phelpAtt.PropertyType = tp.PropertyType;
                        if (phelpAtt.RelatedTypes != null)
                        {
                            foreach (var t in phelpAtt.RelatedTypes)
                            {
                                var att2 = t.GetCustomAttribute(typeof(HelpTopicAttribute));
                                if (att2 is HelpTopicAttribute thelpAtt)
                                {
                                    LinkTopics(helpTopicAtt, thelpAtt);
                                    BuildTypeTopic(thelpAtt, t, ret);
                                }

                            }
                        }

                    }
                }
            }

            //todo enum
            var tFileds = type.GetFields();
            //foreach (var tf in tFileds)
            //{
            //    var att = tf.GetCustomAttribute(typeof(HelpAttribute));
            //    if (att is HelpAttribute helpAtt)
            //    {
            //        HelpTopic subTopic = new HelpTopic();
            //        subTopic.ID = "";
            //        subTopic.DataType = tf.FieldType;
            //        subTopic.Title = !string.IsNullOrWhiteSpace(helpAtt.Title) ? helpAtt.Title : tf.Name;
            //        subTopic.HelpAttribute = helpAtt;
            //        helpTopic.SubTopics.Add(subTopic);
            //    }
            //}

        }
        private static void LinkTopics(HelpTopicAttribute helpTopicAtt, HelpTopicAttribute mhelpAtt)
        {
            helpTopicAtt.Uses.Add(mhelpAtt);
            mhelpAtt.UsedBy.Add(helpTopicAtt);
        }

        private static void SetParts(HelpTopicAttribute conthelpAtt, IEnumerable<Attribute> parts)
        {
            if (parts != null)
            {
                foreach (var part in parts)
                {
                    conthelpAtt.Parts.Add((HelpPartAttribute)part);
                }
            }
        }
    }
}