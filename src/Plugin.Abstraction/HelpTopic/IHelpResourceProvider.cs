using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Abstraction.HelpTopic
{
    public interface IHelpResourceProvider
    {
       object GetResource(string name);
    }
}
