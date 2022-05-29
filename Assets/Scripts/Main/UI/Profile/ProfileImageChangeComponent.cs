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

public class ProfileImageChangeComponent : MonoBehaviour, IUserData
{
	[SerializeField]
	private Transform _profileImageChangeButtonParent = null; //��ư���� �� �θ� Ʈ������
	[SerializeField]
	private GameObject _profileImageChangeButton = null;

	private int _currentlyCount = 0; //���������� �����ߴ� ��Ų���� ����

	private void Awake()
	{
		SaveManager.Instance.SaveData.AddObserver(this);
	}

	private void Start()
	{
		SetProfileImageChangeButtons();
	}

	/// <summary>
	/// ������ �̹��� ����
	/// </summary>
	/// <param name="profileType"></param>
	public void OnChangeProfileImage(ProfileType profileType)
	{
		SaveManager.Instance.SaveData.userSaveData._currentProfileType = profileType;
	}

	public void Notify()
	{
		//���� ���� ������ �̹��� ����
		int count = UserSaveManagerSO.UserSaveData._haveProfileList.Count;

		//���� ������ �����ߴ� ������ �̹��� ��ư���� �� ���ų� �ٸ��ٸ� ��Ų ��ư�� �����Ѵ� 
		if (count != _currentlyCount || count > _currentlyCount)
		{
			SetProfileImageChangeButtons();
		}
	}

	/// <summary>
	/// ��Ų ��ư�� ����
	/// </summary>
	private void SetProfileImageChangeButtons()
	{
		int count = SaveManager.Instance.SaveData.userSaveData._haveProfileList.Count;
		
		for(int i = 0; i < _profileImageChangeButtonParent.childCount; i++)
		{
			_profileImageChangeButtonParent.GetChild(i).gameObject.SetActive(false);
		}
		
		for(int i = 0; i < count; i++)
		{
			Button changeButton = null;
			if(_profileImageChangeButtonParent.childCount > i)
			{
				changeButton = _profileImageChangeButtonParent.GetChild(i).GetComponent<Button>();
			}
			else
			{
				changeButton = Instantiate(_profileImageChangeButton, _profileImageChangeButtonParent).GetComponent<Button>();
			}

			changeButton.gameObject.SetActive(true);

			ProfileType changeProfileType = SaveManager.Instance.SaveData.userSaveData._haveProfileList[i];

			changeButton.onClick.RemoveAllListeners();
			changeButton.GetComponent<ProfileImageChangeButton>().SetChangeButton(changeProfileType);
			changeButton.onClick.AddListener(() => OnChangeProfileImage(changeProfileType));
		}
	}
}
