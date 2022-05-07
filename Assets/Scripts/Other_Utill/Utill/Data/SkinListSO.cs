using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
	[CreateAssetMenu(fileName = "SkinListSO", menuName = "Scriptable Object/SkinListSO")]
	public class SkinListSO : ScriptableObject
	{
		public List<CardNamingSkins> _cardNamingSkins = new List<CardNamingSkins>();
	}


	/// <summary>
	/// ī�尡 ������ �ִ� ��Ų ����Ʈ
	/// </summary>
	[System.Serializable]
	public class CardNamingSkins
	{
		public CardNamingType _cardNamingType = CardNamingType.None;
		public List<SkinData> _skinDatas = new List<SkinData>();
	}

}

