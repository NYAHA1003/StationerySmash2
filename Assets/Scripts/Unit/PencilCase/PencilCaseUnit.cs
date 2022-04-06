using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class PencilCaseUnit : Unit
{
    [SerializeField]
    private PencilCaseDataSO _pencilCaseData;
    public PencilCaseDataSO PencilCaseData => _pencilCaseData;
    public AbstractPencilCaseAbilityState AbilityState { get; private set; }

    /// <summary>
    /// 필통 데이터 초기화
    /// </summary>
    /// <param name="dataBase"></param>
    /// <param name="eTeam"></param>
    /// <param name="stageData"></param>
    /// <param name="id"></param>
    /// <param name="grade"></param>
    public override void SetUnitData(DataBase dataBase, TeamType eTeam, StageData stageData, int id, int grade)
    {
        _battleManager ??= FindObjectOfType<BattleManager>();
        base.SetUnitData(dataBase, eTeam, stageData, id, grade);
        SetPencilCaseAbility(_pencilCaseData.PencilCasedataBase);
    }

    /// <summary>
    /// 필통 능력 초기화
    /// </summary>
    /// <param name="ability_State"></param>
    /// <param name="pencilCaseData"></param>
    public void SetPencilCaseAbility(PencilCaseData pencilCaseData)
    {
        switch (pencilCaseData.pencilCaseType)
        {
            default:
            case PencilCaseType.Normal:
                AbilityState = PoolManager.GetPencilCase<NormalAbilityState>();
                break;
        }
    }

    /// <summary>
    /// 필통은 상태이상을 적용시키지 않음
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public override void AddStatusEffect(AtkType atkType, params float[] value)
    {

    }

}
