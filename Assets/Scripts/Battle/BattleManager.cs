using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;
using TMPro;

public class BattleManager : MonoBehaviour
{
    #region 데이터들

    [SerializeField, Header("공용 데이터들"), Space(30)]
    private UnitDataSO unitDataSO;
    [SerializeField]
    public StageDataSO stageDataSO;
    [SerializeField]
    public StarategyDataSO  starategyDataSO;
    [SerializeField]
    public DeckData deckData;

    private bool isEndSetting = false;

    public StageData currentStageData
    {
        get
        {
            return stageDataSO.stageDatas[0];
        }

        private set {}
    }

    #endregion

    #region 카드 시스템 Battle_Card

    public Battle_Card battle_Card { get; private set;}

    [SerializeField, Header("카드시스템 Battle_Card"), Space(30)]
    public List<CardMove> card_DatasTemp;
    [SerializeField]
    private GameObject card_cardMove_Prefeb;
    [SerializeField]
    private Transform card_PoolManager;
    [SerializeField]
    private Transform card_Canvas;
    [SerializeField]
    private RectTransform card_SpawnPosition;
    [SerializeField]
    private RectTransform card_LeftPosition;
    [SerializeField]
    private RectTransform card_RightPosition;
    [SerializeField]
    private GameObject card_AfterImage;
    [SerializeField]
    private LineRenderer card_SummonRangeLine;

    [Header("어느 곳이든 유닛 소환 가능")]
    public bool isAnySummon;

    #endregion

    #region 유닛 시스템 Battle_Unit

    public Battle_Unit battle_Unit { get; private set; }

    [SerializeField, Header("유닛시스템 Battle_Unit"), Space(30)]
    public List<Unit> unit_MyDatasTemp;
    public List<Unit> unit_EnemyDatasTemp;
    [SerializeField]
    private GameObject unit_Prefeb;
    [SerializeField]
    private Transform unit_PoolManager;
    [SerializeField]
    private Transform unit_Parent;

    public TextMeshProUGUI unit_teamText;

    #endregion

    #region 카메라 시스템 Battle_Camera

    public Battle_Camera battle_Camera { get; private set; }

    [SerializeField, Header("카메라시스템 Battle_Camera"), Space(30)]
    public Camera main_Cam;

    #endregion

    #region 이펙트 시스템 Battle_Effect

    public Battle_Effect battle_Effect { get; private set; }

    [SerializeField, Header("이펙트 시스템 Battle_Effect"), Space(30)]
    private Transform effect_PoolManager;
    public List<GameObject> effect_ObjList;

    #endregion

    #region 던지기 시스템 Battle_Throw

    public Battle_Throw battle_Throw { get; private set; }
    [SerializeField, Header("던지기 시스템 Battle_Throw"), Space(30)]
    private LineRenderer throw_parabola;
    [SerializeField]
    private Transform throw_Arrow;

    #endregion

    #region 시간 시스템 Battle_Time

    public Battle_Time battle_Time { get; private set; }

    [SerializeField, Header("시간시스템 Battle_Time"), Space(30)]
    public TextMeshProUGUI time_TimeText;


    #endregion

    #region 스테이지 AI 시스템 Battle_AI

    public Battle_AI battle_AI;

    [SerializeField, Header("AI 시스템 Battle_Ai"), Space(30)]
    public bool ai_isEnemyActive;
    public bool ai_isPlayerActive;
    [SerializeField]
    public StageLog ai_Log;
    [SerializeField]
    public AIDataSO ai_EnemyDataSO;
    [SerializeField]
    public AIDataSO ai_PlayerDataSO;

    #endregion

    #region 코스트 시스템 Battle_Cost

    public Battle_Cost battle_Cost { get; private set; }

    [SerializeField, Header("코스트 시스템 Battle_Cost"), Space(30)]
    public TextMeshProUGUI cost_CostText;

    #endregion

    #region 필통 시스템 Battle_PencilCase

    public Battle_PencilCase battle_PencilCase { get; private set; }
    [SerializeField, Header("필통시스템 Battle_PencilCase"), Space(30)]
    public Unit pencilCase_My;
    public Unit pencilCase_Enemy;
    public PencilCaseDataSO pencilCase_MyData;
    public PencilCaseData pencilCase_EnemyData;


    #endregion

    #region 일시정지 시스템 Battle_Pause

    public Battle_Pause battle_Pause { get; private set; }

    [SerializeField, Header("일시정지시스템 Battle_Pause"), Space(30)]
    private RectTransform pause_UI;
    [SerializeField]
    private Canvas pause_Canvas;

    #endregion

    #region 승리 패배 시스템 Battle_WinLose

    public Battle_WinLose battle_WinLose { get; private set; }
    [SerializeField, Header("승리패배시스템 Battle_WinLose"), Space(30)]
    private Canvas winLose_Canvas;
    [SerializeField]
    private RectTransform winlose_winPanel;
    [SerializeField]
    private RectTransform winlose_losePanel;

    #endregion

    private void Start()
    {
        deckData = new DeckData();
        battle_Card = new Battle_Card(this, deckData, unitDataSO, starategyDataSO, card_cardMove_Prefeb, card_PoolManager, card_Canvas, card_SpawnPosition, card_LeftPosition, card_RightPosition, card_AfterImage, card_SummonRangeLine);
        battle_Camera = new Battle_Camera(this, main_Cam);
        battle_Unit = new Battle_Unit(this, unit_Prefeb, unit_PoolManager, unit_Parent);
        battle_Effect = new Battle_Effect(this, effect_PoolManager);
        battle_Throw = new Battle_Throw(this, throw_parabola, throw_Arrow, currentStageData);
        battle_AI = new Battle_AI(this, ai_EnemyDataSO, ai_PlayerDataSO, ai_isEnemyActive, ai_isPlayerActive);
        battle_Time = new Battle_Time(this, time_TimeText);
        battle_Cost = new Battle_Cost(this, cost_CostText);
        pencilCase_EnemyData = stageDataSO.enemyPencilCase;
        battle_PencilCase = new Battle_PencilCase(this, pencilCase_My, pencilCase_Enemy, pencilCase_MyData.pencilCaseData, pencilCase_EnemyData);
        battle_Pause = new Battle_Pause(this, pause_UI, pause_Canvas);
        battle_WinLose = new Battle_WinLose(this, winLose_Canvas, winlose_winPanel, winlose_losePanel);

        battle_Cost.Set_CostSpeed(pencilCase_MyData.pencilCaseData.costSpeed);
        battle_Card.Set_MaxCard(pencilCase_MyData.pencilCaseData.maxCard);

        isEndSetting = true;
    }

    private void Update()
    {
        if (!isEndSetting)
            return;

        //시간 시스템
        battle_Time.Update_Time();

        //카메라 위치, 크기 조정
        battle_Camera.Update_CameraPos();
        battle_Camera.Update_CameraScale();

        //카드 시스템
        battle_Card.Update_UnitAfterImage();
        battle_Card.Update_SelectCardPos();
        battle_Card.Update_CardDrow();
        battle_Card.Check_PossibleSummon();
        battle_Card.Update_SummonRange();
        if (Input.GetKeyDown(KeyCode.X))
        {
            battle_Card.Add_OneCard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            battle_Card.Add_AllCard();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            battle_Card.Clear_Cards();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            battle_Card.Subtract_Card();
        }

        //유닛 시스템
        if (Input.GetKeyDown(KeyCode.Q))
        {
            battle_Unit.Clear_Unit();
        }

        //던지기 시스템
        if(Input.GetMouseButtonDown(0))
        {
            battle_Throw.Pull_Unit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButton(0))
        {
            battle_Throw.Draw_Parabola(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            battle_Throw.Throw_Unit();
        }

        //코스트 시스템
        battle_Cost.Update_Cost();

        //AI 시스템
        battle_AI.Update_EnemyAICard();
        battle_AI.Update_EnemyAIThrow();
        battle_AI.Update_PlayerAICard();
        battle_AI.Update_PlayerAIThrow();

    }

    #region 공용함수

    /// <summary>
    /// 오브젝트 생성
    /// </summary>
    /// <param name="prefeb">생성할 프리펩</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="quaternion">생성할 때의 각도</param>
    /// <returns></returns>
    public GameObject Create_Object(GameObject prefeb, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(prefeb, position, quaternion);
    }

    #endregion

    #region 유닛 시스템 함수 Battle_Unit

    /// <summary>
    /// 버튼함수. 유닛을 소환할 때의 팀
    /// </summary>
    public void Change_Team()
    {
        if(battle_Unit.eTeam.Equals(TeamType.MyTeam))
        {
            battle_Unit.eTeam = Utill.TeamType.EnemyTeam;
            unit_teamText.text = "적의 팀";
            return;
        }
        if (battle_Unit.eTeam.Equals(Utill.TeamType.EnemyTeam))
        {
            battle_Unit.eTeam = Utill.TeamType.MyTeam;
            unit_teamText.text = "나의 팀";
            return;
        }
    }

    /// <summary>
    /// 유닛 제거
    /// </summary>
    /// <param name="unit">제거할 유닛</param>
    public void Pool_DeleteUnit(Unit unit)
    {
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(unit_PoolManager);
    }

    #endregion

    #region 코스트 시스템 함수 Battle_Cost

    public void Run_UpgradeCostGrade()
    {
        battle_Cost.Run_UpgradeCostGrade();
    }

    #endregion

    #region 필통 시스템 함수 Battle_PencilCase

    public void Run_PencilCaseAbility()
    {
        battle_PencilCase.Run_PencilCaseAbility();
    }

    #endregion

    #region 일시정지 시스템 함수 Battle_Pause

    public void Set_Pause()
    {
        battle_Pause.Set_Pause();
    }

    #endregion
}
