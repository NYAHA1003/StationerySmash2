using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Battle;

namespace Battle.Badge
{

	public class ThornBadge : AbstractBadge
	{
		public override void SetBadge(PencilCaseComponent pencilCaseCommand, PencilCaseUnit pencilCaseUnit, TeamType teamType, BadgeData badgeData)
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
		/// PCKill ��ȿ
		/// </summary>
		/// <param name="atkData"></param>
		public void RunThorn(AtkData atkData)
		{
			atkData.attacker.Run_Damaged(atkData);
			atkData.Reset_Kncockback(0, 0, 0, true); //true�� �ƴϸ� false����
		}
	}

}