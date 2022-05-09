using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;
using DG.Tweening;

// 1-0 스테이지 튜토리얼 
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
    /// 소환 설명
    /// </summary>
    public void ExplainSummon()
    {
        //SetTimeScalse();
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[0];
        //battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOAnchorPos(impactTrans[0].anchoredPosition, 0.3f);
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().anchoredPosition = impactTrans[0].anchoredPosition;

        // 설명 텍스트에 텍스트 넣어야해 
        // 강조하는 부분 변경 

        Debug.Log("소환설명");
    }
    /// <summary>
    /// 돈 설명
    /// </summary>
    public void ExplainMoney()
    {
        //SetTimeScale(); 
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[1];
        //battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOAnchorPos(impactTrans[1].anchoredPosition,0.3f);
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().anchoredPosition = impactTrans[1].anchoredPosition;
        Debug.Log("돈 설명"); 
    }
    /// <summary>
    /// 업그레이드 설명 
    /// </summary>
    public void ExplainUpgrade()
    {
        //SetTimeScale(); 
        battleTurtorialComponent.SpeechBubbleText.text = battleTurtorialComponent.TutorialTextSO._textDatas[(int)BattleStageType.S1_1]._tutorialText[2];
        //battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().DOAnchorPos(impactTrans[2].anchoredPosition, 0.3f);
        battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>().anchoredPosition = impactTrans[2].anchoredPosition;

        Debug.Log("업그레이드 설명"); 
    }
}