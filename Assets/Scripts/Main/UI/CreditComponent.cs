using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;
using TMPro;
using Main.Event;
using Utill.Data; 

public class CreditComponent : MonoBehaviour
{
    [SerializeField]
    private RectTransform _creditTextTrm = null;
    [SerializeField]
    private TextMeshProUGUI _creditText = null;
    private Tweener _creditTweener = null;

    private void Awake()
    {
        EventManager.StartListening(EventsType.MoveCredit, ShowCredit);
    }
    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    public void Initialized()
    {

        int moveY = _creditText.textInfo.lineCount * 100;
        float duration = _creditText.textInfo.lineCount * 1f;
        _creditTweener = _creditTextTrm.DOAnchorPosY(moveY, duration).SetAutoKill(false);
       
    }

    /// <summary>
    /// ũ���� �����ֱ�
    /// </summary>
    public void StartCredit()
    {
        //�����̶� ����ϰ� ������
        if(_creditTweener == null)
        {
            Debug.Log("Ʈ���� �������");
            Initialized();
        }

        _creditTweener.Pause();
        _creditTextTrm.anchoredPosition = new Vector2(0, -1200);
        _creditTweener.Restart();
    }
    public void ShowCredit()
    {
        Invoke(nameof(StartCredit), 0.01f);
    }

}
