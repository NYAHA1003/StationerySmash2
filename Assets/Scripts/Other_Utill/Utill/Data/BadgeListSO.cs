using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{

	[CreateAssetMenu(fileName = "BadgeListSO", menuName = "Scriptable Object/BadgeListSO")]
	public class BadgeListSO : ScriptableObject
	{
		public List<BadgeData> _badgeLists = new List<BadgeData>();
	}

}