using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;
using DG.Tweening;

// 1-0 �������� Ʃ�丮�� 
[System.Serializable]
public class One_ZeroStageTutorial : AbstractStageTutorial
{

    [SerializeField]
    private GameObject combineCard; 
    [SerializeField]
    private RectTransform _fingerPoint; 
    /// <summary>
    /// �̺�Ʈť�� 
    /// </summary>
   
    public override void SetQueue()
    {
        //ResetQueue(); 
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Welcome);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCards);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCardDetail);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCombineCard);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainCoin);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainUseCoin);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainSummonCard);
    }
    public override void EndTutorial()
    {
        //_fingerPoint.DOKill();
        battleTurtorialComponent.DOKill(_fingerPoint);

        _fingerPoint.gameObject.SetActive(false);
    }

    /// <summary>
    /// ȯ���մϴ� 
    /// </summary>
    private void Welcome()
    {
        SetSpeechText();
       // speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[0];
    }
    /// <summary>
    /// ī�� ����
    /// </summary>
    private void ExplainCards()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
      //  speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[1];
       // blackImpact.anchoredPosition = impactTrans[0].anchoredPosition; 
    }
    /// <summary>
    /// �ϳ��� ī�� ���� ���� 
    /// </summary>
    private void ExplainCardDetail()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[1].anchoredPosition);
        //speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[2];
        //blackImpact.anchoredPosition = impactTrans[1].anchoredPosition;
        blackImpact.transform.localScale = new Vector2(0.4f, 0.6f);
    }
    private void ExplainCombineCard()
    {
        SetSpeechText();
        SetImpactPos(originTrans.anchoredPosition); 
        //speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[3];
       // blackImpact.anchoredPosition = originTrans.anchoredPosition;
        blackImpact.transform.localScale = Vector2.one;

        combineCard.SetActive(true);
    }
    private void ExplainCoin()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[2].anchoredPosition);
        //speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[4];
      //  blackImpact.anchoredPosition = impactTrans[2].anchoredPosition;

        combineCard.SetActive(false); 
    }
    private void ExplainUseCoin()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[3].anchoredPosition);
        //speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[5];
        //blackImpact.anchoredPosition = impactTrans[3].anchoredPosition;

    }
    private void ExplainSummonCard()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[4].anchoredPosition); 
        //speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[6];
       // blackImpact.anchoredPosition = impactTrans[4].anchoredPosition;
        _fingerPoint.gameObject.SetActive(true);
        battleTurtorialComponent.DO(_fingerPoint); 
       // _fingerPoint.DOAnchorPosY(-135f, 1f).SetLoops(-1,LoopType.Yoyo); 
    
    }
}