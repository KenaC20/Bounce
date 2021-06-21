using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ArrayPatern))]
public class PaternDataEditor : PropertyDrawer
{
    int spaceBetweenField = 20;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);

        Rect newPosition = position;
        newPosition.y += spaceBetweenField;
        SerializedProperty rows = property.FindPropertyRelative("rows");
        
        for (int i = 0; i < rows.arraySize; i++)
        {
            SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("row");
            newPosition.height = spaceBetweenField;

            if (row.arraySize != rows.arraySize)
                row.arraySize = rows.arraySize;

            newPosition.width = spaceBetweenField;

            for (int j = 0; j < rows.arraySize; j++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
                newPosition.x += newPosition.width;
            }

            newPosition.x = position.x;
            newPosition.y += spaceBetweenField;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return spaceBetweenField * 12;
    }
}
