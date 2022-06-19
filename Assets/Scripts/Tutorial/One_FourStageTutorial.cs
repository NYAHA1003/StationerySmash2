using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_FourStageTutorial : AbstractStageTutorial
{
    [SerializeField]
    private GameObject _throwGlue;

    public static bool isTutorial = false;
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainWhatIsThrow);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExpainThrowDetail);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Advice);
    }

    public override void EndTutorial()
    {
        isTutorial = true; 
        base.EndTutorial();
    }

    /// <summary>
    /// 던지기가 무엇인지 설명 
    /// </summary>
    private void ExplainWhatIsThrow()
    {
        SetSpeechText();
        blackImpact.localScale = new Vector2(0.6f, 1); 
        SetImpactPos(impactTrans[0].anchoredPosition);

    }
    /// <summary>
    /// 던지기 자세히 설명 
    /// </summary>
    private void ExpainThrowDetail()
    {
        SetSpeechText();
        blackImpact.localScale = Vector2.one;
        _throwGlue.SetActive(true);
    }
    /// <summary>
    /// 조언 
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
        _throwGlue.SetActive(false);
    }
}
