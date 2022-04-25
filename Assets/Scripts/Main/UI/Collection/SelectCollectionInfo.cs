using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using TMPro;


public class SelectCollectionInfo : MonoBehaviour
{
    //�ν����� ���� ����
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private Image _collectionImage = null;
    [SerializeField]
    private Button _createButton = null;
    [SerializeField]
    private Button _closeButton = null;
    private CollectionData _collectionData = null;


    private void Start()
    {
        _createButton.onClick.AddListener(() => OnCreate());
        _closeButton.onClick.AddListener(() => OnClosePanel());
    }

    /// <summary>
    /// �÷��� ������ ����
    /// </summary>
    public void SetCollection(CollectionData collectionData)
    {
        if (collectionData == null)
        {
            _collectionData = null;
            _nameText.text = "����";
            _collectionImage.sprite = null;
        }
        else
        {
            _collectionData = collectionData;
            _nameText.text = collectionData._name;
            _collectionImage.sprite = collectionData._collectionSprite;
        }
    }
    /// <summary>
    /// �÷����� ����
    /// </summary>
    public void OnCreate()
    {

    }

    /// <summary>
    /// �÷��� ���� â�� �ݴ´�.
    /// </summary>
    public void OnClosePanel()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// �÷��� ���� â�� ����.
    /// </summary>
    public void OnOpenPanel()
    {
        gameObject.SetActive(true);
    }
}
