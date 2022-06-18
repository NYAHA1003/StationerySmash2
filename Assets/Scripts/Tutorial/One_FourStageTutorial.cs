using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_FourStageTutorial : AbstractStageTutorial
{
    [SerializeField]
    private GameObject _throwGlue;
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainWhatIsThrow);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExpainThrowDetail);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Advice);
    }

    public override void EndTutorial()
    {
        base.EndTutorial();
    }

    /// <summary>
    /// �����Ⱑ �������� ���� 
    /// </summary>
    private void ExplainWhatIsThrow()
    {
        SetSpeechText();
        blackImpact.localScale = new Vector2(0.5f, 1); 
        SetImpactPos(impactTrans[0].anchoredPosition);

    }
    /// <summary>
    /// ������ �ڼ��� ���� 
    /// </summary>
    private void ExpainThrowDetail()
    {
        SetSpeechText();
        blackImpact.localScale = Vector2.one;
        _throwGlue.SetActive(true);
    }
    /// <summary>
    /// ���� 
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
        _throwGlue.SetActive(false);
    }
}
