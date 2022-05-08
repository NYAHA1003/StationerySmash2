using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;
using Main.Event;
using Utill.Data; 

/// <summary>
/// 설명으로 나올 텍스트타입
/// </summary>
public enum TutorialType
{
    One_Zero, // 1-0
    One_One, // 1-1
    One_Two, // 1-2
    One_Three, // 1-3
    One_Four, // 1-4
    One_Boss // 1-Boss
}

public class BattleTurtorialComponent : MonoBehaviour
{
    public static Queue<Action> tutorialEventQueue = new Queue<Action>();

    [SerializeField]  
    private Image blackBackground; // 검은 배경이미지 
    [SerializeField]
    private Image explainer; // 설명하는 돼지 

    [SerializeField]
    private Button checkButton; // OK 버튼, 다음 설명으로 넘어감
    [SerializeField]

    private TextMeshProUGUI expaineText; // 상단에 뜰 설명 텍스트    
    [SerializeField]
    private TextMeshProUGUI speechBubbleText; // 말풍선에 뜰 설명 텍스트  
    [SerializeField]
    private Image speechBubble; // 말풍선 
    [SerializeField]
    private Canvas tutorialCanvas; // 튜토리얼 캔버스 

    [SerializeField]
    private One_ZeroStageTutorial one_ZeroStageTutorial;



    private AbstractStageTutorial currentStageTutorial; // 현재 튜토리얼 
    //private bool isPause = false;
    private bool isCheckInput = false;
    private void Start()
    {
        EventManager.StartListening(EventsType.NextExplain, NextExplain);

    }

    /// <summary>
    /// 현재 스테이지에 따른 튜토리얼 설정 
    /// </summary>
    /// <param name="tutorialType"></param>
    public void SetTutorial(TutorialType tutorialType)
    {
        switch (tutorialType)
        {
            case TutorialType.One_Zero:
                currentStageTutorial = one_ZeroStageTutorial;
                break;
            case TutorialType.One_One:
                break;
            case TutorialType.One_Two:
                break;
            case TutorialType.One_Three:
                break;
            case TutorialType.One_Four:
                break;
            case TutorialType.One_Boss:
                break;
        }
        ShowTutorialCanvas(); 
        currentStageTutorial.SetQueue(); 
    }

    /// <summary>
    /// 튜토리얼 캔버스 활성화,비활성화 
    /// </summary>
    public void ShowTutorialCanvas()
    {
        tutorialCanvas.gameObject.SetActive(!tutorialCanvas.gameObject.activeSelf); 
    }
    /// <summary>
    /// 다음 설명 
    /// </summary>
    public void NextExplain()
    {
        Debug.Log("다음 설명");
        currentStageTutorial.NextExplain(); 
        // 현재 스테이지의 텍스트 업데이트해주는 함수 
        // 말풍선 텍스트 다른 걸로 바뀜 
    }
    /// <summary>
    /// 유닛 소환에 대해 설명 
    /// </summary>
    public void ExplainSummon()
    {
        
        Debug.Log("유닛 소한 설명");
    }

    ///// <summary>
    ///// 타임스케일 조정 ( timeScale이 0 이면 1 / 1 이면 0 으로 ) 
    ///// </summary>
    //public void SetTimeScale()
    //{
    //    if(isPause == true)
    //    {
    //        Time.timeScale = 1;
    //        isPause = false; 
    //        return; 
    //    }
    //    Time.timeScale = 0;
    //    isPause = true; 
    //}
}
