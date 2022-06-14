using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

[System.Serializable]
public class SetSkinComponent : MonoBehaviour
{
    //�ν����� ��������
    [SerializeField]
    private SkinListSO _skinListSO = null;

    //������Ƽ 
    public bool IsAllSetSkin => _isAllSetSkin;
    
    //����
    private bool _isAllSetSkin = false;


    private void Start()
    {
        SetSkinAll();
        SetShaderAll();
        SetThemeSkinAll();
        SetAnimationAll();
        _isAllSetSkin = true;
    }

    /// <summary>
    /// ��� ��������Ʈ�� �ҷ��´�
    /// </summary>
    private void SetSkinAll()
    {
        int cardTypeCount = _skinListSO._cardNamingSkins.Count;
        
        for (int i = 0; i < cardTypeCount; i++)
        {
            CardNamingSkins cardNamingSkins = _skinListSO._cardNamingSkins[i];
            int skinCount = cardNamingSkins._skinDatas.Count;
            
            for(int j = 0; j < skinCount; j++)
			{
                SkinData skinData = cardNamingSkins._skinDatas[j];
                skinData.AddSkinDataIntCardDictionary(cardNamingSkins._cardNamingType);
                SkinData.SetSkin(skinData._skinType);
			}
        }
    }

    /// <summary>
    /// ��� ���̴��� �ҷ��´�
    /// </summary>
    private void SetShaderAll()
    {
        int shaderTypeCount = System.Enum.GetValues(typeof(ShaderType)).Length;

        for (int i = 0; i < shaderTypeCount; ++i)
        {
            ShaderData.SetSkin((ShaderType)System.Enum.ToObject(typeof(ShaderType), i));
        }
    }

    /// <summary>
    /// ��� �ִϸ��̼��� �ҷ��´�
    /// </summary>
    private void SetAnimationAll()
    {
        int cardTypeCount = _skinListSO._cardNamingSkins.Count;

        for (int i = 0; i < cardTypeCount; i++)
        {
            CardNamingSkins cardNamingSkins = _skinListSO._cardNamingSkins[i];
            int skinCount = cardNamingSkins._skinDatas.Count;

            for (int j = 0; j < skinCount; j++)
            {
                AnimationData.SetAnimator(cardNamingSkins._skinDatas[i]._skinType);
            }
        }
    }

    /// <summary>
    /// ��� �׸� ��Ų �����͸� �ҷ��´�
    /// </summary>
    private async void SetThemeSkinAll()
    {
        int themeTypeCount = System.Enum.GetValues(typeof(ThemeSkinType)).Length;
        int uiTypeCount = System.Enum.GetValues(typeof(ThemeUIType)).Length;

        for(int i = 0; i < themeTypeCount; ++i)
		{
            for(int j = 0; j < uiTypeCount; ++j)
			{
                await ThemeSkin.SetSkinStaticAsync((ThemeSkinType)System.Enum.ToObject(typeof(ThemeSkinType), i), (ThemeUIType)System.Enum.ToObject(typeof(ThemeUIType), j));
			}
		}
    }

    
}
