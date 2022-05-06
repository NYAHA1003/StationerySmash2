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
    /// �̸� ���� �ݹ��Լ�
    /// </summary>
    public void OnChangeName()
	{
        string name = _inputField.text;

        if(name.Length < 3)
		{
            _warrningComponent.SetWarrning("�̸��� 3���� �̻��̾�� �մϴ�");
            return;
		}
        else if(name.Length > 10)
        {
            _warrningComponent.SetWarrning("�̸��� 10���� ���Ͽ��� �մϴ�");
            return;
        }

        SaveManager._instance.SaveData.userSaveData._name = name;
        SaveManager._instance.SaveJsonData();

    }

    /// <summary>
    /// �̸� �ؽ�Ʈ �� ����
    /// </summary>
    public void SetNameText(ref UserSaveData userSaveData)
    {
        _nameText.text = userSaveData._name;
    }
}
