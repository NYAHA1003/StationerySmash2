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

    public void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
    }

    public void Notify(ref UserSaveData userSaveData)
    {
        SetProfileImage(ref userSaveData);
    }

    /// <summary>
    /// 프로필 이미지 수정
    /// </summary>
    public void SetProfileImage(ref UserSaveData userSaveData)
    {
        //어드레서블로 이미지를 가져와 비동기적으로 프로필 이미지를 변경한다
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

