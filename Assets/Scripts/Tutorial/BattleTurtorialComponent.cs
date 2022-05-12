using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;
using Main.Event;
using Utill.Data;
using Battle.Tutorial;
using Utill.Load;
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
    private Image blackBackground; // 검은 배경이미지,강조 
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
    private TutorialTextSO tutorialTextSO; // 설명 텍스트정보
    [SerializeField]
    private LoadingBattleDataSO _loadingBattleDataSO;

    public TutorialTextSO TutorialTextSO => tutorialTextSO;
    public TextMeshProUGUI SpeechBubbleText => speechBubbleText;
    public Image BlackBackground => blackBackground; 
    
    [SerializeField]
    private One_ZeroStageTutorial one_ZeroStageTutorial; // 1-0스테이지 튜토리얼 


    private AbstractStageTutorial currentStageTutorial; // 현재 튜토리얼 
    private BattleStageType currentBattleStageType;

    private bool _isPause = false;
    public static bool _isTutorial = false; // 튜토리얼 했는지 
    private void Start()
    {
        speechBubbleText.text = tutorialTextSO._textDatas[(int)currentBattleStageType]._tutorialText[0];
        checkButton.onClick.AddListener(() => NextExplain());
        SetTutorial(); 
        // EventManager.StartListening(EventsType.NextExplain, NextExplain);
    }

    /// <summary>
    /// 현재 스테이지에 따른 튜토리얼 설정 
    /// </summary>
    /// <param name="tutorialType"></param>
    /// SceneLoadButtonManager의 SetBattleLoadButton에서 이벤트로 설정해줄거임
    public void SetTutorial()
    {
        currentBattleStageType = _loadingBattleDataSO.CurrentStageData.battleStageType;
        switch (currentBattleStageType)
        {
            case BattleStageType.S1_1:
                currentStageTutorial = one_ZeroStageTutorial;
                break;
            case BattleStageType.S1_2:
                break;
            case BattleStageType.S1_3:
                break;
            case BattleStageType.S1_4:
                break;
        }
        tutorialEventQueue.Enqueue(StartTutorial);
        currentStageTutorial.SetQueue();
    }

    /// <summary>
    /// 튜토리얼 시작
    /// </summary>
    [ContextMenu("튜토리얼 테스트")]
    public void StartTutorial()
    {
        _isTutorial = true; 
        SetTimeScale();
        ActiveTutorialCanvas();
        NextExplain();
    }

    /// <summary>
    /// 튜토리얼 캔버스 활성화,비활성화 
    /// </summary>
    public void ActiveTutorialCanvas()
    {
        tutorialCanvas.gameObject.SetActive(!tutorialCanvas.gameObject.activeSelf); 
    }
    /// <summary>
    /// 다음 설명 
    /// </summary>
    public void NextExplain()
    {
        Debug.Log("다음 설명");
        if (tutorialEventQueue.Count == 0)
        {
            ActiveTutorialCanvas();
            SetTimeScale(); 
            return; 
        }
        tutorialEventQueue.Dequeue().Invoke();

        // 현재 스테이지의 텍스트 업데이트해주는 함수 
        // 말풍선 텍스트 다른 걸로 바뀜 
    }


    ///// <summary>
    ///// 타임스케일 조정 ( timeScale이 0 이면 1 / 1 이면 0 으로 ) 
    ///// </summary>
    public void SetTimeScale()
    {
        if (_isPause == true)
        {
            Time.timeScale = 1;
            _isPause = false;
            return;
        }
        Time.timeScale = 0;
        _isPause = true;
    }
}
