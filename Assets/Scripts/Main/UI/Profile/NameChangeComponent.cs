using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
using Utill.Tool;
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
        SaveManager.Instance.SaveData.AddObserver(this);
    }

    private void Start()
	{
        if (_warrningComponent == null)
		{
            _warrningComponent = FindObjectOfType<WarrningComponent>();
		}

        _nameChangeButton.onClick.AddListener(() => OnChangeName());

    }

    public void Notify()
    {
        SetNameText();
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

        SaveManager.Instance.SaveData.userSaveData._name = name;

    }

    /// <summary>
    /// 이름 텍스트 값 수정
    /// </summary>
    public void SetNameText()
    {
        _nameText.text = UserSaveManagerSO.UserSaveData._name;
    }
}
