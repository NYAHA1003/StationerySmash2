using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Main.Card;
using Main.Deck;

namespace Main.Deck
{
	public class CardSortChanger : MonoBehaviour
	{
		[SerializeField]
		private TMP_Dropdown _dropdown = null;

		[SerializeField]
		private DeckSettingComponent _deckSettingComponent = null;

		/// <summary>
		/// ��Ӵٿ� ���� ���Կ� ���� ���� ���ش�
		/// </summary>
		public void ChangeValueToSort()
		{
			_deckSettingComponent.ChangeSortSystem(_dropdown.value);
		}

	}
}
