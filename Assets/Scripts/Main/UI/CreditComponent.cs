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

    private void Start()
    {
        EventManager.Instance.StartListening(EventsType.MoveCredit, StartCredit); 
    }

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    public void Initialized()
    {
        _creditText.ForceMeshUpdate();
        int moveY = _creditText.textInfo.lineCount * 120;
        float duration = _creditText.textInfo.lineCount * 1f;
        _creditTweener = _creditTextTrm.DOAnchorPosY(moveY, duration).SetAutoKill(false);
    }

    /// <summary>
    /// ũ���� �����ֱ�
    /// </summary>
    public void StartCredit()
    {
        _creditTextTrm.anchoredPosition = new Vector2(0, -1200);

        if (_creditTweener == null)
        {
            Initialized();
        }

        _creditTweener.Pause();
        _creditTweener.Restart();
    }
}
