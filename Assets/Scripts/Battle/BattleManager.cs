using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleManager : MonoBehaviour
{
    #region 데이터들

    [Header("공용 데이터들")]
    [Space(30)]
    [SerializeField]
    private UnitDataSO unitDataSO;
    public PencilCaseDataSO pencilCaseDataSO;
    [SerializeField]
    private StageDataSO stageDataSO;

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

    [Header("카드시스템 Battle_Card")]
    [Space(30)]
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

    [Header("유닛시스템 Battle_Unit")]
    [Space(30)]
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

    [Header("카메라시스템 Battle_Card")]
    [Space(30)]
    [SerializeField]
    public Camera main_Cam;

    #endregion

    #region 이펙트 시스템 Battle_Effect

    public Battle_Effect battle_Effect { get; private set; }
    [Header("이펙트 시스템")]
    [Space(30)]
    [SerializeField]
    private Transform effect_PoolManager;


    #endregion

    #region 던지기 시스템 Battle_Throw

    public Battle_Throw battle_Throw { get; private set; }
    [Header("던지기 시스템")]
    [Space(30)]
    [SerializeField]
    private LineRenderer throw_parabola;
    [SerializeField]
    private Transform throw_Arrow;

    #endregion

    #region 시간 시스템 Battle_Time

    public Battle_Time battle_Time { get; private set; }

    [Header("시간시스템 Battle_Time")]
    [Space(30)]
    public TextMeshProUGUI time_TimeText;


    #endregion

    #region 스테이지 AI 시스템 Battle_AI

    public Battle_AI battle_AI;

    [Header("AI 시스템")]
    [Space(30)]
    public bool ai_isActive;


    #endregion

    #region 코스트 시스템 Battle_Cost

    public Battle_Cost battle_Cost { get; private set; }
    [Header("코스트 시스템 Battle_Cost")]
    [Space(30)]
    public TextMeshProUGUI cost_CostText;

    #endregion

    #region 필통 시스템 Battle_PencilCase

    public Battle_PencilCase battle_PenCase;
    public PencilCase_Unit pencilCase_My;
    public PencilCase_Unit pencilCase_Enemy;


    #endregion

    private void Awake()
    {
        battle_Card = new Battle_Card(this, unitDataSO, card_cardMove_Prefeb, card_PoolManager, card_Canvas, card_SpawnPosition, card_LeftPosition, card_RightPosition, card_AfterImage, card_SummonRangeLine);
        battle_Camera = new Battle_Camera(this, main_Cam);
        battle_Unit = new Battle_Unit(this, unit_Prefeb, unit_PoolManager, unit_Parent);
        battle_Effect = new Battle_Effect(this, effect_PoolManager);
        battle_Throw = new Battle_Throw(this, throw_parabola, throw_Arrow, currentStageData);
        battle_AI = new Battle_AI(this);
        battle_Time = new Battle_Time(this, time_TimeText);
        battle_Cost = new Battle_Cost(this, cost_CostText);
        battle_PenCase = new Battle_PencilCase(this);
    }

    private void Update()
    {
        //시간 시스템
        battle_Time.Update_Time();

        //카메라 위치, 크기 조정
        battle_Camera.Update_CameraPos();
        battle_Camera.Update_CameraScale();

        //카드 시스템
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
        if(battle_Unit.eTeam == Utill.TeamType.MyTeam)
        {
            battle_Unit.eTeam = Utill.TeamType.EnemyTeam;
            unit_teamText.text = "적의 팀";
            return;
        }
        if (battle_Unit.eTeam == Utill.TeamType.EnemyTeam)
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
        battle_PenCase.Run_PencilCaseAbility();
    }

    #endregion
}
