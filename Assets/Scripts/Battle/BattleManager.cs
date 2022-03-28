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
    private Canvas _winLoseCanvas;
    [SerializeField]
    private RectTransform _winPanel;
    [SerializeField]
    private RectTransform _losePanel;

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
        BattlePencilCase = new BattlePencilCase(this, _myPencilCase, _enemyPencilCase, _pencilCaseMyData.pencilCaseData, _pencilCaseEnemyData);
        BattlePause = new BattlePause(this, _pauseUI, _pauseCanvas);
        BattleWinLose = new BattleWinLose(this, _winLoseCanvas, _winPanel, _losePanel);
        _pencilCaseEnemyData = _stageDataSO.enemyPencilCase;

        BattleCost.SetCostSpeed(_pencilCaseMyData.pencilCaseData.costSpeed);
        BattleCard.SetMaxCard(_pencilCaseMyData.pencilCaseData.maxCard);

        _isEndSetting = true;
    }

    private void Update()
    {
        if (!_isEndSetting)
            return;

        //�ð� �ý���
        BattleTime.UpdateTime();

        //ī�޶� ��ġ, ũ�� ����
        BattleCamera.UpdateCameraPos();
        BattleCamera.UpdateCameraScale();

        //ī�� �ý���
        BattleCard.UpdateUnitAfterImage();
        BattleCard.UpdateSelectCardPos();
        BattleCard.UpdateCardDrow();
        BattleCard.CheckPossibleSummon();
        BattleCard.UpdateSummonRange();
        if (Input.GetKeyDown(KeyCode.X))
        {
            BattleCard.AddOneCard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BattleCard.AddAllCard();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            BattleCard.ClearCards();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            BattleCard.SubtractCard();
        }

        //���� �ý���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BattleUnit.ClearUnit();
        }

        //������ �ý���
        if(Input.GetMouseButtonDown(0))
        {
            BattleThrow.PullUnit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButton(0))
        {
            BattleThrow.DrawParabola(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            BattleThrow.ThrowUnit();
        }

        //�ڽ�Ʈ �ý���
        BattleCost.UpdateCost();

        //AI �ý���
        BattleAI.UpdateEnemyAICard();
        BattleAI.UpdateEnemyAIThrow();
        BattleAI.UpdatePlayerAICard();
        BattleAI.UpdatePlayerAIThrow();

    }

    #region �����Լ�

    /// <summary>
    /// ������Ʈ ����
    /// </summary>
    /// <param name="prefeb">������ ������</param>
    /// <param name="position">������ ��ġ</param>
    /// <param name="quaternion">������ ���� ����</param>
    /// <returns></returns>
    public GameObject CreateObject(GameObject prefeb, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(prefeb, position, quaternion);
    }

    #endregion

    #region ���� �ý��� �Լ� BattleUnit

    /// <summary>
    /// ��ư�Լ�. ������ ��ȯ�� ���� ��
    /// </summary>
    public void ChangeTeam()
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
    public void PoolDeleteUnit(Unit unit)
    {
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(_unitPoolManager);
    }

    #endregion

    #region �ڽ�Ʈ �ý��� �Լ� BattleCost

    public void RunUpgradeCostGrade()
    {
        BattleCost.RunUpgradeCostGrade();
    }

    #endregion

    #region ���� �ý��� �Լ� BattlePencilCase

    public void RunPencilCaseAbility()
    {
        BattlePencilCase.RunPencilCaseAbility();
    }

    #endregion

    #region �Ͻ����� �ý��� �Լ� BattlePause

    public void SetPause()
    {
        BattlePause.SetPause();
    }

    #endregion
}
