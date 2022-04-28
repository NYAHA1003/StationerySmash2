using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class CollectionComponent : MonoBehaviour
{
    //인스펙터 변수
    [SerializeField]
    private CollectionInfo _collectionInfo1 = null;
    [SerializeField]
    private CollectionInfo _collectionInfo2 = null;
    [SerializeField]
    private SelectCollectionInfo _selectCollectionInfo = null;
    [SerializeField]
    private CollectionDataSO _normalCollectionDataSO = null;
    [SerializeField]
    private CollectionDataSO _skinCollectionDataSO = null;
    [SerializeField]
    private Button _nextButton = null;
    [SerializeField]
    private Button _peviousButton = null;
    [SerializeField]
    private Button _changeNormalButton = null;
    [SerializeField]
    private Button _changeSkinButton = null;
    [SerializeField]
    private SaveDataSO _saveDataSO = null;

    //참조 변수
    private CollectionDataSO _currentCollectionData = null;

    //변수
    private int _collectionIndex1 = 0;
    private int _collectionIndex2 = 1;

    public void Awake()
    {
        _currentCollectionData = _normalCollectionDataSO;
        ResetData();
        _nextButton.onClick.AddListener(() => OnNextData());
        _peviousButton.onClick.AddListener(() => OnPeviousData());

        _changeNormalButton.onClick.AddListener(() => OnChangeCollection(CollectionType.Normal));
        _changeSkinButton.onClick.AddListener(() => OnChangeCollection(CollectionType.Skin));
    }

	private void OnEnable()
    {
        _collectionIndex1 = 0;
        _collectionIndex2 = 1;

        ResetData();
    }

	/// <summary>
	/// 다음 데이터 가지고 오기
	/// </summary>
	public void OnNextData()
    {
        if (_collectionIndex2 + 2 >= _normalCollectionDataSO._collectionDatas.Count - 1)
        {
            return;
        }
        _collectionIndex1 += 2;
        _collectionIndex2 += 2;

        ResetData();
    }

    /// <summary>
    /// 이전 데이터 가지고 오기
    /// </summary>
    public void OnPeviousData()
    {
        if (_collectionIndex1 - 2 < 0)
        {
            return;
        }
        _collectionIndex1 -= 2;
        _collectionIndex2 -= 2; 
        
        ResetData();
    }
    /// <summary>
    /// 현재 데이터 리셋
    /// </summary>
    public void ResetData()
    {
        CollectionData collectionData1 = _currentCollectionData._collectionDatas[_collectionIndex1];
        CollectionData collectionData2 = _currentCollectionData._collectionDatas[_collectionIndex2];

        _collectionInfo1.SetCollection(collectionData1, ContainHaveCollectionType(collectionData1._collectionThemeType));
        _collectionInfo2.SetCollection(collectionData2, ContainHaveCollectionType(collectionData2._collectionThemeType));
    }

    /// <summary>
    /// 콜렉션의 종류를 바꾼다
    /// </summary>
    public void OnChangeCollection(CollectionType collectionType)
    {
        switch (collectionType)
        {
            default:
            case CollectionType.None:
                break;
            case CollectionType.Normal:
                _currentCollectionData = _normalCollectionDataSO;
                break;
            case CollectionType.Skin:
                _currentCollectionData = _skinCollectionDataSO;
                break;
        }
        _collectionIndex1 = 0;
        _collectionIndex2 = 1;
        ResetData();
    }

    /// <summary>
    /// 해당 콜렉션을 가지고 있는지
    /// </summary>
    /// <returns></returns>
    private bool ContainHaveCollectionType(CollectionThemeType collectionThemeType)
    {
        return _saveDataSO.userSaveData._haveCollectionDatas.Contains(collectionThemeType);
    }
}
