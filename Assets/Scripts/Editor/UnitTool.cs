using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Text;

public class UnitTool : EditorWindow
{

	// Tool Data Start -->
	public const int WidthMiddle = 200;
	public const int WidthLarge = 300;
	public const int WidthXLarge = 450;

	private int selection = 0;
	private string name = "";

	private Vector2 scollerPoint1 = Vector2.zero;
	private Vector2 scollerPoint2 = Vector2.zero;
	// <---End

	[MenuItem("Editor/UnitTool")]
	//gui �ʱ�ȭ
	static void Init()
	{
		UnitTool window = (UnitTool)EditorWindow.GetWindow<UnitTool>(false, "UnitTool");

		window.Show();
	}

	private void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add", GUILayout.Width(WidthLarge)))
			{
				CreateMonsterStructure();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical();
			name = EditorGUILayout.TextField(name, GUILayout.Width(WidthLarge));
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();

	}


	public void CreateMonsterStructure()
	{
		//이름 
		string monsterName = name;

		EditPathClass.CreateUnitStructure(monsterName);

	}
}
