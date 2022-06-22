using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utill.Load;
using UnityEngine.UI;
using TMPro;
using Battle.Tutorial;
using System.Linq;
using DG.Tweening; 
public abstract class AbstractStageTutorial
{
    [SerializeField]
    protected List<RectTransform> impactTrans = new List<RectTransform>();
    
    protected RectTransform originTrans; // ���������� ���̴� ��ġ 
    protected BattleTurtorialComponent battleTurtorialComponent;
    protected TextMeshProUGUI speechText;
    protected List<TextData> textDatas;
    protected RectTransform blackImpact; 
    
    private bool isPause = false;
    private int _curTextIndex = 0; 
    /// <summary>
    /// ���� ���������� ���� �ؽ�Ʈ ����
    /// </summary>
    /// <param name="index"></param>
    protected void SetSpeechText()
    {
        speechText.text = textDatas[(int)BattleTurtorialComponent.CurrentBattleStageType]._tutorialText[_curTextIndex++];
    }
    /// <summary>
    /// ���� �̹��� ��ġ ���� 
    /// </summary>
    protected void SetImpactPos(Vector2 anchorPos)
    {
        blackImpact.DOAnchorPos(anchorPos,0.4f).SetUpdate(true);
        //blackImpact.anchoredPosition = anchorPos; 
    }
    public void Initialize(Transform impactTransParent)
    {
        _curTextIndex = 0; 
        battleTurtorialComponent = GameObject.FindObjectOfType<BattleTurtorialComponent>();
        speechText = battleTurtorialComponent.SpeechBubbleText;
        textDatas = battleTurtorialComponent.TutorialTextSO._textDatas.ToList();
        blackImpact = battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>();
        originTrans = battleTurtorialComponent.OriginTrans;

        impactTrans.Clear(); 
        for (int i = 0; i  < impactTransParent.childCount;i++)
        {
            impactTrans.Add(impactTransParent.GetChild(i).GetComponent<RectTransform>()); 
        }

    }
    /// <summary>
    /// Ʃ�丮�� ������ �������ٰ� ���ֱ� 
    /// </summary>
    public virtual void EndTutorial()
    {
        SetImpactPos(originTrans.anchoredPosition); 
    }

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

