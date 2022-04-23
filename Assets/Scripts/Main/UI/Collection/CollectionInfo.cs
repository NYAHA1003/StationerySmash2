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
    private CollectionInfo _selectCollectionInfo = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private Image _collectionImage = null;
    [SerializeField]
    private Button _selectButton = null;
    [SerializeField]
    private bool isSelectInfo = false;

    private CollectionData _collectionData = null;


    private void Start()
    {
        if(!isSelectInfo)
        {
            _selectButton.onClick.AddListener(() => OnSelectCollectionInfo());
        }
    }

    /// <summary>
    /// �÷��� ������ ����
    /// </summary>
    public void SetCollection(CollectionData collectionData)
    {
        gameObject.SetActive(true);

        if(collectionData == null)
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
        _selectCollectionInfo.SetCollection(_collectionData);
    }
}
