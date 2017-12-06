using AppPoolManager;
using System;
using System.Collections.Generic;
using AppPoolManager.Dto;
using AppPoolManager.Tools;
using BLL.Dto;
using BLL.Services;

namespace TrainingConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsTreeMenuGenerator = new JsTreeMenuService();
            var node = jsTreeMenuGenerator.GetTreeMenuData();

            ViewJsTree(node.Childrens);

            Console.Read();
        }

        static void ViewJsTree(List<JsTreeModel> nodesList, int tabCount = 0)
        {
            foreach (var node in nodesList)
            {
                Console.WriteLine($"{new string('\t', tabCount)}Node name: {node.Data}, Id: {node.Attribute.Id}");
                if (node.Childrens.Count > 0)
                {
                    ViewJsTree(node.Childrens, ++tabCount);
                    tabCount--;
                }
            }
        }
    }
}
