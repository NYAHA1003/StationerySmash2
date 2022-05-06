using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;
using TMPro;

public class CreditComponent : MonoBehaviour
{
    [SerializeField]
    private RectTransform _creditTextTrm = null;
    [SerializeField]
    private TextMeshProUGUI _creditText = null;
    private Tweener _creditTweener = null;

    /// <summary>
    /// 초기화
    /// </summary>
    public void Initialized()
    {
        int moveY = _creditText.textInfo.lineCount * 120;
        float duration = _creditText.textInfo.lineCount * 1f;
        _creditTweener = _creditTextTrm.DOAnchorPosY(moveY, duration).SetAutoKill(false);
    }

    /// <summary>
    /// 크레딧 보여주기
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

    [ContextMenu("SetCreditt")]
    public void SetCredit()
    {

        Initialized();
        StartCredit();
    }
}
