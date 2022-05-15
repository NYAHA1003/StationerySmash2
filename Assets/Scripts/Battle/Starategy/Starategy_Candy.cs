using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Battle.Starategy
{

	public class Starategy_Candy : AbstractStarategy
	{
		public override void Run_Card(TeamType eTeam)
		{
			if (eTeam == TeamType.MyTeam)
			{
				Check_Range(_battleManager.UnitComponent._enemyUnitList, true, _card.CardDataValue.strategyData.starategyablityData);
			}
			else if (eTeam == TeamType.EnemyTeam)
			{
				Check_Range(_battleManager.UnitComponent._playerUnitList, false, _card.CardDataValue.strategyData.starategyablityData);
			}
		}
		private void Check_Range(List<Unit> list, bool isMyTeam, params float[] value)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			for (int i = 0; i < list.Count; i++)
			{
				if (isMyTeam)
				{
					if (list[i].transform.position.x < 0)
					{
						list[i].Run_Damaged(new AtkData(null, 10, 20, 0, 45, !isMyTeam, 90000 + i));
					}
				}
				else if (list[i].transform.position.x > 0)
				{
					list[i].Run_Damaged(new AtkData(null, 10, 20, 0, 45, !isMyTeam, 90000 + i));
				}
			}
		}
	}

}