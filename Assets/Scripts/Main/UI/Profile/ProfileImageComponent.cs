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
        SaveManager.Instance.SaveData.AddObserver(this);
    }

    public void Notify()
    {
        SetProfileImage();
    }

    /// <summary>
    /// 프로필 이미지 수정
    /// </summary>
    public void SetProfileImage()
    {
        //프로필 타입이 변경되지 않으면 실행하지 않는다
        if(_profileType == UserSaveManagerSO.UserSaveData._currentProfileType)
		{
            return;
		}

        //어드레서블로 이미지를 가져와 비동기적으로 프로필 이미지를 변경한다
        string name = System.Enum.GetName(typeof(ProfileType), UserSaveManagerSO.UserSaveData._currentProfileType);

        Addressables.LoadAssetAsync<Sprite>(name).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                Sprite sprite = obj.Result;
                _profileImage.sprite = sprite;
                Addressables.Release(obj);
            };
        
    }
}

