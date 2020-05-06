using ToolboxEngine;
using UnityEditor;
using UnityEngine;

namespace ToolboxEditor
{
    [CustomPropertyDrawer(typeof(CustomSliderPropertyAttribute))]
    public class CustomSliderPropertyDrawer : PropertyDrawer
    {
        const float WARNING_HEIGHT = 30f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CustomSliderPropertyAttribute sliderAttribute = attribute as CustomSliderPropertyAttribute;

            Rect sliderPosition = position;
            sliderPosition.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.IntSlider(sliderPosition, property, sliderAttribute.GetMin(), sliderAttribute.GetMax());

            if (property.intValue > 20)
            {
                Rect helpPosition = position;

                helpPosition.y += EditorGUIUtility.singleLineHeight;
                helpPosition.height = WARNING_HEIGHT;

                EditorGUI.HelpBox(helpPosition, "Value is big! Careful!", MessageType.Warning);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.intValue > 20)
            {
                return EditorGUIUtility.singleLineHeight + WARNING_HEIGHT;
            }

            return base.GetPropertyHeight(property, label);
        }
    }
}
