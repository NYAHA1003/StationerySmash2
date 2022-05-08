using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class AbstractStageTutorial 
{
    private bool isPause = false;

    /// <summary>
    /// ť ���� 
    /// </summary>
    public abstract void SetQueue(); 
    /// <summary>
    /// ť �ʱ�ȭ 
    /// </summary>
    public void ResetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Clear(); 
    }

    /// <summary>
    /// Ÿ�ӽ����� ���� ( timeScale�� 0 �̸� 1 / 1 �̸� 0 ���� ) 
    /// </summary>
    public void SetTimeScale()
    {
        if (isPause == true)
        {
            Time.timeScale = 1;
            isPause = false;
            return;
        }
        Time.timeScale = 0;
        isPause = true;
    }

    /// <summary>
    /// ���� ���� 
    /// </summary>
    public void NextExplain()
    {
        Debug.Log("���� ����");
        // ��ǳ�� �ؽ�Ʈ �ٸ� �ɷ� �ٲ� 
    }
}
