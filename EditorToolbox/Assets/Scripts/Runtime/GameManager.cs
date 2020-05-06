using UnityEngine;

namespace ToolboxEngine
{
    public class GameManager : MonoBehaviour
    {
        public const string gameObjectName = "GameManager";

        public GameData gameData;

        private void OnValidate()
        {
            if (gameObject.name != gameObjectName)
            {
                gameObject.name = gameObjectName;
            }
        }
    }
}
