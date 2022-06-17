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
    
    protected RectTransform originTrans; // 검은색으로 덮이는 위치 
    protected BattleTurtorialComponent battleTurtorialComponent;
    protected TextMeshProUGUI speechText;
    protected List<TextData> textDatas;
    protected RectTransform blackImpact; 

    private bool isPause = false;
    public int _curTextIndex = 0; 
    /// <summary>
    /// 현재 스테이지의 말할 텍스트 설정
    /// </summary>
    /// <param name="index"></param>
    protected void SetSpeechText()
    {
        speechText.text = textDatas[(int)battleTurtorialComponent.CurrentBattleStageType]._tutorialText[_curTextIndex++];
    }
    /// <summary>
    /// 강조 이미지 위치 설정 
    /// </summary>
    protected void SetImpactPos(Vector2 anchorPos)
    {
        blackImpact.anchoredPosition = anchorPos; 
    }
    public void Initialize(Transform impactTransParent)
    {
        _curTextIndex = 0; 
        battleTurtorialComponent = GameObject.FindObjectOfType<BattleTurtorialComponent>();
        speechText = battleTurtorialComponent.SpeechBubbleText;
        textDatas = battleTurtorialComponent.TutorialTextSO._textDatas.ToList();
        blackImpact = battleTurtorialComponent.BlackBackground.GetComponent<RectTransform>();
        originTrans = battleTurtorialComponent.OriginTrans;
        
        for(int i = 0; i  < impactTransParent.childCount;i++)
        {
            impactTrans.Add(impactTransParent.GetChild(i).GetComponent<RectTransform>()); 
        }

    }
    /// <summary>
    /// 튜토리얼 끝나고 설정해줄거 해주기 
    /// </summary>
    public abstract void EndTutorial();

      /// <summary>
    /// 큐 설정 
    /// </summary>
    public abstract void SetQueue();
    /// <summary>
    /// 큐 초기화 
    /// </summary>
    public void ResetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Clear();
    }

    /// <summary>
    /// 타임스케일 조정 ( timeScale이 0 이면 1 / 1 이면 0 으로 ) 
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
    /// 다음 설명 
    /// </summary>
    //public abstract TextType NextExplain();
    // Debug.Log("다음 설명");
    // 말풍선 텍스트 다른 걸로 바뀜 
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

