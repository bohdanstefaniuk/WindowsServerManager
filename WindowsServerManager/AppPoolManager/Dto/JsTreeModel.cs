using System.Collections.Generic;

namespace AppPoolManager.Dto
{
    public class JsTreeModel
    {
        public string Data;
        public JsTreeAttribute Attribute;
        public string State = "open";
        public List<JsTreeModel> Childrens;

        public JsTreeModel()
        {
            Childrens = new List<JsTreeModel>();
        }
    }

    public class JsTreeAttribute
    {
        public string Id;
    }
}