using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Abstraction.HelpTopic
{
    public class HelpConst
    {
        public const string URL = "[URL]";

        public const string EmbddedResPattern = "<res>(.*?)</res>";

        public const string png = "png";

        public const string EmbeddedImg = @"<img src=""data: image / png; base64,{0}"" alt =""Not Found"">";

    }
}
