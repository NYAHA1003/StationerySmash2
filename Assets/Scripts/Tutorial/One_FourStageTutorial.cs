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
    /// �����Ⱑ �������� ���� 
    /// </summary>
    private void ExpainWhatIsThrow()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
    /// <summary>
    /// ������ �ڼ��� ���� 
    /// </summary>
    private void ExpainThrowDetail()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[1].anchoredPosition);
    }
    /// <summary>
    /// ���� 
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
    }
}
