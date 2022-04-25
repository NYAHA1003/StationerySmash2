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
    private CollectionComponent _collectionComponent = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private Image _collectionImage = null;
    [SerializeField]
    private Button _createButton = null;
    [SerializeField]
    private Button _closeButton = null;
    [SerializeField]
    private SaveDataSO _saveDataSO = null;

    private CollectionData _collectionData = null;
    private bool _isHave = false;


    private void Start()
    {
        _createButton.onClick.AddListener(() => OnCreate());
        _closeButton.onClick.AddListener(() => OnClosePanel());
    }

    /// <summary>
    /// 컬렉션 데이터 설정
    /// </summary>
    public void SetCollection(CollectionData collectionData, bool ishave)
    {
        _isHave = ishave;
        if (collectionData == null)
        {
            _collectionData = null;
            _nameText.text = "없음";
            _collectionImage.sprite = null;
        }
        else
        {
            _collectionData = collectionData;
            if (_isHave)
            {
                _nameText.text = collectionData._name + "가지고 있음";
            }
            else
            {
                _nameText.text = collectionData._name;
            }
            _collectionImage.sprite = collectionData._collectionSprite;
        }
    }
    /// <summary>
    /// 컬렉션을 제작
    /// </summary>
    public void OnCreate()
    {
        if(CheckCanCreate())
        {
            MaterialSpend();
            _isHave = true;
            _saveDataSO.userSaveData._haveCollectionDatas.Add(_collectionData._collectionThemeType);
            _collectionComponent.ResetData();
        }
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

    /// <summary>
    /// 제작 가능한지 체크
    /// </summary>
    /// <returns></returns>
    private bool CheckCanCreate()
    {
        if (_isHave)
        {
            return false;
        }
        int count = 0;
        switch (_collectionData._collectionType)
        {
            case CollectionType.None:
                break;
            case CollectionType.Normal:
                count = _collectionData._needCardNamingType.Count;
                for (int i = 0; i < count; i++)
                {
                    CardSaveData cardSaveData = _saveDataSO.userSaveData._unitSaveDatas.Find(x => x._cardNamingType == _collectionData._needCardNamingType[i]);
                    if (cardSaveData._count < _collectionData._needCardNamingCount[i])
                    {
                        return false;
                    }
                }
                break;
            case CollectionType.Skin:
                count = _collectionData._needSkinTypes.Count;
                for (int i = 0; i < count; i++)
                {
                    if(!_saveDataSO.userSaveData._haveSkinList.Contains(_collectionData._needSkinTypes[i]))
                    {
                        return false;
                    }
                }
                break;
        }
        return true;
    }

    /// <summary>
    /// 재료 소비
    /// </summary>
    /// <returns></returns>
    private void MaterialSpend()
    {
        switch (_collectionData._collectionType)
        {
            case CollectionType.None:
                break;
            case CollectionType.Normal:
                int count = _collectionData._needCardNamingType.Count;
                for (int i = 0; i < count; i++)
                {
                    CardSaveData cardSaveData = _saveDataSO.userSaveData._unitSaveDatas.Find(x => x._cardNamingType == _collectionData._needCardNamingType[i]);
                    cardSaveData._count -= _collectionData._needCardNamingCount[i];
                }
                break;
            case CollectionType.Skin:
                break;
        }
    }
}
