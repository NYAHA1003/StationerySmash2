using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
using Utill.Data;
using Utill.Tool;
public class ThemeChangeComponent : MonoBehaviour, IUserData
{
	[SerializeField]
	private Transform _themeChangeButtonParent = null; //버튼들이 들어갈 부모 트랜스폼
	[SerializeField]
	private GameObject _themeChangeButton = null;

	private int _currentlyCount = 0; //마지막으로 제작했던 스킨들의 갯수

	private void Awake()
	{
		UserSaveManagerSO.AddObserver(this);
	}

	private void Start()
	{
		SetThemeChangeButtons();
	}


	/// <summary>
	/// 테마 변경
	/// </summary>
	/// <param name="profileType"></param>
	public void OnChangeTheme(ThemeSkinType themeSkinType)
	{
		UserSaveManagerSO.UserSaveData._themeSkinType = themeSkinType;
	}

	public void Notify()
	{
		//현재 가진 프로필 이미지 갯수
		int count = UserSaveManagerSO.UserSaveData._haveThemeSkinTypeList.Count;

		//가진 갯수가 제작했던 프로필 이미지 버튼보다 더 많거나 다르다면 스킨 버튼을 제작한다 
		if (count != _currentlyCount || count > _currentlyCount)
		{
			SetThemeChangeButtons();
		}
	}


	/// <summary>
	/// 테마 버튼들 제작
	/// </summary>
	private void SetThemeChangeButtons()
	{
		int count = UserSaveManagerSO.UserSaveData._haveThemeSkinTypeList.Count;

		for (int i = 0; i < _themeChangeButtonParent.childCount; i++)
		{
			_themeChangeButtonParent.GetChild(i).gameObject.SetActive(false);
		}

		for (int i = 0; i < count; i++)
		{
			Button changeButton = null;
			if (_themeChangeButtonParent.childCount > i)
			{
				changeButton = _themeChangeButtonParent.GetChild(i).GetComponent<Button>();
			}
			else
			{
				changeButton = Instantiate(_themeChangeButton, _themeChangeButtonParent).GetComponent<Button>();
			}

			changeButton.gameObject.SetActive(true);

			ThemeSkinType themeType = UserSaveManagerSO.UserSaveData._haveThemeSkinTypeList[i];

			changeButton.onClick.RemoveAllListeners();
			changeButton.GetComponent<ThemeChangeButton>().SetChangeButton(themeType);
			changeButton.onClick.AddListener(() => OnChangeTheme(themeType));
		}
	}
}
