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

    [SerializeField, Header("ī��ý��� BattleCard"), Space(30)]
    private CardComponent _commandCard = null;
    [SerializeField, Header("���ֽý��� BattleUnit"), Space(30)]
    private UnitComponent _commandUnit = null;
    [SerializeField, Header("ī�޶�ý��� BattleCamera"), Space(30)]
    private CameraComponent _commandCamera = null;
    [SerializeField, Header("����Ʈ �ý��� BattleEffect"), Space(30)]
    private EffectComponent _commandEffect = null;
    [SerializeField, Header("������ �ý��� BattleThrow"), Space(30)]
    private ThrowComponent _commandThrow = null;
    [SerializeField, Header("�ð��ý��� BattleTime"), Space(30)]
    private TimeComponent _commandTime = null;
    [SerializeField, Header("AI �ý��� BattleAi"), Space(30)]
    private AIComponent _commandAI = null;
    [SerializeField, Header("�ڽ�Ʈ �ý��� BattleCost"), Space(30)]
    private CostComponent _commandCost = null;
    [SerializeField, Header("����ý��� BattlePencilCase"), Space(30)]
    private PencilCaseComponent _commandPencilCase = null;
    [SerializeField, Header("�Ͻ������ý��� Battle_Pause"), Space(30)]
    private PauseComponent _commandPause = null;
    [SerializeField, Header("�¸��й�ý��� BattleWinLose"), Space(30)]
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
        //  Debug.Log("���� ���� : " + _commandUnit.UnitParent.childCount + " FPS : " + 1.0f / Time.deltaTime);

        if (!_isEndSetting)
        {
            return;
        }

        //������ �ý���
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


        //�׽�Ʈ��
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

        //���� �ý���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _commandUnit.ClearUnit();
        }


        //������Ʈ���� ������Ʈ�� �ʿ��� �Լ� ���
        _updateAction.Invoke();
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
        CommandPencilCase.RunPlayerPencilCaseAbility();
    }

    /// <summary>
    /// Ŭ���ϸ� �Ͻ�������
    /// </summary>
    public void OnPause()
    {
        CommandPause.SetPause();
    }
}
