using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle.PCAbility
{

public abstract class AbstractPencilCaseAbility
{
    public BattleManager _battleManager;
    protected TeamType _teamType;

    public virtual void SetState(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    /// <summary>
    /// 팀 설정
    /// </summary>
    /// <param name="teamType"></param>
    public void SetTeam(TeamType teamType)
    {
        _teamType = teamType;
    }

    /// <summary>
    /// 필통능력 발동
    /// </summary>
    public abstract void RunPencilCaseAbility();

    /// <summary>
    /// AI의 필통 능력 발동 조건
    /// </summary>
    public abstract bool AIAbilityCondition();
    }
}