using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof(CardTest))]
public class CardData : Editor
{
    SerializedProperty Option_Prop;
    SerializedProperty OptionB_TR_Prop;
    SerializedProperty OptionC_Name_Prop;

    private void Awake()
    {
        Option_Prop = serializedObject.FindProperty("options");
        OptionB_TR_Prop = serializedObject.FindProperty("OptionB_TR");
        OptionC_Name_Prop = serializedObject.FindProperty("OptionC_Name");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(Option_Prop);

        //if((CardTest.Option) Option_Prop.enumValueIndex ==  CardTest.Option.OptaionA)
        //{

        //}
        //if ((CardTest.Option)Option_Prop.enumValueIndex == CardTest.Option.OptaionB)
        //{
        //    EditorGUILayout.PropertyField(OptionB_TR_Prop);
        //}
        //if ((CardTest.Option)Option_Prop.enumValueIndex == CardTest.Option.OptaionC)
        //{
        //    EditorGUILayout.PropertyField(OptionC_Name_Prop);
        //}

        serializedObject.ApplyModifiedProperties();
    }
}
