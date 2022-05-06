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

public class ProfileImageComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private Image _profileImage = null;
    private ProfileType _profileType = ProfileType.ProNone;
    public void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
    }

    public void Notify(ref UserSaveData userSaveData)
    {
        SetProfileImage(ref userSaveData);
    }

    /// <summary>
    /// ������ �̹��� ����
    /// </summary>
    public void SetProfileImage(ref UserSaveData userSaveData)
    {
        //������ Ÿ���� ������� ������ �������� �ʴ´�
        if(_profileType == userSaveData._currentProfileType)
		{
            return;
		}

        //��巹����� �̹����� ������ �񵿱������� ������ �̹����� �����Ѵ�
        string name = System.Enum.GetName(typeof(ProfileType), userSaveData._currentProfileType);

        Addressables.LoadAssetAsync<Sprite>(name).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                Sprite sprite = obj.Result;
                _profileImage.sprite = sprite;
                Addressables.Release(obj);
            };
        
    }
}

