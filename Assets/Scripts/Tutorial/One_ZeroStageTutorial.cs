using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;

// 1-0 �������� Ʃ�丮�� 
[System.Serializable]
public class One_ZeroStageTutorial : AbstractStageTutorial
{
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
        SetTimeScale(); 
        // ���� �ؽ�Ʈ�� �ؽ�Ʈ �־���� 
        // �����ϴ� �κ� ���� 
        Debug.Log("��ȯ����");
    }
    /// <summary>
    /// �� ����
    /// </summary>
    public void ExplainMoney()
    {
        SetTimeScale(); 
        Debug.Log("�� ����"); 
    }
    /// <summary>
    /// ���׷��̵� ���� 
    /// </summary>
    public void ExplainUpgrade()
    {
        SetTimeScale(); 
        Debug.Log("���׷��̵� ����"); 
    }

    public override TextType NextExplain()
    {
        return new TextType(BattleStageType.S1_1, 0);
    }
}