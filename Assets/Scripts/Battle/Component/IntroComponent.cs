using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using DG.Tweening;
using TMPro;

[System.Serializable]
public class IntroComponent
{
    //������Ƽ
    public bool isEndIntro => _isEndIntro;

    //�ν����� ���� ����
    [SerializeField]
    private TextMeshProUGUI _countText;

    //����
    private bool _isEndIntro = false;


    /// <summary>
    /// ��Ʈ�� ����
    /// </summary>
    public IEnumerator SetIntro()
    {
        _countText.gameObject.SetActive(true);
        _countText.text = "3";
        yield return new WaitForSeconds(0.5f);
        _countText.text = "2";
        yield return new WaitForSeconds(0.5f);
        _countText.text = "1";
        yield return new WaitForSeconds(0.5f);
        _countText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        _countText.gameObject.SetActive(false);
        _isEndIntro = true;
    }
}
