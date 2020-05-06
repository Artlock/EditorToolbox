using UnityEngine;
//using UnityEngine.Serialization;

namespace ToolboxEngine
{
    public class MoveAround2D : MonoBehaviour
    {
        //[Header("Params")]
        [SerializeField] private float circleRadius = 1f; // [..., FormerlySerializedAs("radius")]
        [SerializeField] private float circularSpeed = 90f; // [..., , FormerlySerializedAs("speed")]

        public Vector3 centerPos { get; private set; }
        public float angle { get; private set; }

        private void Start()
        {
            centerPos = transform.position;
        }

        private void Update()
        {
            angle += circularSpeed * Time.deltaTime;
            angle %= 360f;

            float radAngle = angle * Mathf.Deg2Rad;
            Vector3 newPos = transform.position;

            newPos.x = centerPos.x + circleRadius * Mathf.Cos(radAngle);
            newPos.y = centerPos.y + circleRadius * Mathf.Sin(radAngle);

            transform.position = newPos;
        }

#if UNITY_EDITOR

        //[Header("Debug")]
        public bool guiDebug = false;
        public int guiDebugFontSize = 16;
        public Color guiTextColor = Color.white;

        private GUIStyle debugTextStyle = null;

        //[Header("Gizmos")]
        public bool gizmosDebug = false;
        public float gizmosSphereRadius = 0.1f;
        public Color gizmosStartColor = Color.green;
        public Color gizmosEndColor = Color.blue;
        public Color gizmosLineColor = Color.red;

        private void OnGUI()
        {
            if (!guiDebug) return;

            if (debugTextStyle == null)
                debugTextStyle = new GUIStyle();

            debugTextStyle.fontSize = guiDebugFontSize;
            debugTextStyle.normal.textColor = guiTextColor;

            GUILayout.BeginVertical();
            GUILayout.Label("Radius = " + circleRadius, debugTextStyle);
            GUILayout.Label("Speed = " + circularSpeed, debugTextStyle);
            GUILayout.Label("Angle = " + angle, debugTextStyle);
            GUILayout.EndVertical();
        }

#endif

    }
}
