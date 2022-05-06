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
	/// ������ �̹��� ���� ��ư�� �����Ѵ�
	/// </summary>
	public void SetChangeButton(ProfileType profileType)
    {
        //��巹����� �̹����� ������ �񵿱������� ������ �̹����� �����Ѵ�
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
