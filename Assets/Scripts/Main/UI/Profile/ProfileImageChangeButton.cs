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

public class ProfileImageChangeButton : MonoBehaviour
{
	[SerializeField]
	private Image _buttonImage = null;

	/// <summary>
	/// 프로필 이미지 변경 버튼을 설정한다
	/// </summary>
	public void SetChangeButton(ProfileType profileType)
    {
        //어드레서블로 이미지를 가져와 비동기적으로 프로필 이미지를 변경한다
        string name = System.Enum.GetName(typeof(ProfileType), profileType);

        Addressables.LoadAssetAsync<Sprite>(name).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                Sprite sprite = obj.Result;
                _buttonImage.sprite = sprite;
                Addressables.Release(obj);
            };
    }
}
