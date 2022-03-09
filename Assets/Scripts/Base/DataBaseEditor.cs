using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill;

//[CustomPropertyDrawer(typeof(DataBase))]
//public class DataEditor : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
//        position.y += GetPropertyHeight(property, label);


//        EditorGUI.PropertyField(position, property, true);
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        SerializedObject childObj = new UnityEditor.SerializedObject(property.objectReferenceValue);
//        SerializedProperty ite = childObj.GetIterator();

//        float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;

//        while(ite.NextVisible(true))
//        {
//            totalHeight += EditorGUI.GetPropertyHeight(ite, label, true) + EditorGUIUtility.standardVerticalSpacing;
//        }

//        return totalHeight;
//    }
//}