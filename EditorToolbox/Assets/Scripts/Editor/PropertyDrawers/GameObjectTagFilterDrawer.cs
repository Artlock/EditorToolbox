using System;
using System.Collections.Generic;
using ToolboxEngine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToolboxEditor
{
    [CustomPropertyDrawer(typeof(GameObjectTagFilterAttribute))]
    public class GameObjectTagFilterDrawer : PropertyDrawer
    {
        private GameObject[] gameObjectsArr = null;
        private string[] gameObjectNamesArr = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GameObjectTagFilterAttribute tagFilterAttribute = attribute as GameObjectTagFilterAttribute;

            if (gameObjectsArr == null)
            {
                gameObjectsArr = FindGameObjectsWithTagInScene(tagFilterAttribute.GetTagFilter());
            }

            if (gameObjectsArr.Length == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            if (gameObjectNamesArr == null)
            {
                List<string> namesList = new List<string>();

                foreach (GameObject ga in gameObjectsArr)
                {
                    namesList.Add(ga.name);
                }

                gameObjectNamesArr = namesList.ToArray();
            }

            GameObject currentGameObject = property.objectReferenceValue as GameObject;

            int currentIndex = Array.IndexOf(gameObjectsArr, currentGameObject);

            if (currentIndex < 0)
            {
                currentIndex = 0;
            }

            currentIndex = EditorGUI.Popup(position, label.text, currentIndex, gameObjectNamesArr);

            property.objectReferenceValue = gameObjectsArr[currentIndex];
        }

        private GameObject[] FindGameObjectsWithTagInScene(string tag)
        {
            List <GameObject> resultList = new List<GameObject>();
            Scene activeScene = SceneManager.GetActiveScene();

            foreach (GameObject gameObject in activeScene.GetRootGameObjects())
            {
                foreach (Transform childTransform in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (childTransform.gameObject.CompareTag(tag))
                    {
                        resultList.Add(childTransform.gameObject);
                    }
                }
            }

            return resultList.ToArray();
        }
    }
}
