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
    /// �� ����
    /// </summary>
    /// <param name="teamType"></param>
    public void SetTeam(TeamType teamType)
    {
        _teamType = teamType;
    }

    /// <summary>
    /// ����ɷ� �ߵ�
    /// </summary>
    public abstract void RunPencilCaseAbility();

    /// <summary>
    /// AI�� ���� �ɷ� �ߵ� ����
    /// </summary>
    public abstract bool AIAbilityCondition();
    }
}