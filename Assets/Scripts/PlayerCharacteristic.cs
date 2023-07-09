using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "PlayerCharacteristic", menuName = "ScriptableObjects/PlayerCharacteristic", order = 1)]
public class PlayerCharacteristic : ScriptableObject
{
    // [Range(0, 2)]
    // public float Multiplier;
    public Sprite Icon;
    public TagReact[] TagReacts;

    [System.Serializable]
    public struct TagReact
    {
        public CardTag Tag;
        [Range(-2, 2)]
        public float Value;

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(TagReact))]
        public class TagReactDrawer : PropertyDrawer
        {
            // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            // {
            //     return EditorGUIUtility.singleLineHeight * 2;
            // }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);

                var tagRect = new Rect(position.x, position.y, position.width / 2, position.height);
                var valueRect = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);

                EditorGUI.PropertyField(tagRect, property.FindPropertyRelative("Tag"), GUIContent.none);
                EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);

                EditorGUI.EndProperty();
            }
        }
#endif
    }
}
