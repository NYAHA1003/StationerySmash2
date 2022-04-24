using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class SkinMakerComponent : MonoBehaviour
{
    //인스펙터 변수
    [SerializeField]
    private SaveDataSO _saveDataSO;

    [SerializeField]
    private Transform _skinParent = null; // 스킨 선택 버튼 관리 부모
    [SerializeField]
    private GameObject _skinButtonPrefeb = null; //스킨 선택 버튼 프리펩
    [SerializeField]
    private Image _currentMakeSkinImage = null; //현재 선택한 스킨 이미지 
    [SerializeField]
    private Transform _materialParent = null; //필요 재료 박스 관리 부모
    [SerializeField]
    private Button _skinMakeButton = null; // 스킨 제작 버튼

    //변수
    public List<SkinMakeData> _skinMakeDatas = new List<SkinMakeData>();
    private bool _isCanCrate = false;

    //참조 변수 
    private SkinMakeData _selectSkinData = null;

    private void Start()
    {
        //스킨 데이터들을 불러와 스킨 버튼들을 만듦.
        for(int i = 0; i < _skinMakeDatas.Count; i++)
        {
            SkinButton skinButton = Instantiate(_skinButtonPrefeb, _skinParent).GetComponent<SkinButton>();
            skinButton.SetSkinData(_skinMakeDatas[i], this);
        }

        //스킨 제작버튼에 함수 추가
        _skinMakeButton.onClick.AddListener(() => OnCreate());

    }


    /// <summary>
    /// 스킨 제작 콜백 함수
    /// </summary>
    public void OnCreate()
    {
        //스킨을 제작할 수 있다면 제작한다
        if(_isCanCrate)
        {
            //재료 소모
            for(int i = 0; i < _selectSkinData._needMaterial.Count; i++)
            {
                MaterialData materialData = _selectSkinData._needMaterial[i];
                _saveDataSO.userSaveData._materialDatas.Find(x => x._materialType == materialData._materialType)._count -= materialData._count;
            }
            SetMaterialBoxs(_selectSkinData);
        }
        else
        {
            //재료가 부족하거나 이미 제작했습니다
        }
    }

    /// <summary>
    /// 스킨 데이터를 받으면 그거에 맞게 스킨 제작 버튼과 필요한 재료들을 보여줌
    /// </summary>
    /// <param name="skinMakeData"></param>
    public void SetSkinMakeButtonAndBoxs(SkinMakeData skinMakeData)
    {
        //현재 스킨 데이터를 바꾸고 스프라이트도 변경해준다
        _selectSkinData = skinMakeData;
        _currentMakeSkinImage.sprite = skinMakeData.sprite;
        
        //재료 설정
        SetMaterialBoxs(skinMakeData);
        
        //제작할 수 있는지 여부 체크
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
        //필요 재료 만큼 재료 박스들을 켜준다.
        for (int i = 0; i < _materialParent.childCount; i++)
        {
            if (i < skinMakeData._needMaterial.Count)
            {
                //재료 박스 켜주기
                _materialParent.GetChild(i).gameObject.SetActive(true);
                
                //재료 데이터가 있는지 탐색
                MaterialData materialData = _saveDataSO.userSaveData._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
                int haveMaterialCount = 0;
                
                //인벤토리에 재료가 있다면 그거의 갯수로 바꾼다
                if (materialData != null)
                {
                    haveMaterialCount = materialData._count;
                }
                
                //아이템 박스에 현재 가진 아이템의 갯수를 보낸다.
                _materialParent.GetChild(i).GetComponent<MaterialBox>().SetMaterial(skinMakeData._needMaterial[i], haveMaterialCount);
            }
            else
            {
                //재료 종류가 필요한 재료 종류보다 적으면 꺼준다.
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
            MaterialData haveMaterialData = _saveDataSO.userSaveData._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
            if (haveMaterialData != null)
            {
                if(haveMaterialData._count < skinMakeData._needMaterial[i]._count)
                {
                    //필요한 아이템이 필요 갯수만큼 없으면 False를 반환
                    return false;
                }
            }
            else
            {
                //필요 아이템이 없으면 Flase를 반환
                return false;
            }
        }
        //문제 사항이 없으면 True를 반환
        return true;
    }
}
