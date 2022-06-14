using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

[System.Serializable]
public class SetSkinComponent : MonoBehaviour
{
    //인스펙터 참조변수
    [SerializeField]
    private SkinListSO _skinListSO = null;

    //프로퍼티 
    public bool IsAllSetSkin => _isAllSetSkin;
    
    //변수
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
    /// 모든 스프라이트를 불러온다
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
    /// 모든 쉐이더를 불러온다
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
    /// 모든 애니메이션을 불러온다
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
    /// 모든 테마 스킨 데이터를 불러온다
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
