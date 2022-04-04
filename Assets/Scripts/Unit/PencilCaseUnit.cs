using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class PencilCaseUnit : Unit
{
    [SerializeField]
    private PencilCaseDataSO pencilCaseData;
    public PencilCaseAbilityState AbilityState { get; private set; }

    public override void SetUnitData(DataBase dataBase, TeamType eTeam, StageData stageData, int id, int grade)
    {
        base.SetUnitData(dataBase, eTeam, stageData, id, grade);
        SetPencilCaseAbility(pencilCaseData.PencilCasedataBase);
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
                AbilityState = new PencilCaseNormalAbilityState(_battleManager);
                break;
        }
    }
}
