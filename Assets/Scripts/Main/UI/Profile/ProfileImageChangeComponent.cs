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
	private Transform _profileImageChangeButtonParent = null; //버튼들이 들어갈 부모 트랜스폼
	[SerializeField]
	private GameObject _profileImageChangeButton = null;

	private int _currentlyCount = 0; //마지막으로 제작했던 스킨들의 갯수

	private void Awake()
	{
		SaveManager.Instance.SaveData.AddObserver(this);
	}

	private void Start()
	{
		SetProfileImageChangeButtons();
	}

	/// <summary>
	/// 프로필 이미지 변경
	/// </summary>
	/// <param name="profileType"></param>
	public void OnChangeProfileImage(ProfileType profileType)
	{
		SaveManager.Instance.SaveData.userSaveData._currentProfileType = profileType;
	}

	public void Notify()
	{
		//현재 가진 프로필 이미지 갯수
		int count = UserSaveManagerSO.UserSaveData._haveProfileList.Count;

		//가진 갯수가 제작했던 프로필 이미지 버튼보다 더 많거나 다르다면 스킨 버튼을 제작한다 
		if (count != _currentlyCount || count > _currentlyCount)
		{
			SetProfileImageChangeButtons();
		}
	}

	/// <summary>
	/// 스킨 버튼들 제작
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
