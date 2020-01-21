 using System;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using Plugin.Abstraction.HelpTopic;

namespace Plugin.Abstraction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PluginBaseController : Controller
    {
        private readonly HelpTopicsProvider _helpTopicsProvider;
        private readonly IHelpResourceProvider _helpResourceProvider;
        public PluginBaseController(HelpTopicsProvider helpTopicsProvider, IHelpResourceProvider helpResourceProvider)
        {
            _helpTopicsProvider = helpTopicsProvider;
            _helpResourceProvider = helpResourceProvider;
        }
        [HttpGet]
        [HttpGet(nameof(Help))]
        public virtual IActionResult Help([FromQuery]string id)
        {
            var HelpTopicsModel = new HelpTopicModel(_helpResourceProvider);
            HelpTopicsModel.Topics = _helpTopicsProvider.GetHelpTopics(this);

            HelpTopicsModel.HelpURL = GetHelpURL();
            HelpTopicsModel.ControllerURL = GetControllerUrl();

            var topic = HelpTopicsModel.Topics.FirstOrDefault(x => x.ID == id);

            if (topic == null)
            {
                HelpTopicsModel.DisplayID = HelpTopicsModel.Topics.FirstOrDefault(x => x.TopicType == HelpTopicType.Controller).ID;
            }
            else
            {
                HelpTopicsModel.DisplayID = topic.ID;
            }
            return GetView(HelpTopicsModel);
        }


        [HttpGet(nameof(Resource))]
        public FileContentResult Resource([FromQuery]string id)
        {
            //  return File((byte[])_helpResourceProvider.GetResource(id), "application/octet-stream", id);
            var ext = System.IO.Path.GetExtension(id);
            var contentType = "text/plain";
            if (ext.ToLower().Contains("png"))
            {
                contentType = "image/png";
            }
            return new FileContentResult((byte[])_helpResourceProvider.GetResource(id), contentType);
        }

        [HttpGet(nameof(DownLoad))]
        public ActionResult<string> DownLoad([FromQuery]string id)
        {
            return File((byte[])_helpResourceProvider.GetResource(id), "application/octet-stream", id);
        }

        protected virtual string GetControllerUrl()
        {
            string str = HttpContext.Request.GetEncodedUrl();

            var uri = new Uri(str);

            if (!string.IsNullOrWhiteSpace(uri.Query))
                str = str.Replace(uri.Query, "");

            int index= str.IndexOf(ControllerContext.ActionDescriptor.ControllerName,StringComparison.OrdinalIgnoreCase);
             str= str.Substring(0, index + ControllerContext.ActionDescriptor.ControllerName.Length);

            return str;
        }

        protected virtual string GetHelpURL()
        {
            return GetControllerUrl() + "/help";
        }

        protected virtual IActionResult GetView(HelpTopicModel helpTopicsModel)
        {
            return View("HelpTopic", helpTopicsModel);
        }

    }



}
