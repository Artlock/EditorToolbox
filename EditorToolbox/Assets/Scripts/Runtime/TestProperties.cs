using UnityEngine;

namespace ToolboxEngine
{
    public class TestProperties : MonoBehaviour
    {
        [CustomSliderProperty(0, 100)] public int testInt = 0;

        [GameObjectTagFilter("MainCamera")] public GameObject testTagMainCamera;

        public SerializableData data;
    }
}
