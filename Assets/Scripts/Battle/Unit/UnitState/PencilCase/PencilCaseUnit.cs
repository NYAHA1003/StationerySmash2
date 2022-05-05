using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle;
using Battle.PCAbility;

public class PencilCaseUnit : Unit
{
    [SerializeField]
    private PencilCaseDataSO _pencilCaseData;
    public PencilCaseDataSO PencilCaseData => _pencilCaseData;
    public AbstractPencilCaseAbility AbilityState { get; private set; }
    public Dictionary<System.Action<AtkData>, System.Action<AtkData>> _actionsAtkData = new Dictionary<System.Action<AtkData>, System.Action<AtkData>>();


    /// <summary>
    /// 필통 데이터 초기화
    /// </summary>
    /// <param name="dataBase"></param>
    /// <param name="eTeam"></param>
    /// <param name="stageData"></param>
    /// <param name="id"></param>
    /// <param name="grade"></param>
    public override void SetUnitData(CardData dataBase, TeamType eTeam, StageData stageData, int id, int grade, int orderIndex)
    {
        _battleManager ??= FindObjectOfType<BattleManager>();
        base.SetUnitData(dataBase, eTeam, stageData, id, grade, orderIndex);
        SetPencilCaseAbility(_pencilCaseData._pencilCaseData);
    }

    /// <summary>
    /// 필통 능력 초기화
    /// </summary>
    /// <param name="ability_State"></param>
    /// <param name="pencilCaseData"></param>
    public void SetPencilCaseAbility(PencilCaseData pencilCaseData)
    {
        switch (pencilCaseData._pencilCaseType)
        {
            default:
            case PencilCaseType.Normal:
                AbilityState = PoolManager.GetPencilCase<NormalAbility>();
                break;
            case PencilCaseType.Traffic:
                AbilityState = PoolManager.GetPencilCase<TrafficAbility>();
                break;
            case PencilCaseType.Wing:
                AbilityState = PoolManager.GetPencilCase<WingAbility>();
                break;
            case PencilCaseType.Soccer:
                break;
            case PencilCaseType.Gold:
                AbilityState = PoolManager.GetPencilCase<GoldAbility>();
                break;
            case PencilCaseType.Princess:
                AbilityState = PoolManager.GetPencilCase<PrincessAbility>();
                break;
            case PencilCaseType.Pencil:
                break;
            case PencilCaseType.Timer:
                AbilityState = PoolManager.GetPencilCase<TimerAbility>();
                break;
        }
    }

    /// <summary>
    /// 필통은 상태이상을 적용시키지 않음
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public override void AddStatusEffect(EffAttackType atkType, params float[] value)
    {

    }

    public override void Run_Damaged(AtkData atkData)
    {
        _unitStateChanger.UnitState.RunDamaged(atkData);
        if (_actionsAtkData.TryGetValue(Run_Damaged, out var name))
        {
            _actionsAtkData[Run_Damaged].Invoke(atkData);
        }
    }

    public override void AddInherence(AtkData atkData)
    {
        if (_actionsAtkData.TryGetValue(AddInherence, out var name))
        {
            _actionsAtkData[AddInherence].Invoke(atkData);
        }
        atkData.RunIncrease(this);
    }

    /// <summary>
    /// 공격데이터를 가진 액션 추가
    /// </summary>
    /// <param name="method"></param>
    /// <param name="addMethod"></param>
    public void AddDictionary(System.Action<AtkData> method, System.Action<AtkData> addMethod)
    {
        if (!_actionsAtkData.TryGetValue(method, out var name))
        {
            _actionsAtkData.Add(method, new System.Action<AtkData>((x) => { }));

        }
        _actionsAtkData[method] += addMethod;
    }
}
