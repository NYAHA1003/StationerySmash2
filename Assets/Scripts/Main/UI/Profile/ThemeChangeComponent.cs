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
	private Transform _themeChangeButtonParent = null; //��ư���� �� �θ� Ʈ������
	[SerializeField]
	private GameObject _themeChangeButton = null;

	private int _currentlyCount = 0; //���������� �����ߴ� ��Ų���� ����

	private void Awake()
	{
		UserSaveManagerSO.AddObserver(this);
	}

	private void Start()
	{
		SetThemeChangeButtons();
	}


	/// <summary>
	/// �׸� ����
	/// </summary>
	/// <param name="profileType"></param>
	public void OnChangeTheme(ThemeSkinType themeSkinType)
	{
		UserSaveManagerSO.UserSaveData._themeSkinType = themeSkinType;
	}

	public void Notify()
	{
		//���� ���� ������ �̹��� ����
		int count = UserSaveManagerSO.UserSaveData._haveThemeSkinTypeList.Count;

		//���� ������ �����ߴ� ������ �̹��� ��ư���� �� ���ų� �ٸ��ٸ� ��Ų ��ư�� �����Ѵ� 
		if (count != _currentlyCount || count > _currentlyCount)
		{
			SetThemeChangeButtons();
		}
	}


	/// <summary>
	/// �׸� ��ư�� ����
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
