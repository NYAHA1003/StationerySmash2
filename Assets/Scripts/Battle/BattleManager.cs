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
    #region 데이터들

    [SerializeField, Header("공용 데이터들"), Space(30)]
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

    #region 카드 시스템

    public CardCommand CommandCard { get; private set;}

    [SerializeField, Header("카드시스템 BattleCard"), Space(30)]
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

    [Header("어느 곳이든 유닛 소환 가능")]
    public bool isAnySummon = false;

    #endregion

    #region 유닛 시스템

    public UnitCommand CommandUnit { get; private set; }

    [SerializeField, Header("유닛시스템 BattleUnit"), Space(30)]
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

    #region 카메라 시스템

    public CameraCommand CommandCamera { get; private set; }

    [SerializeField, Header("카메라시스템 BattleCamera"), Space(30)]
    private Camera _mainCam;

    #endregion

    #region 이펙트 시스템

    public EffectCommand CommandEffect { get; private set; }

    [SerializeField, Header("이펙트 시스템 BattleEffect"), Space(30)]
    private Transform _effectPoolManager;
    public List<GameObject> _effectObjectList;

    #endregion

    #region 던지기 시스템

    public ThrowCommand CommandThrow { get; private set; }
    [SerializeField, Header("던지기 시스템 BattleThrow"), Space(30)]
    private LineRenderer _throwParabola;
    [SerializeField]
    private Transform _throwArrow;

    #endregion

    #region 시간 시스템

    public TimeCommand CommandTime { get; private set; }

    [SerializeField, Header("시간시스템 BattleTime"), Space(30)]
    public TextMeshProUGUI _timeText;


    #endregion

    #region 스테이지 AI 시스템

    public AICommand CommandAI;

    [SerializeField, Header("AI 시스템 BattleAi"), Space(30)]
    public bool _isEnemyActive;
    public bool _isPlayerActive;
    [SerializeField]
    public StageLog _aiLog;
    [SerializeField]
    public AIDataSO _aiEnemyDataSO;
    [SerializeField]
    public AIDataSO _aiPlayerDataSO;

    #endregion

    #region 코스트 시스템

    public CostCommand CommandCost { get; private set; }

    [SerializeField, Header("코스트 시스템 BattleCost"), Space(30)]
    public TextMeshProUGUI _costText;

    #endregion

    #region 필통 시스템

    public PencilCaseCommand CommandPencilCase { get; private set; }
    [SerializeField, Header("필통시스템 BattlePencilCase"), Space(30)]
    public Unit _myPencilCase;
    public Unit _enemyPencilCase;
    public PencilCaseDataSO _pencilCaseMyData;
    public PencilCaseData _pencilCaseEnemyData;


    #endregion

    #region 일시정지 시스템

    public PauseCommand CommandPause { get; private set; }

    [SerializeField, Header("일시정지시스템 Battle_Pause"), Space(30)]
    private RectTransform _pauseUI;
    [SerializeField]
    private Canvas _pauseCanvas;

    #endregion

    #region 승리 패배 시스템

    public WinLoseCommand CommandWinLose { get; private set; }
    [SerializeField, Header("승리패배시스템 BattleWinLose"), Space(30)]
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

        //던지기 시스템
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

        //컴포넌트들의 업데이트가 필요한 함수 재생
        _updateAction.Invoke();
        
        //테스트용

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

        //유닛 시스템
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CommandUnit.ClearUnit();
        }
    }

    /// <summary>
    /// 업데이트 액션에 사용할 함수 추가
    /// </summary>
    /// <param name="method"></param>
    public void AddUpdateAction(Action method)
    {
        _updateAction += method;
    }

    /// <summary>
    /// 오브젝트 생성
    /// </summary>
    /// <param name="prefeb">생성할 프리펩</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="quaternion">생성할 때의 각도</param>
    /// <returns></returns>
    public GameObject CreateObject(GameObject prefeb, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(prefeb, position, quaternion);
    }

    /// <summary>
    /// 버튼함수. 유닛을 소환할 때의 팀
    /// </summary>
    public void OnChangeTeam()
    {
        //내 팀인지 적팀인지 체크
        if (CommandUnit.eTeam.Equals(TeamType.MyTeam))
        {
            CommandUnit.eTeam = Utill.TeamType.EnemyTeam;
            _unitTeamText.text = "적의 팀";
            return;
        }
        if (CommandUnit.eTeam.Equals(Utill.TeamType.EnemyTeam))
        {
            CommandUnit.eTeam = Utill.TeamType.MyTeam;
            _unitTeamText.text = "나의 팀";
            return;
        }
    }

    /// <summary>
    /// 유닛 제거
    /// </summary>
    /// <param name="unit">제거할 유닛</param>
    public void PoolDeleteUnit(Unit unit)
    {
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(_unitPoolManager);
    }

    /// <summary>
    /// 클릭하면 코스트 단계 증가
    /// </summary>
    public void OnUpgradeCostGrade()
    {
        CommandCost.UpgradeCostGrade();
    }

    /// <summary>
    /// 클릭하면 필통 능력 사용
    /// </summary>
    public void OnPencilCaseAbility()
    {
        CommandPencilCase.RunPencilCaseAbility();
    }

    /// <summary>
    /// 클릭하면 일시정지함
    /// </summary>
    public void OnPause()
    {
        CommandPause.SetPause();
    }
}
