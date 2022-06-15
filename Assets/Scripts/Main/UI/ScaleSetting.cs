using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSetting : MonoBehaviour
{
	[SerializeField]
	private bool _isScale;

	private void Start()
	{
		if(_isScale)
		{
			SettingScale();
		}
		else
		{
			SettingRect();
		}
	}

	/// <summary>
	/// RectTransform을 화면 사이즈에 맞게 변환
	/// </summary>
	private void SettingRect()
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
		if (Screen.height >= 1440)
		{
			size.y = Mathf.Pow(size.y, Mathf.Log(Screen.height, 1440));
		}
		else
		{
			size.y = Mathf.Pow(size.y, Mathf.Log(Screen.height, 1080));
		}
		rect.sizeDelta = size;
	}


	/// <summary>
	/// RectTransform을 화면 사이즈에 맞게 변환
	/// </summary>
	private void SettingScale()
	{
		Vector3 size = GetComponent<RectTransform>().localScale;
		if (Screen.width >= 2560)
		{
			size.x = Mathf.Pow(size.x, Mathf.Log(Screen.width, 2560));
		}
		else
		{
			size.x = Mathf.Pow(size.x, Mathf.Log(Screen.width, 1920));
		}
		if (Screen.height >= 1440)
		{
			size.y = Mathf.Pow(size.y, Mathf.Log(Screen.height, 1440));
		}
		else
		{
			size.y = Mathf.Pow(size.y, Mathf.Log(Screen.height, 1080));
		}
		GetComponent<RectTransform>().localScale = size;
	}
}
