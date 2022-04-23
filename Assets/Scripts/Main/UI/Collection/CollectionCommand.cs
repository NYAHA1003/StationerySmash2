using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class CollectionCommand : MonoBehaviour
{
    //인스펙터 변수
    [SerializeField]
    private CollectionInfo _collectionInfo1 = null;
    [SerializeField]
    private CollectionInfo _collectionInfo2 = null;
    [SerializeField]
    private CollectionInfo _selectCollectionInfo = null;
    [SerializeField]
    private CollectionDataSO _collectionDataSO = null;
    [SerializeField]
    private Button _nextButton = null;
    [SerializeField]
    private Button _peviousButton = null;

    //변수
    private int _collectionIndex1 = 0;
    private int _collectionIndex2 = 1;

    public void Start()
    {
        _nextButton.onClick.AddListener(() => OnNextData());
        _peviousButton.onClick.AddListener(() => OnPeviousData());
    }

    /// <summary>
    /// 다음 데이터 가지고 오기
    /// </summary>
    public void OnNextData()
    {
        if (_collectionIndex2 == _collectionDataSO._collectionDatas.Count - 1)
        {
            return;
        }
        _collectionIndex1++;
        _collectionIndex2++;

        _collectionInfo1.SetCollection(_collectionDataSO._collectionDatas?[_collectionIndex1]);
        _collectionInfo2.SetCollection(_collectionDataSO._collectionDatas?[_collectionIndex2]);
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

        _collectionInfo1.SetCollection(_collectionDataSO._collectionDatas[_collectionIndex1] ?? null);
        _collectionInfo2.SetCollection(_collectionDataSO._collectionDatas[_collectionIndex2] ?? null);
    }
}
