using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;

public class ThornBadge : AbstractBadge
{
    public override void SetBadge(PencilCaseCommand pencilCaseCommand, PencilCaseUnit pencilCaseUnit, TeamType teamType, BadgeData badgeData)
    {
        base.SetBadge(pencilCaseCommand, pencilCaseUnit, teamType, badgeData);
        _pencilCaseUnit.AddDictionary(_pencilCaseUnit.AddInherence, RunThorn);
    }

    public override void SetBattleManager(BattleManager battleManager)
    {
        base.SetBattleManager(battleManager);
    }

    public override void RunBadgeAbility()
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// PCKill ¹«È¿
    /// </summary>
    /// <param name="atkData"></param>
    public void RunThorn(AtkData atkData)
    {
        atkData.attacker.Run_Damaged(atkData);
    }
}
