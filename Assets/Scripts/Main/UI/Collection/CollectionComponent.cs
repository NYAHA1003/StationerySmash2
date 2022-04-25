using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class CollectionComponent : MonoBehaviour
{
    //�ν����� ����
    [SerializeField]
    private CollectionInfo _collectionInfo1 = null;
    [SerializeField]
    private CollectionInfo _collectionInfo2 = null;
    [SerializeField]
    private SelectCollectionInfo _selectCollectionInfo = null;
    [SerializeField]
    private CollectionDataSO _collectionDataSO = null;
    [SerializeField]
    private Button _nextButton = null;
    [SerializeField]
    private Button _peviousButton = null;

    //����
    private int _collectionIndex1 = 0;
    private int _collectionIndex2 = 1;

    public void Start()
    {
        ResetData();
        _nextButton.onClick.AddListener(() => OnNextData());
        _peviousButton.onClick.AddListener(() => OnPeviousData());
    }

    /// <summary>
    /// ���� ������ ������ ����
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
    /// ���� ������ ������ ����
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
    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public void ResetData()
    {
        _collectionInfo1.SetCollection(_collectionDataSO._collectionDatas[_collectionIndex1] ?? null);
        _collectionInfo2.SetCollection(_collectionDataSO._collectionDatas[_collectionIndex2] ?? null);
    }
}
