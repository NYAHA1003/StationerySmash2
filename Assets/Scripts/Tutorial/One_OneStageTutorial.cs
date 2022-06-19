using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;
using DG.Tweening;
using UnityEngine.UI; 
// 1-0 스테이지 튜토리얼 
[System.Serializable]
public class One_OneStageTutorial : AbstractStageTutorial
{

    [SerializeField]
    private RectTransform _cardParent; 
    [SerializeField]
    private RectTransform _fingerPoint;
    [SerializeField]
    private Image _arrow;
    [SerializeField]
    private GameObject _combineCard; 
    private Vector3 _originPos;

    public static bool isTutorial = false; 
    /// <summary>
    /// 이벤트큐에 
    /// </summary>

    public override void SetQueue()
    {
        //ResetQueue(); 
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Welcome);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCards);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCardDetail);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCombine);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCombineCard);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCoin);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainUseCoin);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainSummonCard);
    }
    public override void EndTutorial()
    {
        isTutorial = true;
        _fingerPoint.DOKill();
        base.EndTutorial();
        _fingerPoint.anchoredPosition = _originPos; 
        _fingerPoint.gameObject.SetActive(false);

    }

    /// <summary>
    /// 환영합니다 
    /// </summary>
    private void Welcome()
    {
        SetSpeechText();
    }
    /// <summary>
    /// 카드 설명
    /// </summary>
    private void ExplainCards()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
    /// <summary>
    /// 하나의 카드 세부 설명 
    /// </summary>
    private void ExplainCardDetail()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[1].anchoredPosition);
        blackImpact.transform.localScale = new Vector2(0.45f, 0.6f);
    }
    private void ExplainCombine()
    {
        SetSpeechText();
        SetImpactPos(originTrans.anchoredPosition); 
        blackImpact.transform.localScale = Vector2.one;

        _cardParent.gameObject.SetActive(true);
        _cardParent.DOAnchorPos(new Vector2(0, -386), 0.4f).SetUpdate(true);
    }
    private void ExplainCombineCard()
    {
        blackImpact.transform.localScale = Vector2.one;

        Sequence seq = DOTween.Sequence();
        seq.Append(_arrow.DOFade(1, 0.4f).SetUpdate(true));
        seq.AppendCallback(()=> _combineCard.SetActive(true)).SetUpdate(true); 
    }
    private void ExplainCoin()
    {    
        SetSpeechText();
        SetImpactPos(impactTrans[2].anchoredPosition);                          
        _cardParent.gameObject.SetActive(false); 
    }
    private void ExplainUseCoin()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[3].anchoredPosition);

    }
    
    private void ExplainSummonCard()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[4].anchoredPosition); 
        _fingerPoint.gameObject.SetActive(true);
        _originPos = _fingerPoint.anchoredPosition; 
        _fingerPoint.DOAnchorPosY(-135f,1f).SetLoops(-1,LoopType.Yoyo).SetUpdate(true);
    }
}