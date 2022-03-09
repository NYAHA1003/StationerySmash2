using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleManager : MonoBehaviour
{
    #region �����͵�

    [Header("���� �����͵�")]
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

    #region ī�� �ý��� Battle_Card

    public Battle_Card battle_Card { get; private set;}

    [Header("ī��ý��� Battle_Card")]
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

    [Header("��� ���̵� ���� ��ȯ ����")]
    public bool isAnySummon;

    #endregion

    #region ���� �ý��� Battle_Unit

    public Battle_Unit battle_Unit { get; private set; }

    [Header("���ֽý��� Battle_Unit")]
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

    #region ī�޶� �ý��� Battle_Camera

    public Battle_Camera battle_Camera { get; private set; }

    [Header("ī�޶�ý��� Battle_Card")]
    [Space(30)]
    [SerializeField]
    public Camera main_Cam;

    #endregion

    #region ����Ʈ �ý��� Battle_Effect

    public Battle_Effect battle_Effect { get; private set; }
    [Header("����Ʈ �ý���")]
    [Space(30)]
    [SerializeField]
    private Transform effect_PoolManager;


    #endregion

    #region ������ �ý��� Battle_Throw

    public Battle_Throw battle_Throw { get; private set; }
    [Header("������ �ý���")]
    [Space(30)]
    [SerializeField]
    private LineRenderer throw_parabola;
    [SerializeField]
    private Transform throw_Arrow;

    #endregion

    #region �ð� �ý��� Battle_Time

    public Battle_Time battle_Time { get; private set; }

    [Header("�ð��ý��� Battle_Time")]
    [Space(30)]
    public TextMeshProUGUI time_TimeText;


    #endregion

    #region �������� AI �ý��� Battle_AI

    public Battle_AI battle_AI;

    [Header("AI �ý���")]
    [Space(30)]
    public bool ai_isActive;


    #endregion

    #region �ڽ�Ʈ �ý��� Battle_Cost

    public Battle_Cost battle_Cost { get; private set; }
    [Header("�ڽ�Ʈ �ý��� Battle_Cost")]
    [Space(30)]
    public TextMeshProUGUI cost_CostText;

    #endregion

    #region ���� �ý��� Battle_PencilCase

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
        //�ð� �ý���
        battle_Time.Update_Time();

        //ī�޶� ��ġ, ũ�� ����
        battle_Camera.Update_CameraPos();
        battle_Camera.Update_CameraScale();

        //ī�� �ý���
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

        //������ �ý���
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

        //�ڽ�Ʈ �ý���
        battle_Cost.Update_Cost();

    }

    #region �����Լ�

    /// <summary>
    /// ������Ʈ ����
    /// </summary>
    /// <param name="prefeb">������ ������</param>
    /// <param name="position">������ ��ġ</param>
    /// <param name="quaternion">������ ���� ����</param>
    /// <returns></returns>
    public GameObject Create_Object(GameObject prefeb, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(prefeb, position, quaternion);
    }

    #endregion

    #region ���� �ý��� �Լ� Battle_Unit

    /// <summary>
    /// ��ư�Լ�. ������ ��ȯ�� ���� ��
    /// </summary>
    public void Change_Team()
    {
        if(battle_Unit.eTeam == Utill.TeamType.MyTeam)
        {
            battle_Unit.eTeam = Utill.TeamType.EnemyTeam;
            unit_teamText.text = "���� ��";
            return;
        }
        if (battle_Unit.eTeam == Utill.TeamType.EnemyTeam)
        {
            battle_Unit.eTeam = Utill.TeamType.MyTeam;
            unit_teamText.text = "���� ��";
            return;
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="unit">������ ����</param>
    public void Pool_DeleteUnit(Unit unit)
    {
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(unit_PoolManager);
    }

    #endregion

    #region �ڽ�Ʈ �ý��� �Լ� Battle_Cost

    public void Run_UpgradeCostGrade()
    {
        battle_Cost.Run_UpgradeCostGrade();
    }

    #endregion

    #region ���� �ý��� �Լ� Battle_PencilCase

    public void Run_PencilCaseAbility()
    {
        battle_PenCase.Run_PencilCaseAbility();
    }

    #endregion
}
