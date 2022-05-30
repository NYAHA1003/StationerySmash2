using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
using Utill.Data;

public class ThemeChangeButton : MonoBehaviour
{
    [SerializeField]
    private Image _buttonImage = null;

    /// <summary>
    /// 테마 변경 버튼을 설정한다
    /// </summary>
    public void SetChangeButton(ThemeSkinType themeSkinType)
    {
        //어드레서블로 이미지를 가져와 비동기적으로 테마 이미지를 변경한다
        _buttonImage.sprite = ThemeSkin.GetSkin(themeSkinType, ThemeUIType.ThemePreview);
    }
}
