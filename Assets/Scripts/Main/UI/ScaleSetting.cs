using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSetting : MonoBehaviour
{
	private void Start()
	{
		SettingScale();
	}

	/// <summary>
	/// RectTransform�� ȭ�� ����� �°� ��ȯ
	/// </summary>
	private void SettingScale()
	{
		RectTransform rect = GetComponent<RectTransform>();
		Vector2 size = rect.sizeDelta;
		if(Screen.width >= 2560)
		{
			size.x = Mathf.Pow(size.x, Mathf.Log(Screen.width, 2560));
		}
		else 
		{
			size.x = Mathf.Pow(size.x, Mathf.Log(Screen.width, 1920));
		}
		rect.sizeDelta = size;
	}
}
