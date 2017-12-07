using System;
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface IJsTreeMenuService: IDisposable
    {
        JsTreeModel GetTreeMenuData();
    }
}