using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;
using TMPro;

public class WarrningCommand : MonoBehaviour
{
    //�ν����� ����
    [SerializeField]
    private RectTransform _warrningTransform = null;
    [SerializeField]
    private TextMeshProUGUI _textMeshPro = null;

    public string testText;
    //����
    private Tweener _tweener = null;

    public void Start()
    {
        Initialized();
    }


    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    public void Initialized()
    {
        _tweener = _warrningTransform.DOAnchorPosY(-2000, 1).SetDelay(1).SetAutoKill(false);
    }

    /// <summary>
    /// ��� �޽��� ���
    /// </summary>
    /// <param name="text"></param>
    [ContextMenu("��� �޽��� ���")]
    public void SetWarrning()
    {
        _textMeshPro.text = testText;
        _tweener.Pause();
        _warrningTransform.anchoredPosition = new Vector2(0, 0);
        _tweener.Restart();
    }

}
