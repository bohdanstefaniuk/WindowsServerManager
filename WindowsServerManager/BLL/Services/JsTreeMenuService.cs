using AppPoolManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppPoolManager.Dto;
using BLL.Dto;
using Microsoft.Web.Administration;

namespace BLL.Services
{
    public class JsTreeMenuService: IDisposable
    {
        private readonly SiteCollection _siteCollection;
        private readonly SitesManager _sitesManager;

        public JsTreeMenuService()
        {
            _sitesManager = new SitesManager();
            _siteCollection = _sitesManager.GetSiteCollection();
        }

        public JsTreeModel GetTreeMenuData()
        {
            var rootNode = new JsTreeModel
            {
                Attribute = new JsTreeAttribute(),
                Data = "Server"
            };
            rootNode.Attribute.Id = "Root";

            PopulateTree(rootNode);

            return rootNode;
        }

        private void PopulateTree(JsTreeModel node)
        {
            foreach (var site in _siteCollection)
            {
                var siteJsTreeModel = new JsTreeModel();
                siteJsTreeModel.Attribute = new JsTreeAttribute();
                siteJsTreeModel.Attribute.Id = site.Name;
                siteJsTreeModel.Data = site.Name;
                node.Childrens.Add(siteJsTreeModel);

                if (site.Applications.Count > 0)
                {
                    var applications = site.Applications;
                    List<ApplicationPath> splitPaths = applications
                        .Select(x => new ApplicationPath
                        {
                            PathElements = x.Path
                                .Split('/')
                                .ToList(),
                            FullPath = x.Path
                        })
                        .ToList();
                    RecursiveAdd(splitPaths, siteJsTreeModel);
                }
            }
        }

        private void RecursiveAdd(List<ApplicationPath> splitPaths, JsTreeModel node)
        {
            var groups = splitPaths.GroupBy(x => x.PathElements[0]).ToList();
            foreach (var group in groups)
            {
                var childNode = new JsTreeModel
                {
                    Attribute = new JsTreeAttribute
                    {
                        Id = group.Select(x => x.FullPath).FirstOrDefault()
                    },
                    Data = group.Key
                };
                node.Childrens.Add(childNode);

                List<ApplicationPath> children = group
                    .Where(x => x.PathElements.Count() > 1)
                    .Select(x => new ApplicationPath
                    {
                        PathElements = x.PathElements.Skip(1).ToList(),
                        FullPath = x.FullPath
                    }).ToList();
                if (children.Count > 0)
                {
                    RecursiveAdd(children, childNode);
                }
            }
        }

        public void Dispose()
        {
            _sitesManager?.Dispose();
        }
    }
}
