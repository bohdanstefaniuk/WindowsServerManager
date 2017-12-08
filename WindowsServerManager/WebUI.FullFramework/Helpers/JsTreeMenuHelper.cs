using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BLL.Dto;
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
                var href = new UrlHelper(requestContext).Action("Index", "IIS", new
                {
                    applicationPath = node.Id,
                    siteType = node.Properties.IISSiteType
                });
                a.Attributes.Add("href", href);
                a.SetInnerText(node.Data);
                li.InnerHtml += a.ToString();
                

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