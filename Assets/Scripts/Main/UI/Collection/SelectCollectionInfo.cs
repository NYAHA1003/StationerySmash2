using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using TMPro;


public class SelectCollectionInfo : MonoBehaviour
{
    //인스펙터 참조 변수
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
    /// 컬렉션 데이터 설정
    /// </summary>
    public void SetCollection(CollectionData collectionData)
    {
        if (collectionData == null)
        {
            _collectionData = null;
            _nameText.text = "없음";
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
    /// 컬렉션을 제작
    /// </summary>
    public void OnCreate()
    {

    }

    /// <summary>
    /// 컬렉션 정보 창을 닫는다.
    /// </summary>
    public void OnClosePanel()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 컬렉션 정보 창을 연다.
    /// </summary>
    public void OnOpenPanel()
    {
        gameObject.SetActive(true);
    }
}
