using System.Collections.Generic;
using Newtonsoft.Json;

namespace BLL.Dto
{
    public class JsTreeModel
    {
        [JsonProperty(PropertyName = "text")]
        public string Data { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id;

        [JsonProperty(PropertyName = "state")]
        public JsTreeModelState State { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<JsTreeModel> Childrens;

        public JsTreeModel()
        {
            Childrens = new List<JsTreeModel>();
        }
    }

    public class JsTreeModelState
    {
        [JsonProperty(PropertyName = "opened")]
        public bool Opened { get; set; }

        [JsonProperty(PropertyName = "disabled")]
        public bool Disabled { get; set; }

        [JsonProperty(PropertyName = "selected")]
        public bool Selected { get; set; }
    }
}