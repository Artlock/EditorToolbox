using ToolboxEngine;
using UnityEditor;
using UnityEngine;

namespace ToolboxEditor
{
    [CustomEditor(typeof(ChangeSpriteColor))]
    public class ChangeSpriteColorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Change Color Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            SerializedProperty changeModeProperty = serializedObject.FindProperty("changeMode");
            EditorGUILayout.PropertyField(changeModeProperty);

            ChangeSpriteColor.CHANGE_MODE changeMode = (ChangeSpriteColor.CHANGE_MODE)changeModeProperty.intValue;

            if (changeMode == ChangeSpriteColor.CHANGE_MODE.CUSTOM)
            {
                SerializedProperty customColorProperty = serializedObject.FindProperty("customColor");
                EditorGUILayout.PropertyField(customColorProperty);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Apply Color Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (changeMode == ChangeSpriteColor.CHANGE_MODE.RANDOM)
            {
                EditorGUILayout.HelpBox("Color will be randomized. Be careful.", MessageType.Warning);
            }

            if (GUILayout.Button("Change Color"))
            {
                ChangeSpriteColor changeColorScript = (ChangeSpriteColor)target;

                switch (changeMode)
                {
                    case ChangeSpriteColor.CHANGE_MODE.RANDOM:

                        Color randomColor = new Color();
                        randomColor.r = Random.Range(0f, 1f);
                        randomColor.g = Random.Range(0f, 1f);
                        randomColor.b = Random.Range(0f, 1f);
                        randomColor.a = 1f;

                        changeColorScript.GetComponent<SpriteRenderer>().color = randomColor;

                        break;
                    case ChangeSpriteColor.CHANGE_MODE.CUSTOM:
                        changeColorScript.GetComponent<SpriteRenderer>().color = changeColorScript.customColor;
                        break;
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }
}
