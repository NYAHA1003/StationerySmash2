using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class SkinMakerCommand : MonoBehaviour
{
    //인스펙터 변수
    [SerializeField]
    private Transform _skinParent = null;
    [SerializeField]
    private GameObject _skinButton = null;
    [SerializeField]
    private Image _makeSkinImage = null;
    [SerializeField]
    private Transform _materialParent = null;
    [SerializeField]
    private SkinTestInventory _skinTestInventory = null;

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
        for(int i = 0; i < _materialParent.childCount; i++)
        {
            if(i < skinMakeData._needMaterial.Count)
            {
                _materialParent.GetChild(i).gameObject.SetActive(true);
                MaterialData materialData = _skinTestInventory._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
                int inventoryCount = 0;
                if(materialData != null)
                {
                    inventoryCount = materialData._count;
                }
                _materialParent.GetChild(i).GetComponent<MaterialBox>().SetMaterial(skinMakeData._needMaterial[i], inventoryCount);
            }
            else
            {
                _materialParent.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(CheckCanCreate(skinMakeData))
        {
            Debug.Log("제작 가능");
        }
        else
        {
            Debug.Log("제작 불가능");
        }
    }

    /// <summary>
    /// 재료를 다 모았는지 체크한다
    /// </summary>
    private bool CheckCanCreate(SkinMakeData skinMakeData)
    {
        for (int i = 0; i < skinMakeData._needMaterial.Count; i++)
        {
            MaterialData inventoryData = _skinTestInventory._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
            if (inventoryData != null)
            {
                if(inventoryData._count < skinMakeData._needMaterial[i]._count)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
