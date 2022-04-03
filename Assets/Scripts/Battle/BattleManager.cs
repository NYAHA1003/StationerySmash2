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

    [SerializeField, Header("ī��ý��� BattleCard"), Space(30)]
    private CardCommand _commandCard;
    public CardCommand CommandCard => _commandCard;


    public List<CardMove> _cardDatasTemp = new List<CardMove>();

    [Header("��� ���̵� ���� ��ȯ ����")]
    public bool isAnySummon = false;

    #endregion

    #region ���� �ý���

    [SerializeField, Header("���ֽý��� BattleUnit"), Space(30)]
    private UnitCommand _commandUnit;
    public UnitCommand CommandUnit => _commandUnit;

    public List<Unit> _myUnitDatasTemp;
    public List<Unit> _enemyUnitDatasTemp;

    public TextMeshProUGUI _unitTeamText;

    #endregion

    #region ī�޶� �ý���

    [SerializeField, Header("ī�޶�ý��� BattleCamera"), Space(30)]
    private CameraCommand _commandCamera;
    public CameraCommand CommandCamera => _commandCamera;

    #endregion

    #region ����Ʈ �ý���


    [SerializeField, Header("����Ʈ �ý��� BattleEffect"), Space(30)]
    private EffectCommand _commandEffect;
    public EffectCommand CommandEffect => _commandEffect;

    #endregion

    #region ������ �ý���


    [SerializeField, Header("������ �ý��� BattleThrow"), Space(30)]
    private ThrowCommand _commandThrow;
    public ThrowCommand CommandThrow => _commandThrow;

    #endregion

    #region �ð� �ý���

    [SerializeField, Header("�ð��ý��� BattleTime"), Space(30)]
    private TimeCommand _commandTime;
    public TimeCommand CommandTime => _commandTime;

    #endregion

    #region �������� AI �ý���

    [SerializeField, Header("AI �ý��� BattleAi"), Space(30)]
    private AICommand _commandAI;
    public AICommand CommandAI => _commandAI;


    #endregion

    #region �ڽ�Ʈ �ý���

    [SerializeField, Header("�ڽ�Ʈ �ý��� BattleCost"), Space(30)]
    private CostCommand _commandCost = null;
    public CostCommand CommandCost => _commandCost;
    #endregion

    #region ���� �ý���

    [SerializeField, Header("����ý��� BattlePencilCase"), Space(30)]
    private PencilCaseCommand _commandPencilCase = null;
    public PencilCaseCommand CommandPencilCase => _commandPencilCase;


    #endregion

    #region �Ͻ����� �ý���

    [SerializeField, Header("�Ͻ������ý��� Battle_Pause"), Space(30)]
    private PauseCommand _commandPause = null;
    public PauseCommand CommandPause => _commandPause;


    #endregion

    #region �¸� �й� �ý���

    [SerializeField, Header("�¸��й�ý��� BattleWinLose"), Space(30)]
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

        //������ �ý���
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

        //������Ʈ���� ������Ʈ�� �ʿ��� �Լ� ���
        _updateAction.Invoke();
        
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
        _commandUnit.DeletePoolUnit(unit);
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
