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
    /// �׸� ���� ��ư�� �����Ѵ�
    /// </summary>
    public void SetChangeButton(ThemeSkinType themeSkinType)
    {
        //��巹����� �̹����� ������ �񵿱������� �׸� �̹����� �����Ѵ�
        _buttonImage.sprite = ThemeSkin.GetSkin(themeSkinType, ThemeUIType.ThemePreview);
    }
}
