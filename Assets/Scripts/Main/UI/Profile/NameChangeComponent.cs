using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
public class NameChangeComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TMP_InputField _inputField = null;
    [SerializeField]
    private Button _nameChangeButton = null;
    [SerializeField]
    private WarrningComponent _warrningComponent;

    private void Awake()
    {
        SaveManager._instance.SaveData.AddObserver(this);
    }

    private void Start()
	{
        if (_warrningComponent == null)
		{
            _warrningComponent = FindObjectOfType<WarrningComponent>();
		}

        _nameChangeButton.onClick.AddListener(() => OnChangeName());

    }

    public void Notify(ref UserSaveData userSaveData)
    {
        SetNameText(ref userSaveData);
    }

    /// <summary>
    /// 이름 변경 콜백함수
    /// </summary>
    public void OnChangeName()
	{
        string name = _inputField.text;

        if(name.Length < 3)
		{
            _warrningComponent.SetWarrning("이름은 3글자 이상이어야 합니다");
            return;
		}
        else if(name.Length > 10)
        {
            _warrningComponent.SetWarrning("이름은 10글자 이하여야 합니다");
            return;
        }

        SaveManager._instance.SaveData.userSaveData._name = name;
        SaveManager._instance.SaveJsonData();

    }

    /// <summary>
    /// 이름 텍스트 값 수정
    /// </summary>
    public void SetNameText(ref UserSaveData userSaveData)
    {
        _nameText.text = userSaveData._name;
    }
}
