using System;
using System.Collections.Generic;
using System.Linq;
using ToolboxEngine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToolboxEditor
{
    public class ToolboxWindow : EditorWindow
    {
        private int tabIndex = 0;

        public static string[] tabs = new string[]
        {
            "General",
            "Path"
        };

        private Path[] pathComponentsArr = null;

        [MenuItem("Toolbox/Toolbox Global")] // Appears at the top under toolbox
        static void InitWindow()
        {
            EditorWindow window = GetWindow<ToolboxWindow>();

            window.autoRepaintOnSceneChange = true;
            window.Show();
            window.titleContent = new GUIContent("Toolbox Global");
        }

        private void OnGUI()
        {
            //GUILayout.Space(10f);
            tabIndex = GUILayout.Toolbar(tabIndex, tabs);

            switch (tabIndex)
            {
                case 0: GUITabsGeneral(); break;
                case 1: GUITabsPath(); break;
            }
        }

        private void OnHierarchyChange()
        {
            pathComponentsArr = FindPathComponentsInScene();
        }

        private void GUITabsGeneral()
        {
            if (GUILayout.Button("Select GameManager"))
            {
                SelectGameManager();
            }

            if (GUILayout.Button("Select GameData"))
            {
                SelectGameData();
            }
        }

        private void GUITabsPath()
        {
            if (pathComponentsArr == null)
            {
                pathComponentsArr = FindPathComponentsInScene();
            }

            foreach (Path path in pathComponentsArr)
            {
                if (path == null) continue;
                if (GUILayout.Button(path.name))
                {
                    Selection.activeGameObject = path.gameObject;
                    SceneView.lastActiveSceneView.FrameSelected();
                    EditorGUIUtility.PingObject(path.gameObject);
                }
            }
        }

        private void SelectGameManager()
        {
            GameManager gameManager = FindGameManagerInScene();

            if (gameManager != null)
            {
                Selection.activeGameObject = gameManager.gameObject;
                SceneView.lastActiveSceneView.FrameSelected(); // Focus sceneview on object
                EditorGUIUtility.PingObject(gameManager.gameObject);
            }
        }

        private GameManager FindGameManagerInScene()
        {
            foreach(GameObject rootGa in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                GameManager gameManager = rootGa.GetComponentInChildren<GameManager>();
                if (gameManager != null)
                {
                    return gameManager;
                }
            }

            return null;
        }

        private void SelectGameData()
        {
            GameManager gameManager = FindGameManagerInScene();

            if (gameManager != null)
            {
                Selection.activeObject = gameManager.gameData;
                EditorGUIUtility.PingObject(gameManager.gameData);
            }
        }

        private Path[] FindPathComponentsInScene()
        {
            List<Path> pathComponentsList = new List<Path>();

            foreach (GameObject rootGa in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                pathComponentsList.AddRange(rootGa.GetComponentsInChildren<Path>());
            }

            return pathComponentsList.ToArray();
        }
    }
}
