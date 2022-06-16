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
    private List<RectTransform> impactTrans = new List<RectTransform>(); 
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

    /// <summary>
    /// ȯ���մϴ� 
    /// </summary>
    private void Welcome()
    {
        speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[0];
    }
    /// <summary>
    /// ī�� ����
    /// </summary>
    private void ExplainCards()
    {
        speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[1];
    }
    private void ExplainCardDetail()
    {
        speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[2];
    }
    private void ExplainCombineCard()
    {
        speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[3];
    }
    private void ExplainCoin()
    {
        speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[4];
    }
    private void ExplainUseCoin()
    {
        speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[5];
    }
    private void ExplainSummonCard()
    {
        speechText.text = textDatas[(int)BattleStageType.S1_1]._tutorialText[6];
    }
    /// <summary>
    /// ��ȯ ����
    /// </summary>
    public void ExplainSummon()
    {
        //SetTimeScalse();
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[0];
        //battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOAnchorPos(impactTrans[0].anchoredPosition, 0.3f);
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().anchoredPosition = impactTrans[0].anchoredPosition;

        // ���� �ؽ�Ʈ�� �ؽ�Ʈ �־���� 
        // �����ϴ� �κ� ���� 

        Debug.Log("��ȯ����");
    }
    /// <summary>
    /// �� ����
    /// </summary>
    public void ExplainMoney()
    {
        //SetTimeScale(); 
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[1];
        //battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOAnchorPos(impactTrans[1].anchoredPosition,0.3f);
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().anchoredPosition = impactTrans[1].anchoredPosition;
        Debug.Log("�� ����"); 
    }
    /// <summary>
    /// ���׷��̵� ���� 
    /// </summary>
    public void ExplainUpgrade()
    {
        //SetTimeScale(); 
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[2];
        //battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOAnchorPos(impactTrans[2].anchoredPosition, 0.3f);
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().anchoredPosition = impactTrans[2].anchoredPosition;

        Debug.Log("���׷��̵� ����"); 
    }
}