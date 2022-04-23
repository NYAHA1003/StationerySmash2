using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class SkinMakerComponent : MonoBehaviour
{
    //�ν����� ����
    [SerializeField]
    private SaveDataSO _saveDataSO;

    [SerializeField]
    private Transform _skinParent = null; // ��Ų ���� ��ư ���� �θ�
    [SerializeField]
    private GameObject _skinButtonPrefeb = null; //��Ų ���� ��ư ������
    [SerializeField]
    private Image _currentMakeSkinImage = null; //���� ������ ��Ų �̹��� 
    [SerializeField]
    private Transform _materialParent = null; //�ʿ� ��� �ڽ� ���� �θ�
    [SerializeField]
    private Button _skinMakeButton = null; // ��Ų ���� ��ư

    //����
    public List<SkinMakeData> _skinMakeDatas = new List<SkinMakeData>();
    private bool _isCanCrate = false;

    //���� ���� 
    private SkinMakeData _selectSkinData = null;

    private void Start()
    {
        //��Ų �����͵��� �ҷ��� ��Ų ��ư���� ����.
        for(int i = 0; i < _skinMakeDatas.Count; i++)
        {
            SkinButton skinButton = Instantiate(_skinButtonPrefeb, _skinParent).GetComponent<SkinButton>();
            skinButton.SetSkinData(_skinMakeDatas[i], this);
        }

        //��Ų ���۹�ư�� �Լ� �߰�
        _skinMakeButton.onClick.AddListener(() => OnCreate());

    }


    /// <summary>
    /// ��Ų ���� �ݹ� �Լ�
    /// </summary>
    public void OnCreate()
    {
        //��Ų�� ������ �� �ִٸ� �����Ѵ�
        if(_isCanCrate)
        {
            //��� �Ҹ�
            for(int i = 0; i < _selectSkinData._needMaterial.Count; i++)
            {
                MaterialData materialData = _selectSkinData._needMaterial[i];
                _saveDataSO.userSaveData._materialDatas.Find(x => x._materialType == materialData._materialType)._count -= materialData._count;
            }
            SetMaterialBoxs(_selectSkinData);
        }
        else
        {
            //��ᰡ �����ϰų� �̹� �����߽��ϴ�
        }
    }

    /// <summary>
    /// ��Ų �����͸� ������ �װſ� �°� ��Ų ���� ��ư�� �ʿ��� ������ ������
    /// </summary>
    /// <param name="skinMakeData"></param>
    public void SetSkinMakeButtonAndBoxs(SkinMakeData skinMakeData)
    {
        //���� ��Ų �����͸� �ٲٰ� ��������Ʈ�� �������ش�
        _selectSkinData = skinMakeData;
        _currentMakeSkinImage.sprite = skinMakeData.sprite;
        
        //��� ����
        SetMaterialBoxs(skinMakeData);
        
        //������ �� �ִ��� ���� üũ
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
    /// �ʿ� �����۵� ��Ÿ����.
    /// </summary>
    /// <param name="skinMakeData"></param>
    public void SetMaterialBoxs(SkinMakeData skinMakeData)
    {
        //�ʿ� ��� ��ŭ ��� �ڽ����� ���ش�.
        for (int i = 0; i < _materialParent.childCount; i++)
        {
            if (i < skinMakeData._needMaterial.Count)
            {
                //��� �ڽ� ���ֱ�
                _materialParent.GetChild(i).gameObject.SetActive(true);
                
                //��� �����Ͱ� �ִ��� Ž��
                MaterialData materialData = _saveDataSO.userSaveData._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
                int haveMaterialCount = 0;
                
                //�κ��丮�� ��ᰡ �ִٸ� �װ��� ������ �ٲ۴�
                if (materialData != null)
                {
                    haveMaterialCount = materialData._count;
                }
                
                //������ �ڽ��� ���� ���� �������� ������ ������.
                _materialParent.GetChild(i).GetComponent<MaterialBox>().SetMaterial(skinMakeData._needMaterial[i], haveMaterialCount);
            }
            else
            {
                //��� ������ �ʿ��� ��� �������� ������ ���ش�.
                _materialParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��Ḧ �� ��Ҵ��� üũ�Ѵ�
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
                    //�ʿ��� �������� �ʿ� ������ŭ ������ False�� ��ȯ
                    return false;
                }
            }
            else
            {
                //�ʿ� �������� ������ Flase�� ��ȯ
                return false;
            }
        }
        //���� ������ ������ True�� ��ȯ
        return true;
    }
}
