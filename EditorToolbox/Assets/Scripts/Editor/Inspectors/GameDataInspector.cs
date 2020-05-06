using ToolboxEngine;
using UnityEditor;
using UnityEngine;

namespace ToolboxEditor
{
    [CustomEditor(typeof(GameData))]
    public class GameDataInspector : Editor
    {
        private SerializedProperty nbPlayersProperty;

        private SerializedProperty player1SpeedProperty;
        private SerializedProperty player2SpeedProperty;
        private SerializedProperty player3SpeedProperty;
        private SerializedProperty player4SpeedProperty;

        private void OnEnable()
        {
            nbPlayersProperty = serializedObject.FindProperty("nbPlayers");
            player1SpeedProperty = serializedObject.FindProperty("player1Speed");
            player2SpeedProperty = serializedObject.FindProperty("player2Speed");
            player3SpeedProperty = serializedObject.FindProperty("player3Speed");
            player4SpeedProperty = serializedObject.FindProperty("player4Speed");
        }

        private void OnDisable()
        {
            nbPlayersProperty = null;
            player1SpeedProperty = null;
            player2SpeedProperty = null;
            player3SpeedProperty = null;
            player4SpeedProperty = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // We could use this but we wanted to recreate a custom slider
            //EditorGUILayout.IntSlider(nbPlayersProperty, 0, 4);
            EditorGUILayout.PropertyField(nbPlayersProperty);

            int nbPlayers = nbPlayersProperty.intValue;
            if (nbPlayers >= 1)
            {
                EditorGUILayout.PropertyField(player1SpeedProperty);
            }
            if (nbPlayers >= 2)
            {
                EditorGUILayout.PropertyField(player2SpeedProperty);
            }
            if (nbPlayers >= 3)
            {
                EditorGUILayout.PropertyField(player3SpeedProperty);
            }
            if (nbPlayers >= 4)
            {
                EditorGUILayout.PropertyField(player4SpeedProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
