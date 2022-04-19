using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class SkinMakerCommand : MonoBehaviour
{
    //�ν����� ����
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

    //����
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
                _materialParent.GetChild(i).GetComponent<MaterialBox>().SetMaterial(skinMakeData._needMaterial[i]);
            }
            else
            {
                _materialParent.GetChild(i).gameObject.SetActive(false);

            }
        }

        //CheckCreate(skinMakeData);
    }

    /// <summary>
    /// ��Ḧ �� ��Ҵ��� üũ�Ѵ�
    /// </summary>
    private void CheckCreate(SkinMakeData skinMakeData)
    {
        for (int i = 0; i < skinMakeData._needMaterial.Count; i++)
        {

        }
    }
}
