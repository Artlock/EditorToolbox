using UnityEngine;

namespace ToolboxEngine
{
    public class GameObjectTagFilterAttribute : PropertyAttribute
    {
        private string tagFilter = "";

        public string GetTagFilter()
        {
            return tagFilter;
        }

        public GameObjectTagFilterAttribute(string tagFilter)
        {
            this.tagFilter = tagFilter;
        }
    }
}
