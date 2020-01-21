using System;
using AECC.Plugin1.Model;
using Microsoft.AspNetCore.Mvc;
using Plugin.Abstraction;
using Plugin.Abstraction.Controllers;
using Plugin.Abstraction.HelpTopic;
using Plugin.Abstraction.HelpTopic.Parts;

namespace AECC.Plugin1.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [TypeFilter(typeof(Tracker), Arguments = new object[] { nameof(Plugin1Controller) })]
    [HelpTopic(HelpTopicType.Controller, "[Controller]"),
     HelpControllerVersion(Version, LastUpdated, "<a href=[URL]/resource?id=ReleaseHistory.txt target=_blank>Release history</a>"),
     HelpSummary("This API is for plugin 1."),
     HelpExternalLink("<a href=[URL]/download?id=Dummydatafile.txt>Download data Example.</a>")
     ]
    public class Plugin1Controller : PluginBaseController
    {

        private const string Version = "1.0.0";
        private const string LastUpdated = "27/07/2019";
        private  readonly static object objLock = new object();
        public Plugin1Controller(HelpTopicsProvider helpTopicsProvider,
            IHelpResourceProvider helpResourceProvider) : base(helpTopicsProvider, helpResourceProvider)
        {

        }


        [HttpGet(nameof(Sum))]
        [HelpTopic(HelpTopicType.Action, nameof(Sum)),
         HelpSummary(
             "This action returns the sum of two numbers. <br><res>sum.png</res>" +
             "<br><a href=[URL]/" + nameof(Sum) + "?a=1.0&b=2.0" +
             " target=_blank>Click here for an example.</a>"),
         HelpActionRequest(typeof(SumRequest)),
         HelpActionResponse(typeof(SumResponse))]
        public SumResponse Sum([FromQuery] SumRequest request)
        {
            var res = CreateSumResponse();
            try
            {
                lock (objLock)
                {
                    var sum = request.a+ request.b;
                    res.Sum = sum;
                }
            }
            catch (Exception ex)
            {
                res.Error = ex.Message;
                res.Succeeded = false;
            }

            return res;
        }


  

        private SumResponse CreateSumResponse()
        {
            string ver = $"ver: {Version}, last updated: {LastUpdated}";
            var ret = new SumResponse(GetHelpURL(), ver);

            return ret;
        }



        protected override IActionResult GetView(HelpTopicModel helpTopicsModel)
        {
            return View(helpTopicsModel);
        }
    }
}