using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinMakerCommand : MonoBehaviour
{
    //인스펙터 변수
    [SerializeField]
    private Transform _skinParent = null;
    [SerializeField]
    private GameObject _skinButton = null;
    [SerializeField]
    private Image _makeSkinImage = null;

    //변수
    public List<SkinMakeData> _skinMakeDatas = new List<SkinMakeData>();
    
    private void Start()
    {
        for(int i = 0; i < _skinMakeDatas.Count; i++)
        {
            SkinButton skinButton = Instantiate(_skinButton, _skinParent).GetComponent<SkinButton>();
            skinButton.SetSkinData(_skinMakeDatas[i], this);
        }
    }

    public void SetSkinMake(SkinMakeData skinMakeData)
    {
        _makeSkinImage.sprite = skinMakeData.sprite;
    }
}
