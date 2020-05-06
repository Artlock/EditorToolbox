using System;
using UnityEngine;

namespace ToolboxEngine
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChangeSpriteColor : MonoBehaviour
    {
        [Serializable]
        public enum CHANGE_MODE
        {
            RANDOM,
            CUSTOM
        }

        public CHANGE_MODE changeMode = CHANGE_MODE.RANDOM;
        public Color customColor = Color.white;
    }
}