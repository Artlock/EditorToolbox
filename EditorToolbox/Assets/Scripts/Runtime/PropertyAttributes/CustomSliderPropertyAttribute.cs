using UnityEngine;

namespace ToolboxEngine
{
    public class CustomSliderPropertyAttribute : PropertyAttribute
    {
        private int min = 0;
        private int max = 100;

        public int GetMin()
        {
            return min;
        }

        public int GetMax()
        {
            return max;
        }

        public CustomSliderPropertyAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
