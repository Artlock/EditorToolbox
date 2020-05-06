using UnityEditor;
using UnityEngine;

namespace ToolboxEditor
{
    public class CameraPreviewWindow : EditorWindow
    {
        private Camera camera = null;
        private RenderTexture renderTexture = null;

        [MenuItem("Toolbox/Camera Preview")] // Appears at the top under toolbox
        static void InitWindow()
        {
            EditorWindow window = GetWindow<CameraPreviewWindow>();

            window.autoRepaintOnSceneChange = true;
            window.Show();
            window.titleContent = new GUIContent("Camera Preview");
        }

        private void Awake()
        {
            CreateRenderTexture();
        }

        private void Update()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            if (camera == null) return;

            if (renderTexture == null)
            {
                CreateRenderTexture();
            }

            // To avoid stretching the view by always resizing the rendertexture if the height or width of the view changed
            if (renderTexture.width != position.width || renderTexture.height != position.height)
            {
                CreateRenderTexture();
            }

            RenderTexture tmpCameraTargetTexture = camera.targetTexture; // Save target texture
            camera.targetTexture = renderTexture; // Assign target to our texture
            camera.Render(); // Render to target texture
            camera.targetTexture = tmpCameraTargetTexture; // Restore default target texture
        }

        private void OnSelectionChange()
        {
            GameObject selectedGameObject = Selection.activeGameObject;

            if (selectedGameObject != null)
            {
                Camera cameraInsideSelection = selectedGameObject.GetComponentInChildren<Camera>(true);

                if (cameraInsideSelection != null)
                {
                    camera = cameraInsideSelection;
                }
            }
        }

        private void OnGUI()
        {
            if (renderTexture != null)
            {
                GUI.DrawTexture(new Rect(0, 0, position.width, position.height), renderTexture);
            }
        }

        private void CreateRenderTexture()
        {
            renderTexture = new RenderTexture((int)position.width, (int)position.height, 24, RenderTextureFormat.ARGB32);
        }
    }
}
