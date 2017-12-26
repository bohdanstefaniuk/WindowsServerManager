using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BLL.Dto;
using BLL.Enums;
using BLL.Services;

namespace WebUI.FullFramework.Helpers
{
    public static class JsTreeMenuHelper
    {
        public static MvcHtmlString GetJsTreeMenu(this HtmlHelper html)
        {
            var rootUl = new TagBuilder("ul");
            JsTreeModel rootNode;
            using (var jsTreeMenuGenerator = new JsTreeMenuService())
            {
                rootNode = jsTreeMenuGenerator.GetTreeMenuData();
            }

            GenerateTreeElements(rootNode.Childrens, rootUl, null);

            return new MvcHtmlString(rootUl.ToString());
        }

        private static void GenerateTreeElements(IEnumerable<JsTreeModel> nodesList, TagBuilder rootUl, TagBuilder previousUl)
        {
            previousUl = rootUl;
            foreach (var node in nodesList)
            {
                TagBuilder li = new TagBuilder("li");
                TagBuilder a = new TagBuilder("a");

                var requestContext = HttpContext.Current.Request.RequestContext;
                var urlHelper = new UrlHelper(requestContext);
                var href = urlHelper.Action("Index", "IIS", new
                {
                    applicationPath = node.Id,
                    siteType = node.Properties.IISSiteType,
                    siteName = node.Properties.SiteName
                });

                a.Attributes.Add("href", href);
                a.SetInnerText(node.Data);

                var iconFileUrl = "";

                switch (node.Properties.IISSiteType)
                {
                    case IISSiteType.Application:
                        iconFileUrl = urlHelper.Content("~/Content/Images/rsz_server-icon.png");
                        break;
                    case IISSiteType.Site:
                        iconFileUrl = urlHelper.Content("~/Content/Images/rsz_site-icon.png");
                        break;
                }
                var iconUrl = $"{{\"icon\":\"{iconFileUrl}\"}}";

                li.Attributes.Add("data-jstree", iconUrl);
                li.InnerHtml += a.ToString();
                li.Attributes.Add("class", "jstree-open");

                if (node.Childrens.Count > 0)
                {
                    var ul = new TagBuilder("ul");
                    GenerateTreeElements(node.Childrens, ul, previousUl);
                    li.InnerHtml += ul.ToString();
                    rootUl.InnerHtml += li.ToString();
                }
                else
                {
                    rootUl.InnerHtml += li.ToString();
                    rootUl = previousUl;
                }
            }
        }
    }
}