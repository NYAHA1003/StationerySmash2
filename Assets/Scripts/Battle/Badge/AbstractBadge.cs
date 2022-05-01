using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle;

namespace Battle.Badge
{

	public abstract class AbstractBadge
	{
		protected BattleManager _battleManager;
		protected PencilCaseComponent _pencilCaseCommand;
		protected PencilCaseUnit _pencilCaseUnit;
		protected TeamType _teamType;
		protected BadgeData _badgeData;
		public BadgeType _bageType;


		/// <summary>
		/// ���� ������ ����
		/// </summary>
		/// <param name="pencilCaseUnit"></param>
		/// <param name="badgeData"></param>
		public virtual void SetBadge(PencilCaseComponent pencilCaseCommand, PencilCaseUnit pencilCaseUnit, TeamType teamType, BadgeData badgeData)
		{
			_pencilCaseCommand = pencilCaseCommand;
			_teamType = teamType;
			_pencilCaseUnit = pencilCaseUnit;
			_badgeData = badgeData;
		}

		/// <summary>
		/// ��Ʋ�Ŵ��� ����
		/// </summary>
		/// <param name="battleManager"></param>
		public virtual void SetBattleManager(BattleManager battleManager)
		{
			_battleManager = battleManager;
		}

		/// <summary>
		/// ���� �ɷ� ���
		/// </summary>
		public abstract void RunBadgeAbility();

	}

}