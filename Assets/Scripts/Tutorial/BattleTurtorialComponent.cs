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
using DG.Tweening; 
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
    //프로퍼티
    public GameObject TutorialCanvas
    {
        get
        {
            //if(tutorialCanvas == null)
            //{
            //    tutorialCanvas = GameObject.Find("TutorialCanvasParent").transform.Find("TutorialCanvas").gameObject;
            //}
            if (tutorialCanvas == null)
            {
                tutorialCanvas = GameObject.Find("TutorialCanvasParent").transform.GetChild(0).gameObject;
            }
            return tutorialCanvas;
        }
    }

    public static Queue<Action> tutorialEventQueue = new Queue<Action>();

    [SerializeField]
    private Image blackBackground; // 검은 배경이미지,강조 
    [SerializeField]
    private Image explainer; // 설명하는 돼지 

    [SerializeField]
    private TextMeshProUGUI expaineText; // 상단에 뜰 설명 텍스트    
    [SerializeField]
    private TextMeshProUGUI speechBubbleText; // 말풍선에 뜰 설명 텍스트  
    [SerializeField]
    private Image speechBubble; // 말풍선 
    [SerializeField]
    private RectTransform _originTrans;
    [SerializeField]
    private Transform[] _impactParent;
    public RectTransform OriginTrans => _originTrans; 
    private GameObject tutorialCanvas; // 튜토리얼 캔버스 

    [SerializeField]
    private TutorialTextSO tutorialTextSO; // 설명 텍스트정보
    [SerializeField]
    private CurrentStageData _currentStageSO; 
    public TutorialTextSO TutorialTextSO => tutorialTextSO;
    public TextMeshProUGUI SpeechBubbleText => speechBubbleText;
    public Image BlackBackground => blackBackground;

    [SerializeField]
    private ST_MakeStageTutorial ST_MakeStageTutorial; // 디버그용 스테이지 튜토리얼 
    [SerializeField]
    private One_OneStageTutorial one_OneStageTutorial; // 1-1스테이지 튜토리얼 
    [SerializeField]
    private One_TwoStageTutorial one_TwoStageTutorial; // 1-2스테이지 튜토리얼 
    [SerializeField]
    private One_ThreeStageTutorial one_ThreeStageTutorial; // 1-3스테이지 튜토리얼 
    [SerializeField]
    private One_FourStageTutorial one_FourStageTutorial; // 1-4스테이지 튜토리얼 
    [SerializeField]
    private One_FiveStageTutorial one_FiveStageTutorial; // 1-5스테이지 튜토리얼 

    private AbstractStageTutorial currentStageTutorial; // 현재 튜토리얼 
    [SerializeField]
    private BattleStageType _currentBattleStageType;
    public BattleStageType CurrentBattleStageType => _currentBattleStageType; 

    private bool _isPause = false;
    public static bool _isTutorial = false; // 튜토리얼 했는지 
    private void Start()
    {
        //SetTutorial(); 
        EventManager.Instance.StartListening(EventsType.NextExplain, NextExplain);
    }

    [ContextMenu("테스트")]
    private void Test()
    {
        tutorialEventQueue.Clear();
        _currentBattleStageType = BattleStageType.S1_1;
        currentStageTutorial = one_OneStageTutorial;

        tutorialEventQueue.Enqueue(StartTutorial);
        currentStageTutorial.Initialize(_impactParent[1]);
        currentStageTutorial.SetQueue();

        tutorialEventQueue.Dequeue().Invoke(); 
    }
    /// <summary>
    /// 큐에 할 게 들어있는지 
    /// </summary>
    public static bool CheckQueue()
    {
        if (tutorialEventQueue.Count == 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 현재 스테이지에 따른 튜토리얼 설정 
    /// </summary>
    /// <param name="tutorialType"></param>
    /// SceneLoadButtonManager의 SetBattleLoadButton에서 이벤트로 설정해줄거임
    [ContextMenu("튜토리얼")]
    public void SetTutorial()
    {
        tutorialEventQueue.Clear(); 
        //_currentBattleStageType = _currentStageSO._currentStageDatas._stageType; // 현재 몇 스테이지인지 받아옴
        switch (_currentBattleStageType)
        {
            case BattleStageType.ST_MAKE: 
                currentStageTutorial = ST_MakeStageTutorial;
                break; 
            case BattleStageType.S1_1:
                currentStageTutorial = one_OneStageTutorial;
                break;
            case BattleStageType.S1_2:
                currentStageTutorial = one_TwoStageTutorial;
                break;
            case BattleStageType.S1_3:
                currentStageTutorial = one_ThreeStageTutorial;
                break;
            case BattleStageType.S1_4:
                currentStageTutorial = one_FourStageTutorial;
                break; 
            case BattleStageType.S1_5:
                currentStageTutorial = one_FiveStageTutorial;
                break;
            default: // 튜토리얼이 있는 스테이지가 아니면 리턴 
                return; 
        }
        tutorialEventQueue.Enqueue(StartTutorial);
        currentStageTutorial.Initialize(_impactParent[(int)_currentBattleStageType]); 
        currentStageTutorial.SetQueue();

        tutorialEventQueue.Dequeue().Invoke(); 
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
        TutorialCanvas.gameObject.SetActive(!TutorialCanvas.gameObject.activeSelf); 
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
