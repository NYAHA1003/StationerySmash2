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
    /// 초기화
    /// </summary>
    public void Initialized()
    {

        int moveY = _creditText.textInfo.lineCount * 100;
        float duration = _creditText.textInfo.lineCount * 1f;
        _creditTweener = _creditTextTrm.DOAnchorPosY(moveY, duration).SetAutoKill(false);
       
    }

    /// <summary>
    /// 크레딧 보여주기
    /// </summary>
    public void StartCredit()
    {
        //조금이라도 대기하게 만들어라
        if(_creditTweener == null)
        {
            Debug.Log("트위너 비어있음");
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
