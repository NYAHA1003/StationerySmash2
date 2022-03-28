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

    #region 카드 시스템 BattleCard

    public BattleCard BattleCard { get; private set;}

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

    #region 유닛 시스템 BattleUnit

    public BattleUnit BattleUnit { get; private set; }

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

    #region 카메라 시스템 BattleCamera

    public BattleCamera BattleCamera { get; private set; }

    [SerializeField, Header("카메라시스템 BattleCamera"), Space(30)]
    private Camera _mainCam;

    #endregion

    #region 이펙트 시스템 BattleEffect

    public BattleEffect BattleEffect { get; private set; }

    [SerializeField, Header("이펙트 시스템 BattleEffect"), Space(30)]
    private Transform _effectPoolManager;
    public List<GameObject> _effectObjectList;

    #endregion

    #region 던지기 시스템 BattleThrow

    public BattleThrow BattleThrow { get; private set; }
    [SerializeField, Header("던지기 시스템 BattleThrow"), Space(30)]
    private LineRenderer _throwParabola;
    [SerializeField]
    private Transform _throwArrow;

    #endregion

    #region 시간 시스템 BattleTime

    public BattleTime BattleTime { get; private set; }

    [SerializeField, Header("시간시스템 BattleTime"), Space(30)]
    public TextMeshProUGUI _timeText;


    #endregion

    #region 스테이지 AI 시스템 Battle_AI

    public BattleAI BattleAI;

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

    #region 코스트 시스템 BattleCost

    public BattleCost BattleCost { get; private set; }

    [SerializeField, Header("코스트 시스템 BattleCost"), Space(30)]
    public TextMeshProUGUI _costText;

    #endregion

    #region 필통 시스템 BattlePencilCase

    public BattlePencilCase BattlePencilCase { get; private set; }
    [SerializeField, Header("필통시스템 BattlePencilCase"), Space(30)]
    public Unit _myPencilCase;
    public Unit _enemyPencilCase;
    public PencilCaseDataSO _pencilCaseMyData;
    public PencilCaseData _pencilCaseEnemyData;


    #endregion

    #region 일시정지 시스템 Battle_Pause

    public BattlePause BattlePause { get; private set; }

    [SerializeField, Header("일시정지시스템 Battle_Pause"), Space(30)]
    private RectTransform _pauseUI;
    [SerializeField]
    private Canvas _pauseCanvas;

    #endregion

    #region 승리 패배 시스템 Battle_WinLose

    public BattleWinLose BattleWinLose { get; private set; }
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

        //시간 시스템
        BattleTime.UpdateTime();

        //카메라 위치, 크기 조정
        BattleCamera.UpdateCameraPos();
        BattleCamera.UpdateCameraScale();

        //카드 시스템
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

        //유닛 시스템
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BattleUnit.ClearUnit();
        }

        //던지기 시스템
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

        //코스트 시스템
        BattleCost.UpdateCost();

        //AI 시스템
        BattleAI.UpdateEnemyAICard();
        BattleAI.UpdateEnemyAIThrow();
        BattleAI.UpdatePlayerAICard();
        BattleAI.UpdatePlayerAIThrow();

    }

    #region 공용함수

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

    #endregion

    #region 유닛 시스템 함수 BattleUnit

    /// <summary>
    /// 버튼함수. 유닛을 소환할 때의 팀
    /// </summary>
    public void ChangeTeam()
    {
        if(BattleUnit.eTeam.Equals(TeamType.MyTeam))
        {
            BattleUnit.eTeam = Utill.TeamType.EnemyTeam;
            _unitTeamText.text = "적의 팀";
            return;
        }
        if (BattleUnit.eTeam.Equals(Utill.TeamType.EnemyTeam))
        {
            BattleUnit.eTeam = Utill.TeamType.MyTeam;
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

    #endregion

    #region 코스트 시스템 함수 BattleCost

    public void RunUpgradeCostGrade()
    {
        BattleCost.RunUpgradeCostGrade();
    }

    #endregion

    #region 필통 시스템 함수 BattlePencilCase

    public void RunPencilCaseAbility()
    {
        BattlePencilCase.RunPencilCaseAbility();
    }

    #endregion

    #region 일시정지 시스템 함수 BattlePause

    public void SetPause()
    {
        BattlePause.SetPause();
    }

    #endregion
}
