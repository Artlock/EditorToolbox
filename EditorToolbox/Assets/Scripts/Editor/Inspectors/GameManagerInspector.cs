using ToolboxEngine;
using UnityEngine;
using UnityEditor;

namespace ToolboxEditor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SerializedProperty gameDataProperty = serializedObject.FindProperty("gameData");

            if (gameDataProperty.objectReferenceValue == null)
            {
                serializedObject.Update();

                gameDataProperty.objectReferenceValue = FindGameDataInProject();

                serializedObject.ApplyModifiedProperties();
            }            
        }

        private GameData FindGameDataInProject()
        {
            string[] fileGuidsArr = AssetDatabase.FindAssets("t:" + typeof(GameData));

            if (fileGuidsArr.Length > 0)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArr[0]);

                return AssetDatabase.LoadAssetAtPath<GameData>(assetPath);
            }
            else
            {
                GameData gameData = ScriptableObject.CreateInstance<GameData>();

                AssetDatabase.CreateAsset(gameData, "Assets/GameData.asset");
                AssetDatabase.SaveAssets();

                return gameData;
            }
        }
    }
}
