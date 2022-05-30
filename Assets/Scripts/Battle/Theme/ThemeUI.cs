using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;

public class ThemeUI : MonoBehaviour
{
	[SerializeField]
	private ThemeUIType _themeUIType;

	private Image _image;

	private void Start()
	{
		_image = GetComponent<Image>();
		_image.sprite = ThemeSkin.GetSkin(UserSaveManagerSO.UserSaveData._themeSkinType, _themeUIType);
	}
}
