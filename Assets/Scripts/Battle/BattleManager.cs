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

    [SerializeField, Header("카드시스템 BattleCard"), Space(30)]
    private CardCommand _commandCard;
    public CardCommand CommandCard => _commandCard;


    public List<CardMove> _cardDatasTemp = new List<CardMove>();

    [Header("어느 곳이든 유닛 소환 가능")]
    public bool isAnySummon = false;

    #endregion

    #region 유닛 시스템

    [SerializeField, Header("유닛시스템 BattleUnit"), Space(30)]
    private UnitCommand _commandUnit;
    public UnitCommand CommandUnit => _commandUnit;

    public List<Unit> _myUnitDatasTemp;
    public List<Unit> _enemyUnitDatasTemp;

    public TextMeshProUGUI _unitTeamText;

    #endregion

    #region 카메라 시스템

    [SerializeField, Header("카메라시스템 BattleCamera"), Space(30)]
    private CameraCommand _commandCamera;
    public CameraCommand CommandCamera => _commandCamera;

    #endregion

    #region 이펙트 시스템


    [SerializeField, Header("이펙트 시스템 BattleEffect"), Space(30)]
    private EffectCommand _commandEffect;
    public EffectCommand CommandEffect => _commandEffect;

    #endregion

    #region 던지기 시스템


    [SerializeField, Header("던지기 시스템 BattleThrow"), Space(30)]
    private ThrowCommand _commandThrow;
    public ThrowCommand CommandThrow => _commandThrow;

    #endregion

    #region 시간 시스템

    [SerializeField, Header("시간시스템 BattleTime"), Space(30)]
    private TimeCommand _commandTime;
    public TimeCommand CommandTime => _commandTime;

    #endregion

    #region 스테이지 AI 시스템

    [SerializeField, Header("AI 시스템 BattleAi"), Space(30)]
    private AICommand _commandAI;
    public AICommand CommandAI => _commandAI;


    #endregion

    #region 코스트 시스템

    [SerializeField, Header("코스트 시스템 BattleCost"), Space(30)]
    private CostCommand _commandCost = null;
    public CostCommand CommandCost => _commandCost;
    #endregion

    #region 필통 시스템

    [SerializeField, Header("필통시스템 BattlePencilCase"), Space(30)]
    private PencilCaseCommand _commandPencilCase = null;
    public PencilCaseCommand CommandPencilCase => _commandPencilCase;


    #endregion

    #region 일시정지 시스템

    [SerializeField, Header("일시정지시스템 Battle_Pause"), Space(30)]
    private PauseCommand _commandPause = null;
    public PauseCommand CommandPause => _commandPause;


    #endregion

    #region 승리 패배 시스템

    [SerializeField, Header("승리패배시스템 BattleWinLose"), Space(30)]
    private WinLoseCommand _commandWinLose = null;
    public WinLoseCommand CommandWinLose => _commandWinLose;

    #endregion

    private void Start()
    {
        _deckData = new DeckData();

        _commandPencilCase.SetInitialization(this);
        _commandCard.SetInitialization(this, _deckData ,_commandPencilCase.PencilCaseDataMy.PencilCasedataBase.maxCard);
        _commandCamera.SetInitialization(this);
        _commandUnit.SetInitialization(this);
        _commandEffect.SetInitialization(this);
        _commandThrow.SetInitialization(this, CurrentStageData);
        _commandAI.SetInitialization(this);
        _commandTime.SetInitialization(this);
        _commandCost.SetInitialization(this, _commandPencilCase.PencilCaseDataMy.PencilCasedataBase);
        _commandPause.SetInitialization(this);
        _commandWinLose.SetInitialization(this);

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
            _commandThrow.PullUnit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButton(0))
        {
            _commandThrow.DrawParabola(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _commandThrow.ThrowUnit();
        }

        //컴포넌트들의 업데이트가 필요한 함수 재생
        _updateAction.Invoke();
        
        //테스트용

        if (Input.GetKeyDown(KeyCode.X))
        {
            _commandCard.AddOneCard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _commandCard.AddAllCard();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _commandCard.ClearCards();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _commandCard.SubtractLastCard();
        }

        //유닛 시스템
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _commandUnit.ClearUnit();
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
        _commandUnit.DeletePoolUnit(unit);
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
