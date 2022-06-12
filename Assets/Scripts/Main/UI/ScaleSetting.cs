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
		size.x = Mathf.Pow(size.x, Mathf.Log(Screen.width, 2560));
		rect.sizeDelta = size;
	}
}
