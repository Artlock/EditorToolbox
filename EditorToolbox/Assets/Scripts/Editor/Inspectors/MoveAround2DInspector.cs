using ToolboxEngine;
using UnityEditor;
using UnityEngine;

namespace ToolboxEditor
{
    [CustomEditor(typeof(MoveAround2D))]
    public class MoveAround2DInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Change Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("circleRadius"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("circularSpeed"));

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Debug Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            SerializedProperty guiDebugProperty = serializedObject.FindProperty("guiDebug");
            EditorGUILayout.PropertyField(guiDebugProperty);

            if (guiDebugProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("guiDebugFontSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("guiTextColor"));
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Gizmos Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            SerializedProperty gizmosDebugProperty = serializedObject.FindProperty("gizmosDebug");
            EditorGUILayout.PropertyField(gizmosDebugProperty);

            if (gizmosDebugProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosSphereRadius"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosStartColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosEndColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosLineColor"));
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
        public static void DrawGizmos(MoveAround2D moveScript, GizmoType gizmoType)
        {
            if (!moveScript.gizmosDebug) return;

            // Draw center
            Gizmos.color = moveScript.gizmosStartColor;
            Gizmos.DrawWireSphere(moveScript.centerPos, moveScript.gizmosSphereRadius);

            // Draw destination
            Gizmos.color = moveScript.gizmosEndColor;
            Gizmos.DrawWireSphere(moveScript.transform.position, moveScript.gizmosSphereRadius);

            // Draw line in between
            Gizmos.color = moveScript.gizmosLineColor;
            Gizmos.DrawLine(moveScript.centerPos, moveScript.transform.position);
        }
    }
}
