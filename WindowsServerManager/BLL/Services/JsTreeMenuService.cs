using AppPoolManager;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Dto;
using BLL.Interfaces;
using Microsoft.Web.Administration;

namespace BLL.Services
{
    public class JsTreeMenuService : IJsTreeMenuService
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
                State = new JsTreeModelState
                {
                    Opened = true
                },
                Data = "Server"
            };
            rootNode.Id = "Root";

            PopulateTree(rootNode);

            return rootNode;
        }

        private void PopulateTree(JsTreeModel node)
        {
            foreach (var site in _siteCollection)
            {
                var siteJsTreeModel = new JsTreeModel();
                siteJsTreeModel.State = new JsTreeModelState
                {
                    Opened = true
                };
                siteJsTreeModel.Id = site.Name;
                siteJsTreeModel.Data = site.Name;
                node.Childrens.Add(siteJsTreeModel);

                if (site.Applications.Count > 0)
                {
                    var applications = site.Applications.Where(x => x.Path != "/" && !string.IsNullOrEmpty(x.Path));
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

        private void RecursiveAdd(IEnumerable<ApplicationPath> splitPaths, JsTreeModel node)
        {
            var groups = splitPaths.GroupBy(x => x.PathElements[0]).ToList();
            foreach (var group in groups)
            {
                var childNode = new JsTreeModel
                {
                    State = new JsTreeModelState
                    {
                        Opened = true
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
