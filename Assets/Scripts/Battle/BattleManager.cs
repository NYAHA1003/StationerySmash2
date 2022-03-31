using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;
using TMPro;
using Battle;
using System;


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
    private Action _updateAction = default;

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

    #region ī�� �ý���

    public CardCommand CommandCard { get; private set;}

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

    #region ���� �ý���

    public UnitCommand CommandUnit { get; private set; }

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

    #region ī�޶� �ý���

    public CameraCommand CommandCamera { get; private set; }

    [SerializeField, Header("ī�޶�ý��� BattleCamera"), Space(30)]
    private Camera _mainCam;

    #endregion

    #region ����Ʈ �ý���

    public EffectCommand CommandEffect { get; private set; }

    [SerializeField, Header("����Ʈ �ý��� BattleEffect"), Space(30)]
    private Transform _effectPoolManager;
    public List<GameObject> _effectObjectList;

    #endregion

    #region ������ �ý���

    public ThrowCommand CommandThrow { get; private set; }
    [SerializeField, Header("������ �ý��� BattleThrow"), Space(30)]
    private LineRenderer _throwParabola;
    [SerializeField]
    private Transform _throwArrow;

    #endregion

    #region �ð� �ý���

    public TimeCommand CommandTime { get; private set; }

    [SerializeField, Header("�ð��ý��� BattleTime"), Space(30)]
    public TextMeshProUGUI _timeText;


    #endregion

    #region �������� AI �ý���

    public AICommand CommandAI;

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

    #region �ڽ�Ʈ �ý���

    public CostCommand CommandCost { get; private set; }

    [SerializeField, Header("�ڽ�Ʈ �ý��� BattleCost"), Space(30)]
    public TextMeshProUGUI _costText;

    #endregion

    #region ���� �ý���

    public PencilCaseCommand CommandPencilCase { get; private set; }
    [SerializeField, Header("����ý��� BattlePencilCase"), Space(30)]
    public Unit _myPencilCase;
    public Unit _enemyPencilCase;
    public PencilCaseDataSO _pencilCaseMyData;
    public PencilCaseData _pencilCaseEnemyData;


    #endregion

    #region �Ͻ����� �ý���

    public PauseCommand CommandPause { get; private set; }

    [SerializeField, Header("�Ͻ������ý��� Battle_Pause"), Space(30)]
    private RectTransform _pauseUI;
    [SerializeField]
    private Canvas _pauseCanvas;

    #endregion

    #region �¸� �й� �ý���

    public WinLoseCommand CommandWinLose { get; private set; }
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
        CommandCard = new CardCommand();
        CommandCamera = new CameraCommand();
        CommandUnit = new UnitCommand();
        CommandEffect = new EffectCommand();
        CommandThrow = new ThrowCommand();
        CommandAI = new AICommand();
        CommandTime = new TimeCommand();
        CommandCost = new CostCommand();
        CommandPencilCase = new PencilCaseCommand();
        CommandPause = new PauseCommand();
        CommandWinLose = new WinLoseCommand();

        CommandCard.SetInitialization(this, _deckData, _unitDataSO, _starategyDataSO, _cardMovePrefeb, _cardPoolManager, _cardCanvas, _cardSpawnPosition, _cardLeftPosition, _cardRightPosition, _cardAfterImage, _cardSummonRangeLine);
        CommandCamera.SetInitialization(this, _mainCam);
        CommandUnit.SetInitialization(this, _unitPrefeb, _unitPoolManager, _unitParent);
        CommandEffect.SetInitialization(this, _effectPoolManager);
        CommandThrow.SetInitialization(this, _throwParabola, _throwArrow, CurrentStageData);
        CommandAI.SetInitialization(this, _aiEnemyDataSO, _aiPlayerDataSO, _isEnemyActive, _isPlayerActive);
        CommandTime.SetInitialization(this, _timeText);
        CommandCost.SetInitialization(this, _costText);
        CommandPencilCase.SetInitialization(this, _myPencilCase, _enemyPencilCase, _pencilCaseMyData.pencilCaseData, _pencilCaseEnemyData);
        CommandPause.SetInitialization(this, _pauseUI, _pauseCanvas);
        CommandWinLose.SetInitialization(this, _winLoseCanvas, _winPanel, _losePanel);

        _pencilCaseEnemyData = _stageDataSO.enemyPencilCase;

        CommandCost.SetCostSpeed(_pencilCaseMyData.pencilCaseData.costSpeed);
        CommandCard.SetMaxCard(_pencilCaseMyData.pencilCaseData.maxCard);

        _isEndSetting = true;
    }

    private void Update()
    {
        if (!_isEndSetting)
        {
            return;
        }

        //������ �ý���
        if(Input.GetMouseButtonDown(0))
        {
            CommandThrow.PullUnit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButton(0))
        {
            CommandThrow.DrawParabola(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            CommandThrow.ThrowUnit();
        }

        //������Ʈ���� ������Ʈ�� �ʿ��� �Լ� ���
        _updateAction.Invoke();
        
        //�׽�Ʈ��

        if (Input.GetKeyDown(KeyCode.X))
        {
            CommandCard.AddOneCard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CommandCard.AddAllCard();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CommandCard.ClearCards();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CommandCard.SubtractLastCard();
        }

        //���� �ý���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CommandUnit.ClearUnit();
        }
    }

    /// <summary>
    /// ������Ʈ �׼ǿ� ����� �Լ� �߰�
    /// </summary>
    /// <param name="method"></param>
    public void AddUpdateAction(Action method)
    {
        _updateAction += method;
    }

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

    /// <summary>
    /// ��ư�Լ�. ������ ��ȯ�� ���� ��
    /// </summary>
    public void OnChangeTeam()
    {
        //�� ������ �������� üũ
        if (CommandUnit.eTeam.Equals(TeamType.MyTeam))
        {
            CommandUnit.eTeam = Utill.TeamType.EnemyTeam;
            _unitTeamText.text = "���� ��";
            return;
        }
        if (CommandUnit.eTeam.Equals(Utill.TeamType.EnemyTeam))
        {
            CommandUnit.eTeam = Utill.TeamType.MyTeam;
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

    /// <summary>
    /// Ŭ���ϸ� �ڽ�Ʈ �ܰ� ����
    /// </summary>
    public void OnUpgradeCostGrade()
    {
        CommandCost.UpgradeCostGrade();
    }

    /// <summary>
    /// Ŭ���ϸ� ���� �ɷ� ���
    /// </summary>
    public void OnPencilCaseAbility()
    {
        CommandPencilCase.RunPencilCaseAbility();
    }

    /// <summary>
    /// Ŭ���ϸ� �Ͻ�������
    /// </summary>
    public void OnPause()
    {
        CommandPause.SetPause();
    }
}
