using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill;

[CustomPropertyDrawer(typeof(ShowWhenAttribute))]
public class DataEditor : PropertyDrawer
{
    private bool showField = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowWhenAttribute attribute = (ShowWhenAttribute)this.attribute;
        //SerializedProperty conditionField = property.serializedObject.FindProperty(attribute.conditionFieldName);
        SerializedProperty conditionField = property.FindSiblingProperty(attribute.conditionFieldName);
        
        // We check that exist a Field with the parameter name
        if (conditionField == null)
        {
            ShowError(position, label, "Error getting the condition Field. Check the name.");
            return;
        }

        switch (conditionField.propertyType)
        {
            case SerializedPropertyType.Enum:
                object paramEnum = attribute.comparationValue;
                object[] paramEnumArray = attribute.comparationValueArray;

                if (paramEnum == null && paramEnumArray == null)
                {
                    ShowError(position, label, "The comparation enum value is null");
                    return;
                }
                else if (IsEnum(paramEnum))
                {
                    //if (!CheckSameEnumType(new[] { paramEnum.GetType() }, property.serializedObject.targetObject.GetType(), conditionField.propertyPath))
                    //{
                    //    ShowError(position, label, "Enum Types doesn't match");
                    //    return;
                    //}
                    //else
                    {
                        string enumValue = Enum.GetValues(paramEnum.GetType()).GetValue(conditionField.enumValueIndex).ToString();
                        if (paramEnum.ToString() != enumValue)
                            showField = false;
                        else
                            showField = true;
                    }
                }
                else if (IsEnum(paramEnumArray))
                {
                    //if (!CheckSameEnumType(paramEnumArray.Select(x => x.GetType()), property.serializedObject.targetObject.GetType(), conditionField.propertyPath))
                    //{
                    //    ShowError(position, label, "Enum Types doesn't match");
                    //    return;
                    //}
                    //else
                    {
                        string enumValue = Enum.GetValues(paramEnumArray[0].GetType()).GetValue(conditionField.enumValueIndex).ToString();
                        if (paramEnumArray.All(x => x.ToString() != enumValue))
                            showField = false;
                        else
                            showField = true;
                    }
                }
                else
                {
                    ShowError(position, label, "The comparation enum value is not an enum");
                    return;
                }
                break;
            default:
                ShowError(position, label, "This type has not supported.");
                return;
        }


        if (showField)
            EditorGUI.PropertyField(position, property, label, true); ;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (showField)
            return EditorGUI.GetPropertyHeight(property);
        else
            return -EditorGUIUtility.standardVerticalSpacing;
    }

    /// <summary>
    /// Return if the object is enum and not null
    /// </summary>
    private static bool IsEnum(object obj)
    {
        return obj != null && obj.GetType().IsEnum;
    }

    /// <summary>
    /// Return if all the objects are enums and not null
    /// </summary>
    private static bool IsEnum(object[] obj)
    {
        return obj != null && obj.All(o => o.GetType().IsEnum);
    }

    /// <summary>
    /// Check if the field with name "fieldName" has the same class as the "checkTypes" classes through reflection
    /// </summary>
    private static bool CheckSameEnumType(IEnumerable<Type> checkTypes, Type classType, string fieldName)
    {
        FieldInfo memberInfo;
        string[] fields = fieldName.Split('.');

        for(int i = 0; i < fields.Length; i++)
        {
            Debug.Log("필드: " + fields[i]);
        }
        Debug.Log("클래스 타입 0번째 필트: " + classType.GetField(fields[0]));

        if (fields.Length > 1)
        {
            memberInfo = classType.GetField(fields[0]);
            Debug.Log("멤버인포 0번째: " + memberInfo);
            for (int i = 1; i < fields.Length; i++)
            {
                memberInfo = memberInfo.FieldType.GetField(fields[i]);
                Debug.Log("멤버인포" + i + "번째: " + memberInfo);
            }
        }
        else
            memberInfo = classType.GetField(fieldName);

        if (memberInfo != null)
            return checkTypes.All(x => x == memberInfo.FieldType);

        return false;
    }

    private void ShowError(Rect position, GUIContent label, string errorText)
    {
        EditorGUI.LabelField(position, label, new GUIContent(errorText));
        showField = true;
    }

    /// <summary>
    /// Return the float value in the content string removing the remove string
    /// </summary>
    private static float? GetValue(string content, string remove)
    {
        string removed = content.Replace(remove, "");
        try
        {
            return float.Parse(removed);
        }
        catch
        {
            return null;
        }
    }
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowWhenAttribute : PropertyAttribute
{

    public readonly string conditionFieldName;
    public readonly object comparationValue;
    public readonly object[] comparationValueArray;

    /// <summary>
    /// Attribute used to show or hide the Field depending on certain conditions
    /// </summary>
    /// <param name="conditionFieldName">Name of the bool condition Field</param>
    public ShowWhenAttribute(string conditionFieldName)
    {
        this.conditionFieldName = conditionFieldName;
    }

    /// <summary>
    /// Attribute used to show or hide the Field depending on certain conditions
    /// </summary>
    /// <param name="conditionFieldName">Name of the Field to compare (bool, enum, int or float)</param>
    /// <param name="comparationValue">Value to compare</param>
    public ShowWhenAttribute(string conditionFieldName, object comparationValue = null)
    {
        this.conditionFieldName = conditionFieldName;
        this.comparationValue = comparationValue;
    }

    /// <summary>
    /// Attribute used to show or hide the Field depending on certain conditions
    /// </summary>
    /// <param name="conditionFieldName">Name of the Field to compare (bool, enum, int or float)</param>
    /// <param name="comparationValueArray">Array of values to compare (only for enums)</param>
    public ShowWhenAttribute(string conditionFieldName, object[] comparationValueArray = null)
    {
        this.conditionFieldName = conditionFieldName;
        this.comparationValueArray = comparationValueArray;
    }
}

public static class SerializedPropertyExt
{
    public static SerializedProperty GetParent(this SerializedProperty aProperty)
    {
        var path = aProperty.propertyPath;
        int i = path.LastIndexOf('.');
        if (i < 0)
            return null;
        return aProperty.serializedObject.FindProperty(path.Substring(0, i));
    }
    public static SerializedProperty FindSiblingProperty(this SerializedProperty aProperty, string aPath)
    {
        var parent = aProperty.GetParent();
        if (parent == null)
            return aProperty.serializedObject.FindProperty(aPath);
        return parent.FindPropertyRelative(aPath);
    }
}