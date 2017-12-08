using System.Collections.Generic;
using BLL.Enums;
using Newtonsoft.Json;

namespace BLL.Dto
{
    public class JsTreeModel
    {
        public string Data { get; set; }

        public string Id;

        public JsTreeModelProperties Properties { get; set; }
        
        public List<JsTreeModel> Childrens;

        public JsTreeModel()
        {
            Childrens = new List<JsTreeModel>();
        }
    }

    public class JsTreeModelProperties
    {
        public IISSiteType IISSiteType { get; set; }
    }
}