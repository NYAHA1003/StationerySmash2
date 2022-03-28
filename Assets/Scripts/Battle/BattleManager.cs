using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;
using TMPro;

public class BattleManager : MonoBehaviour
{
    #region �����͵�

    [SerializeField, Header("���� �����͵�"), Space(30)]
    private UnitDataSO _unitDataSO = null;
    [SerializeField]
    public StageDataSO _stageDataSO = null;
    [SerializeField]
    public StarategyDataSO _starategyDataSO = null;
    [SerializeField]
    public DeckData _deckData = null;
    private bool _isEndSetting = false;

    public StageData CurrentStageData
    {
        get
        {
            return _stageDataSO.stageDatas[0];
        }

        private set 
        {
        }
    }

    #endregion

    #region ī�� �ý��� BattleCard

    public BattleCard BattleCard { get; private set;}

    [SerializeField, Header("ī��ý��� BattleCard"), Space(30)]
    public List<CardMove> _cardDatasTemp = new List<CardMove>();
    [SerializeField]
    private GameObject _cardMovePrefeb = null;
    [SerializeField]
    private Transform _cardPoolManager = null;
    [SerializeField]
    private Transform _cardCanvas = null;
    [SerializeField]
    private RectTransform _cardSpawnPosition = null;
    [SerializeField]
    private RectTransform _cardLeftPosition = null;
    [SerializeField]
    private RectTransform _cardRightPosition = null;
    [SerializeField]
    private GameObject _cardAfterImage = null;
    [SerializeField]
    private LineRenderer _cardSummonRangeLine = null;

    [Header("��� ���̵� ���� ��ȯ ����")]
    public bool isAnySummon = false;

    #endregion

    #region ���� �ý��� BattleUnit

    public BattleUnit BattleUnit { get; private set; }

    [SerializeField, Header("���ֽý��� BattleUnit"), Space(30)]
    public List<Unit> _myUnitDatasTemp;
    public List<Unit> _enemyUnitDatasTemp;
    [SerializeField]
    private GameObject _unitPrefeb;
    [SerializeField]
    private Transform _unitPoolManager;
    [SerializeField]
    private Transform _unitParent;

    public TextMeshProUGUI _unitTeamText;

    #endregion

    #region ī�޶� �ý��� BattleCamera

    public BattleCamera BattleCamera { get; private set; }

    [SerializeField, Header("ī�޶�ý��� BattleCamera"), Space(30)]
    private Camera _mainCam;

    #endregion

    #region ����Ʈ �ý��� BattleEffect

    public BattleEffect BattleEffect { get; private set; }

    [SerializeField, Header("����Ʈ �ý��� BattleEffect"), Space(30)]
    private Transform _effectPoolManager;
    public List<GameObject> _effectObjectList;

    #endregion

    #region ������ �ý��� BattleThrow

    public BattleThrow BattleThrow { get; private set; }
    [SerializeField, Header("������ �ý��� BattleThrow"), Space(30)]
    private LineRenderer _throwParabola;
    [SerializeField]
    private Transform _throwArrow;

    #endregion

    #region �ð� �ý��� BattleTime

    public BattleTime BattleTime { get; private set; }

    [SerializeField, Header("�ð��ý��� BattleTime"), Space(30)]
    public TextMeshProUGUI _timeText;


    #endregion

    #region �������� AI �ý��� Battle_AI

    public BattleAI BattleAI;

    [SerializeField, Header("AI �ý��� BattleAi"), Space(30)]
    public bool _isEnemyActive;
    public bool _isPlayerActive;
    [SerializeField]
    public StageLog _aiLog;
    [SerializeField]
    public AIDataSO _aiEnemyDataSO;
    [SerializeField]
    public AIDataSO _aiPlayerDataSO;

    #endregion

    #region �ڽ�Ʈ �ý��� BattleCost

    public BattleCost BattleCost { get; private set; }

    [SerializeField, Header("�ڽ�Ʈ �ý��� BattleCost"), Space(30)]
    public TextMeshProUGUI _costText;

    #endregion

    #region ���� �ý��� BattlePencilCase

    public BattlePencilCase BattlePencilCase { get; private set; }
    [SerializeField, Header("����ý��� BattlePencilCase"), Space(30)]
    public Unit _myPencilCase;
    public Unit _enemyPencilCase;
    public PencilCaseDataSO _pencilCaseMyData;
    public PencilCaseData _pencilCaseEnemyData;


    #endregion

    #region �Ͻ����� �ý��� Battle_Pause

    public BattlePause BattlePause { get; private set; }

    [SerializeField, Header("�Ͻ������ý��� Battle_Pause"), Space(30)]
    private RectTransform _pauseUI;
    [SerializeField]
    private Canvas _pauseCanvas;

    #endregion

    #region �¸� �й� �ý��� Battle_WinLose

    public BattleWinLose BattleWinLose { get; private set; }
    [SerializeField, Header("�¸��й�ý��� BattleWinLose"), Space(30)]
    private Canvas winLoseCanvas;
    [SerializeField]
    private RectTransform winPanel;
    [SerializeField]
    private RectTransform losePanel;

    #endregion

    private void Start()
    {
        _deckData = new DeckData();
        BattleCard = new BattleCard(this, _deckData, _unitDataSO, _starategyDataSO, _cardMovePrefeb, _cardPoolManager, _cardCanvas, _cardSpawnPosition, _cardLeftPosition, _cardRightPosition, _cardAfterImage, _cardSummonRangeLine);
        BattleCamera = new BattleCamera(this, _mainCam);
        BattleUnit = new BattleUnit(this, _unitPrefeb, _unitPoolManager, _unitParent);
        BattleEffect = new BattleEffect(this, _effectPoolManager);
        BattleThrow = new BattleThrow(this, _throwParabola, _throwArrow, CurrentStageData);
        BattleAI = new BattleAI(this, _aiEnemyDataSO, _aiPlayerDataSO, _isEnemyActive, _isPlayerActive);
        BattleTime = new BattleTime(this, _timeText);
        BattleCost = new BattleCost(this, _costText);
        _pencilCaseEnemyData = _stageDataSO.enemyPencilCase;
        BattlePencilCase = new BattlePencilCase(this, _myPencilCase, _enemyPencilCase, _pencilCaseMyData.pencilCaseData, _pencilCaseEnemyData);
        BattlePause = new BattlePause(this, _pauseUI, _pauseCanvas);
        BattleWinLose = new BattleWinLose(this, winLoseCanvas, winPanel, losePanel);

        BattleCost.Set_CostSpeed(_pencilCaseMyData.pencilCaseData.costSpeed);
        BattleCard.Set_MaxCard(_pencilCaseMyData.pencilCaseData.maxCard);

        _isEndSetting = true;
    }

    private void Update()
    {
        if (!_isEndSetting)
            return;

        //�ð� �ý���
        BattleTime.Update_Time();

        //ī�޶� ��ġ, ũ�� ����
        BattleCamera.Update_CameraPos();
        BattleCamera.Update_CameraScale();

        //ī�� �ý���
        BattleCard.Update_UnitAfterImage();
        BattleCard.Update_SelectCardPos();
        BattleCard.Update_CardDrow();
        BattleCard.Check_PossibleSummon();
        BattleCard.Update_SummonRange();
        if (Input.GetKeyDown(KeyCode.X))
        {
            BattleCard.Add_OneCard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BattleCard.Add_AllCard();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            BattleCard.Clear_Cards();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            BattleCard.Subtract_Card();
        }

        //���� �ý���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BattleUnit.Clear_Unit();
        }

        //������ �ý���
        if(Input.GetMouseButtonDown(0))
        {
            BattleThrow.Pull_Unit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButton(0))
        {
            BattleThrow.Draw_Parabola(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            BattleThrow.Throw_Unit();
        }

        //�ڽ�Ʈ �ý���
        BattleCost.Update_Cost();

        //AI �ý���
        BattleAI.Update_EnemyAICard();
        BattleAI.Update_EnemyAIThrow();
        BattleAI.Update_PlayerAICard();
        BattleAI.Update_PlayerAIThrow();

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
        if(BattleUnit.eTeam.Equals(TeamType.MyTeam))
        {
            BattleUnit.eTeam = Utill.TeamType.EnemyTeam;
            _unitTeamText.text = "���� ��";
            return;
        }
        if (BattleUnit.eTeam.Equals(Utill.TeamType.EnemyTeam))
        {
            BattleUnit.eTeam = Utill.TeamType.MyTeam;
            _unitTeamText.text = "���� ��";
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
        unit.transform.SetParent(_unitPoolManager);
    }

    #endregion

    #region �ڽ�Ʈ �ý��� �Լ� Battle_Cost

    public void Run_UpgradeCostGrade()
    {
        BattleCost.Run_UpgradeCostGrade();
    }

    #endregion

    #region ���� �ý��� �Լ� Battle_PencilCase

    public void Run_PencilCaseAbility()
    {
        BattlePencilCase.Run_PencilCaseAbility();
    }

    #endregion

    #region �Ͻ����� �ý��� �Լ� Battle_Pause

    public void Set_Pause()
    {
        BattlePause.Set_Pause();
    }

    #endregion
}
