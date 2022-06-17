using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class One_FourStageTutorial : AbstractStageTutorial
{
    public override void SetQueue()
    {
    }

    public override void EndTutorial()
    {
    }

    /// <summary>
    /// 던지기가 무엇인지 설명 
    /// </summary>
    private void ExpainWhatIsThrow()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
    /// <summary>
    /// 던지기 자세히 설명 
    /// </summary>
    private void ExpainThrowDetail()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[1].anchoredPosition);
    }
    /// <summary>
    /// 조언 
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
    }
}
