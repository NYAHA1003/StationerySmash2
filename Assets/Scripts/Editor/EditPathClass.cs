using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

/// <summary>
/// ���ҽ� ������ ��θ� ������ Ŭ����
/// </summary>
public class EditPathClass : MonoBehaviour
{
	public static void CreateUnitStructure(string unitName)
	{
		string templateFilePath = "Assets/Editor/tempUnit.txt";

		string entityTemplate = File.ReadAllText(templateFilePath);

		entityTemplate = entityTemplate.Replace("$NAME$", unitName.ToString());

		string folderPath = "Assets/Scripts/Battle/UnitObj/";
		if (Directory.Exists(folderPath) == false)
		{
			Directory.CreateDirectory(folderPath);
		}

		string filePath = folderPath + unitName + ".cs";

		if (File.Exists(filePath) == true)
		{
			File.Delete(folderPath);
		}

		File.WriteAllText(filePath, entityTemplate);

		AssetDatabase.Refresh();
	}

}
