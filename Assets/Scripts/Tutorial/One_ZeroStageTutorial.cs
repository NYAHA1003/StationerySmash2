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
    public override void SetQueue()
    {
        ResetQueue(); 
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainSummon);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainMoney);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainUpgrade);
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