using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

namespace Battle.Units
{

	public class SeFiceAttackState : AbstractAttackState
	{
		protected int _seFiceDamaged = 1;
		protected override void SetAttackData(ref AtkData atkData)
		{
			base.SetAttackData(ref atkData);
			_myUnit.SubtractHP(_seFiceDamaged);

		}
	}

}