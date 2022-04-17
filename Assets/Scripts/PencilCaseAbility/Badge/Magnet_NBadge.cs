using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;
public class Magnet_NBadge : AbstractBadge
{
    List<AbstractBadge> list;
    int num = -1;
    public override void SetBattleManager(BattleManager battleManager)
    {
        base.SetBattleManager(battleManager);
    }

    public override void SetBadge(PencilCaseCommand pencilCaseCommand, PencilCaseUnit pencilCaseUnit, TeamType teamType, BadgeData badgeData)
    {
        base.SetBadge(pencilCaseCommand, pencilCaseUnit, teamType, badgeData);
        if(teamType == TeamType.MyTeam)
        {
            list = _battleManager.CommandPencilCase.PlayerBadges;
        }
    }

    public override void RunBadgeAbility()
    {
        //throw new System.NotImplementedException();
    }

    public void Magnet_N(BadgeData badgeData)
    {
        if(list.Find(x => x._bageType == BadgeType.Magnet_S) != null)
        {
            num = 1;
        }
        _battleManager.CommandCost.AddCostSpeed(10 * num);
    }
}
