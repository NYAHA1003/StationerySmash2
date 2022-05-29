using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
	public static class ServerLinks
	{
		//EndPoint
		public readonly static string endPoint = "http://testsmash.kro.kr";

		//UserSaveData
		public readonly static string linkUserSaveData = "/UserSaveData";

		//Get
		public readonly static string getUnitData = "/UnitData/Get";
		public readonly static string getSticker = "/StickerData/Get";
		public readonly static string getStrategyData = "/StrategyData/Get";
		public readonly static string getBadgeData = "/BadgeData/Get";
		public readonly static string getPencilCaseData = "/PencilCaseData/Get";

		//Post
		public readonly static string postUnitData = "/UnitData/Post";
		public readonly static string postStrategyData = "/StrategyData/Post";
		public readonly static string postStickerData = "/StickerData/Post";
		public readonly static string postBadgeData = "/BadgeData/Post";
		public readonly static string postPencilCaseData = "/PencilCaseData/Post";
	}
}
