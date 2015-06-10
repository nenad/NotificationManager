using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace NM
{
    [CustomEditor(typeof(NotificationManager))]
    public class NotificationManagerGUI : Editor
    {
        NotificationManager manager;
        SerializedObject serManager;
        SerializedProperty symbolSprites;
        Array enumValues;
        int symbolCount;
        private string scriptProperty = "m_Script";

        void OnEnable()
        {
            manager = (NotificationManager)target;
            serManager = new SerializedObject(manager);
            symbolSprites = serManager.FindProperty("sprites");
            enumValues = Enum.GetValues(typeof(Symbol));
            symbolSprites.arraySize = enumValues.Length;
            serManager.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            
            serManager.Update();
            symbolCount = enumValues.Length;
            DrawPropertiesExcluding(serializedObject, scriptProperty);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Symbol sprites", EditorStyles.boldLabel);
            for (int i = 0; i < symbolCount; i++)
            {
                SerializedProperty listSprite = symbolSprites.GetArrayElementAtIndex(i);
                listSprite.objectReferenceValue = EditorGUILayout.ObjectField(enumValues.GetValue(i).ToString(), listSprite.objectReferenceValue, typeof(Sprite), true);
                //EditorGUILayout.LabelField(enumValues.GetValue(i).ToString());
            }
            serManager.ApplyModifiedProperties();
        }
    }
}