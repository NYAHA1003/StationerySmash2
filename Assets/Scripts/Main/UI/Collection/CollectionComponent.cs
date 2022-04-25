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

    //참조 변수
    private CollectionDataSO _currentCollectionData = null;

    //변수
    private int _collectionIndex1 = 0;
    private int _collectionIndex2 = 1;

    public void Start()
    {
        _currentCollectionData = _normalCollectionDataSO;
        ResetData();
        _nextButton.onClick.AddListener(() => OnNextData());
        _peviousButton.onClick.AddListener(() => OnPeviousData());

        _changeNormalButton.onClick.AddListener(() => OnChangeCollection(CollectionType.Normal));
        _changeSkinButton.onClick.AddListener(() => OnChangeCollection(CollectionType.Skin));
    }

    /// <summary>
    /// 다음 데이터 가지고 오기
    /// </summary>
    public void OnNextData()
    {
        if (_collectionIndex2 == _normalCollectionDataSO._collectionDatas.Count - 1)
        {
            return;
        }
        _collectionIndex1++;
        _collectionIndex2++;

        _collectionInfo1.SetCollection(_currentCollectionData._collectionDatas?[_collectionIndex1]);
        _collectionInfo2.SetCollection(_currentCollectionData._collectionDatas?[_collectionIndex2]);
    }

    /// <summary>
    /// 이전 데이터 가지고 오기
    /// </summary>
    public void OnPeviousData()
    {
        if (_collectionIndex1 == 0)
        {
            return;
        }
        _collectionIndex1--;
        _collectionIndex2--;

        _collectionInfo1.SetCollection(_currentCollectionData._collectionDatas[_collectionIndex1] ?? null);
        _collectionInfo2.SetCollection(_currentCollectionData._collectionDatas[_collectionIndex2] ?? null);
    }
    /// <summary>
    /// 현재 데이터 리셋
    /// </summary>
    public void ResetData()
    {
        _collectionInfo1.SetCollection(_currentCollectionData._collectionDatas[_collectionIndex1] ?? null);
        _collectionInfo2.SetCollection(_currentCollectionData._collectionDatas[_collectionIndex2] ?? null);
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
}
