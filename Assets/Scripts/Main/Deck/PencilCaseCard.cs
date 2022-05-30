using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utill.Data;
using Utill.Tool;


namespace Main.Deck
{
	public class PencilCaseCard : MonoBehaviour
	{
		[SerializeField]
		private Image _pencilCaseImage = null;
		[SerializeField]
		private TextMeshProUGUI _nameText = null;

		public void SetPencilCaseData(PencilCaseData pencilCaseData)
		{
			_pencilCaseImage.sprite = SkinData.GetSkin(pencilCaseData._skinType);
			_nameText.text = pencilCaseData._skinType.ToString();
		}
	}
}
