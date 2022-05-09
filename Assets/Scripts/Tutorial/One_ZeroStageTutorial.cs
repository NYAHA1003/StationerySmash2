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
        //SetTimeScale();
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[0];
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOMove(impactTrans[0].position,0.3f);
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
        //impactTrans[0].DOAnc
        Debug.Log("�� ����"); 
    }
    /// <summary>
    /// ���׷��̵� ���� 
    /// </summary>
    public void ExplainUpgrade()
    {
        //SetTimeScale(); 
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[2];
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOMove(impactTrans[2].position,0.3f);
        Debug.Log("���׷��̵� ����"); 
    }
}