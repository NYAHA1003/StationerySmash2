using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utill.Load;
using UnityEngine.UI;
using TMPro;
using Battle.Tutorial;
using System.Linq; 
public abstract class AbstractStageTutorial
{
    [SerializeField]
    protected List<RectTransform> impactTrans = new List<RectTransform>();
    [SerializeField]
    private RectTransform originTrans; // ���������� ���̴� ��ġ 

    protected BattleTurtorialComponent battleTurtorialComponent;
    protected TextMeshProUGUI speechText;
    protected List<TextData> textDatas;
    protected RectTransform blackImpact; 

    private bool isPause = false;

    public void Initialize()
    {
        battleTurtorialComponent = GameObject.FindObjectOfType<BattleTurtorialComponent>();
        speechText = battleTurtorialComponent.SpeechBubbleText;
        textDatas = battleTurtorialComponent.TutorialTextSO._textDatas.ToList();
        blackImpact = battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>(); 
    }
    /// <summary>
    /// Ʃ�丮�� ������ �������ٰ� ���ֱ� 
    /// </summary>
    public abstract void EndTutorial();

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
    //public abstract TextType NextExplain();
    // Debug.Log("���� ����");
    // ��ǳ�� �ؽ�Ʈ �ٸ� �ɷ� �ٲ� 
}

public class TextType
{
    public TextType(BattleStageType battleStageType, int orderIndex)
    {
        _battleStageType = battleStageType;
        _orderIndex = orderIndex;
    }

    public BattleStageType _battleStageType;
    public int _orderIndex;
}

