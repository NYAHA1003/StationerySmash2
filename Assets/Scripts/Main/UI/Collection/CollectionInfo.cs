using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using TMPro;

public class CollectionInfo : MonoBehaviour
{
    //�ν����� ���� ����
    [SerializeField]
    private SelectCollectionInfo _selectCollectionInfo = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private Image _collectionImage = null;
    [SerializeField]
    private Button _selectButton = null;

    private CollectionData _collectionData = null;

    private bool _isHave = false;


    private void Start()
    {
        _selectButton.onClick.AddListener(() => OnSelectCollectionInfo());
    }

    /// <summary>
    /// �÷��� ������ ����
    /// </summary>
    public void SetCollection(CollectionData collectionData, bool isHave)
    {
        _isHave = isHave;
        if(collectionData == null)
        {
            _collectionData = null;
            _nameText.text = "����";
            _collectionImage.sprite = null;
        }
        else
        {
            _collectionData = collectionData;
            if(_isHave)
            {
                _nameText.text = collectionData._name + "������ ����";
            }
            else
            {
                _nameText.text = collectionData._name;
            }
            _collectionImage.sprite = collectionData._collectionSprite;
        }
    }

    /// <summary>
    /// �÷��� ���� â�� �ݴ´�.
    /// </summary>
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �÷��� ����â�� ������ ��
    /// </summary>
    public void OnSelectCollectionInfo()
    {
        if(_collectionData == null)
        {
            return;
        }
        //�÷��� ����â�� Ų��
        _selectCollectionInfo.SetCollection(_collectionData, _isHave);
        _selectCollectionInfo.OnOpenPanel();
    }
}
