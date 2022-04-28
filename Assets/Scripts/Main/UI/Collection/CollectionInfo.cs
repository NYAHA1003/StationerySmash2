using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using TMPro;

public class CollectionInfo : MonoBehaviour
{
    //인스펙터 참조 변수
    [SerializeField]
    private SelectCollectionInfo _selectCollectionInfo = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private Image _collectionImage = null;
    [SerializeField]
    private Button _selectButton = null;
    [SerializeField]
    private GameObject _checkSign = null;

    private CollectionData _collectionData = null;

    private bool _isHave = false;


    private void Start()
    {
        _selectButton.onClick.AddListener(() => OnSelectCollectionInfo());
    }

    /// <summary>
    /// 컬렉션 데이터 설정
    /// </summary>
    public void SetCollection(CollectionData collectionData, bool isHave)
    {
        _isHave = isHave;
        if(collectionData == null)
        {
            _collectionData = null;
            _nameText.text = "없음";
            _collectionImage.sprite = null;
        }
        else
        {
            _collectionData = collectionData;
            _nameText.text = collectionData._name;
            if (_isHave)
            {
                _checkSign.SetActive(true);
            }
            else
            {
                _checkSign.SetActive(false);
            }
            _collectionImage.sprite = collectionData._collectionSprite;
        }
    }

    /// <summary>
    /// 컬렉션 정보 창을 닫는다.
    /// </summary>
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 컬렉션 정보창을 눌렀을 때
    /// </summary>
    public void OnSelectCollectionInfo()
    {
        if(_collectionData == null)
        {
            return;
        }
        //컬렉션 선택창을 킨다
        _selectCollectionInfo.SetCollection(_collectionData, _isHave);
        _selectCollectionInfo.OnOpenPanel();
    }
}
