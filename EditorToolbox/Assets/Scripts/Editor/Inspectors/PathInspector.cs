using ToolboxEngine;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace ToolboxEditor
{
    [CustomEditor(typeof(Path))]
    public class PathInspector : Editor
    {
        private GUIStyle pointLabelStyle = null;
        private ReorderableList pointsReorderableList = null;

        // To save last used tool
        private Tool lastUsedTool = Tool.None;

        private void OnEnable()
        {
            pointsReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"));
            pointsReorderableList.drawElementCallback = OnDrawPointsListElement;

            pointLabelStyle = new GUIStyle();
            pointLabelStyle.normal.textColor = Color.yellow;
            pointLabelStyle.fontSize = 16;

            // Save last used tool and set tool to none on selecting the item
            lastUsedTool = Tools.current;
            Tools.current = Tool.None;
        }

        private void OnDisable()
        {
            pointsReorderableList = null;
            pointLabelStyle = null;

            // If no tool selected on deselecting go back to saved tool
            if (Tools.current == Tool.None)
                Tools.current = lastUsedTool;
        }

        private void OnDrawPointsListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.PropertyField(rect, pointsReorderableList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw everything except certain things (Similar to base.OnInspectorGUI(); but with excludes
            DrawPropertiesExcluding(serializedObject, "points");

            pointsReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            Path path = target as Path;
            if (null == path) return;

            // Edit points
            for (int i = 0; i < path.points.Length; i++)
            {
                Vector3 point = path.points[i];

                // CTRL+Z recording start
                EditorGUI.BeginChangeCheck();

                point = Handles.PositionHandle(point, Quaternion.identity);
                point = Handles.FreeMoveHandle(point, Quaternion.identity, 0.25f, Vector3.one, Handles.SphereHandleCap);

                // CTRL+Z recording end
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(path, "Edit Path Point " + i);
                    path.points[i] = point;
                }
            }

            // Draw points and labels
            Handles.color = Color.yellow;
            for (int i = 0; i < path.points.Length; i++)
            {
                Vector3 point = path.points[i];
                Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);

                Vector3 labelPos = point;
                labelPos.y -= 0.1f;
                Handles.Label(labelPos, (i + 1).ToString(), pointLabelStyle);

                if (i > 0)
                {
                    Vector3 previousPoint = path.points[i - 1];
                    Handles.DrawLine(previousPoint, point);
                }
            }

            if (path.isLooping && path.points.Length > 1)
            {
                Handles.DrawLine(path.points[path.points.Length - 1], path.points[0]);
            }

            // Disable scene interactions
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0);
            }
        }
    }
}
