using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_FiveStageTutorial : AbstractStageTutorial
{
    public static bool isTutorial = false;
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Explain);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Advice);
    }

    public override void EndTutorial()
    {
        isTutorial = true;
        base.EndTutorial(); 
    }

    /// <summary>
    /// ���ݱ��� ������ �׽�Ʈ�ϴ� ���Դϴ� 
    /// </summary>
    private void Explain()
    {
        SetSpeechText();
    }
    /// <summary>
    /// ��� ������ �����غ�����
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
}
