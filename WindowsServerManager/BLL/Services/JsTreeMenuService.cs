﻿using AppPoolManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using BLL.Dto;
using BLL.Enums;
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
                Properties = new JsTreeModelProperties(),
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
                siteJsTreeModel.Properties = new JsTreeModelProperties
                {
                    IISSiteType = IISSiteType.Site
                };
                siteJsTreeModel.Id = site.Name;
                siteJsTreeModel.Data = site.Name;
                node.Childrens.Add(siteJsTreeModel);

                if (site.Applications.Count > 0)
                {
                    var applications = site.Applications.Where(x => x.Path != "/" || (!string.IsNullOrWhiteSpace(x.ToString()) && x.Path == "/"));
                    List<ApplicationPath> splitPaths = applications
                        .Select(x => new ApplicationPath
                        {
                            PathElements = x.Path
                                .Split('/')
                                .ToList(),
                            FullPath = x.Path
                        })
                        .ToList();
                    RecursiveAdd(splitPaths, siteJsTreeModel, site.Name);
                }
            }
        }

        private void RecursiveAdd(IEnumerable<ApplicationPath> splitPaths, JsTreeModel node, string siteName)
        {
            var groups = splitPaths.GroupBy(x => x.PathElements[0]).ToList();
            foreach (var group in groups)
            {
                var childNode = new JsTreeModel
                {
                    Properties = new JsTreeModelProperties
                    {
                        IISSiteType = IISSiteType.Application,
                        SiteName = siteName
                    },
                    Data = string.IsNullOrEmpty(group.Key) ? "Root" : group.Key,
                    Id = group.Select(x => x.FullPath).FirstOrDefault()
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
                    RecursiveAdd(children, childNode, siteName);
                } else if (childNode.Id == "/")
                {
                    node.Childrens.Remove(childNode);
                }
            }
        }

        public void Dispose()
        {
            _sitesManager?.Dispose();
        }
    }
}
