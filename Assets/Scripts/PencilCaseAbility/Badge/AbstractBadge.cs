using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;

public abstract class AbstractBadge 
{
    protected BattleManager _battleManager;
    protected PencilCaseCommand _pencilCaseCommand;
    protected PencilCaseUnit _pencilCaseUnit;
    protected TeamType _teamType;
    protected BadgeData _badgeData;


    /// <summary>
    /// 뱃지 데이터 설정
    /// </summary>
    /// <param name="pencilCaseUnit"></param>
    /// <param name="badgeData"></param>
    public void SetBadge(PencilCaseCommand pencilCaseCommand, PencilCaseUnit pencilCaseUnit, TeamType teamType, BadgeData badgeData)
    {
        _pencilCaseCommand = pencilCaseCommand;
        _teamType = teamType;
        _pencilCaseUnit = pencilCaseUnit;
        _badgeData = badgeData;
    }

    /// <summary>
    /// 배틀매니저 설정
    /// </summary>
    /// <param name="battleManager"></param>
    public virtual void SetBattleManager(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    /// <summary>
    /// 뱃지 능력 사용
    /// </summary>
    public abstract void RunBadgeAbility();

}
