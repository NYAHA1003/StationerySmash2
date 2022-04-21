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

    public CardComponent CommandCard => _commandCard;
    public UnitComponent CommandUnit => _commandUnit;
    public CameraComponent CommandCamera => _commandCamera;
    public EffectComponent CommandEffect => _commandEffect;
    public ThrowComponent CommandThrow => _commandThrow;
    public TimeComponent CommandTime => _commandTime;
    public AIComponent CommandAI => _commandAI;
    public CostComponent CommandCost => _commandCost;
    public PencilCaseComponent CommandPencilCase => _commandPencilCase;
    public PauseComponent CommandPause => _commandPause;
    public WinLoseComponent CommandWinLose => _commandWinLose;

    public TextMeshProUGUI _unitTeamText = null;

    [SerializeField]
    private StageDataSO _stageDataSO = null;
    private DeckData _deckData = null;
    private bool _isEndSetting = false;
    private Action _updateAction = () => { };

    [SerializeField, Header("카드시스템 BattleCard"), Space(30)]
    private CardComponent _commandCard = null;
    [SerializeField, Header("유닛시스템 BattleUnit"), Space(30)]
    private UnitComponent _commandUnit = null;
    [SerializeField, Header("카메라시스템 BattleCamera"), Space(30)]
    private CameraComponent _commandCamera = null;
    [SerializeField, Header("이펙트 시스템 BattleEffect"), Space(30)]
    private EffectComponent _commandEffect = null;
    [SerializeField, Header("던지기 시스템 BattleThrow"), Space(30)]
    private ThrowComponent _commandThrow = null;
    [SerializeField, Header("시간시스템 BattleTime"), Space(30)]
    private TimeComponent _commandTime = null;
    [SerializeField, Header("AI 시스템 BattleAi"), Space(30)]
    private AIComponent _commandAI = null;
    [SerializeField, Header("코스트 시스템 BattleCost"), Space(30)]
    private CostComponent _commandCost = null;
    [SerializeField, Header("필통시스템 BattlePencilCase"), Space(30)]
    private PencilCaseComponent _commandPencilCase = null;
    [SerializeField, Header("일시정지시스템 Battle_Pause"), Space(30)]
    private PauseComponent _commandPause = null;
    [SerializeField, Header("승리패배시스템 BattleWinLose"), Space(30)]
    private WinLoseComponent _commandWinLose = null;

    private void Start()
    {
        Application.targetFrameRate = 60;

        _deckData = new DeckData();

        _commandPencilCase.SetInitialization(CommandUnit, CurrentStageData);
        _commandCard.SetInitialization(this, CommandCamera, CommandUnit, CommandCost, ref _updateAction, CurrentStageData, _deckData, _commandPencilCase.PencilCaseDataMy.PencilCasedataBase.maxCard);
        _commandCamera.SetInitialization(CommandCard, CommandWinLose, ref _updateAction, CurrentStageData);
        _commandUnit.SetInitialization(ref _updateAction, CurrentStageData);
        _commandEffect.SetInitialization();
        _commandThrow.SetInitialization(ref _updateAction, _commandUnit, _commandCamera, CurrentStageData);
        _commandAI.SetInitialization(CommandPencilCase, CommandUnit, ref _updateAction);
        _commandTime.SetInitialization(ref _updateAction, CurrentStageData);
        _commandCost.SetInitialization(ref _updateAction, _commandPencilCase.PencilCaseDataMy.PencilCasedataBase);
        _commandPause.SetInitialization();
        _commandWinLose.SetInitialization(this);

        _isEndSetting = true;
    }

    private void Update()
    {
        //  Debug.Log("유닛 갯수 : " + _commandUnit.UnitParent.childCount + " FPS : " + 1.0f / Time.deltaTime);

        if (!_isEndSetting)
        {
            return;
        }

        //던지기 시스템
        if (Input.GetMouseButtonDown(0))
        {
            _commandThrow.PullUnit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(0))
        {
            _commandThrow.DrawParabola(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _commandThrow.ThrowUnit();
        }


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


        //컴포넌트들의 업데이트가 필요한 함수 재생
        _updateAction.Invoke();
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
        CommandPencilCase.RunPlayerPencilCaseAbility();
    }

    /// <summary>
    /// 클릭하면 일시정지함
    /// </summary>
    public void OnPause()
    {
        CommandPause.SetPause();
    }
}
