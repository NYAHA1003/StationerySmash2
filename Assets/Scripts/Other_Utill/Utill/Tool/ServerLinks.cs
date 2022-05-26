using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
	public static class ServerLinks
	{
		public readonly static string endPoint = "http://testsmash.kro.kr";
		public readonly static string linkUserSaveData = "/UserSaveData";
		public readonly static string getSticker = "/smash/Get/Sticker";
		public readonly static string getUnitData = "/smash/Get/UnitData";
		public readonly static string getStrategyData = "/smash/Get/StrategyData";
		public readonly static string getBadgeData = "/smash/Get/BadgeData";
		public readonly static string getPencilCaseData = "/smash/Get/PencilCaseData";
	}
}
