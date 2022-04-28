using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class BlanketBadge : AbstractBadge
{
    public override void SetBadge(PencilCaseComponent pencilCaseCommand, PencilCaseUnit pencilCaseUnit, TeamType teamType, BadgeData badgeData)
    {
        base.SetBadge(pencilCaseCommand, pencilCaseUnit, teamType, badgeData);
        _pencilCaseUnit.AddDictionary(_pencilCaseUnit.AddInherence, PCKillCancle);
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
    public void PCKillCancle(AtkData atkData)
    {
        if(atkData.atkType == EffAttackType.PCKill)
        {
            atkData.Reset_Damage(atkData.damage / 2);
        }
    }
}
