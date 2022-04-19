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
    [SerializeField]
    private Button _skinMakeButton = null;

    //변수
    public List<SkinMakeData> _skinMakeDatas = new List<SkinMakeData>();
    private bool _isCanCrate = false;

    //참조 변수 
    private SkinMakeData _selectSkinData = null;

    private void Start()
    {
        for(int i = 0; i < _skinMakeDatas.Count; i++)
        {
            SkinButton skinButton = Instantiate(_skinButton, _skinParent).GetComponent<SkinButton>();
            skinButton.SetSkinData(_skinMakeDatas[i], this);
        }
        _skinMakeButton.onClick.AddListener(() => OnCreate());
    }

    /// <summary>
    /// 스킨 제작
    /// </summary>
    public void OnCreate()
    {
        if(_isCanCrate)
        {
            //재료 소모
            for(int i = 0; i < _selectSkinData._needMaterial.Count; i++)
            {
                MaterialData materialData = _selectSkinData._needMaterial[i];
                _skinTestInventory._materialDatas.Find(x => x._materialType == materialData._materialType)._count -= materialData._count;
            }
            SetMaterialBoxs(_selectSkinData);
        }
        else
        {
            //재료가 부족하거나 이미 제작했습니다
        }
    }

    public void SetSkinMake(SkinMakeData skinMakeData)
    {
        _selectSkinData = skinMakeData;
        _makeSkinImage.sprite = skinMakeData.sprite;
        SetMaterialBoxs(skinMakeData);
        if (CheckCanCreate(skinMakeData))
        {
            _isCanCrate = true;
        }
        else
        {
            _isCanCrate = false;
        }
    }

    /// <summary>
    /// 필요 아이템들 나타낸다.
    /// </summary>
    /// <param name="skinMakeData"></param>
    public void SetMaterialBoxs(SkinMakeData skinMakeData)
    {
        for (int i = 0; i < _materialParent.childCount; i++)
        {
            if (i < skinMakeData._needMaterial.Count)
            {
                _materialParent.GetChild(i).gameObject.SetActive(true);
                MaterialData materialData = _skinTestInventory._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
                int inventoryCount = 0;
                if (materialData != null)
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
